using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PlayerGetHit : MonoBehaviour
{
    // [SerializeField]
    // private UnityEvent playerGetHit;
    private float nextHit = 0;
    [SerializeField]
    private float hitInterval = 0.75f;
    [SerializeField]
    private Slider healthBar;
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private ParticleSystem  hitParticle;
    [SerializeField] private ParticleSystem boostParticle;
    private Renderer boostParticleRenderer;
    private PlayerControler playerControler;

    private bool isBoosting = false;
    private Transform particleFolder;
    public UnityEvent eventGameOver;

    [Header("Audio")]
    [SerializeField] private AudioSource getHitAS;
    [SerializeField] private AudioSource deadAS;
    [SerializeField] private AudioSource getBoostAS;

    private void Start()
    {
        healthBar.maxValue = maxHealth;
        particleFolder = GameObject.Find("ParticleManager").transform;
        playerControler = gameObject.GetComponent<PlayerControler>();
        boostParticle.Stop();
        boostParticleRenderer = boostParticle.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Animal"))
        {
            //Init particle
            ParticleSystem hitObject = Instantiate(hitParticle, transform.position, other.transform.rotation, particleFolder);
            hitObject.Play();

            Destroy(hitObject.gameObject, hitObject.main.startLifetime.constant);
            if (nextHit <= Time.time)
            {
                nextHit = hitInterval + Time.time;
                TakeDamage(1);
                //playerGetHit.Invoke();        
            }
        }

        if(other.gameObject.CompareTag("FoodBoost"))
        {
            FoodControl foodHealthType = other.gameObject.GetComponent<FoodControl>();
       
            //green apple
            if (foodHealthType.FoodType == 3)
            {
                getBoostAS.Play();
                FoodHealth(other, 1);
            }

            //rotten apple
            if (foodHealthType.FoodType == 4)
            {
                getHitAS.Play();
                FoodHealth(other, -1);
            }

            if (foodHealthType.FoodType == 2 || foodHealthType.FoodType == 1)
            {
                if (isBoosting == false)
                {
                    StartCoroutine(GetFoodBoost(foodHealthType));
                    Destroy(other.gameObject, 0.2f);
                }
            }
        }
    }
    private IEnumerator GetFoodBoost(FoodControl foodHealthType)
    {
        isBoosting = true;
        boostParticleRenderer.material = foodHealthType.FoodMaterialColor;
        boostParticle.Play();
        getBoostAS.Play();
        playerControler.BoosterType = foodHealthType.FoodType;
        yield return new WaitForSeconds (foodHealthType.BoostDuration);
        playerControler.BoosterType = 0;
        isBoosting = false;
        boostParticle.Stop();
    }
    private void TakeDamage(int dmgValue)
    {
        healthBar.value -= dmgValue;
        getHitAS.Play();
        if (healthBar.value <= 0)
        {
            Debug.Log("Lost");
            deadAS.Play();
            eventGameOver.Invoke();
        }
    }
    private void FoodHealth(Collider food, int boostValue)
    {
        if (healthBar.value < maxHealth)
        {
            healthBar.value += boostValue;
            Destroy(food.gameObject);
        }
    }
}
