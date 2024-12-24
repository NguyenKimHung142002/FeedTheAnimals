using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Numerics;
public class DetectCollision : MonoBehaviour
{
    [SerializeField] private int point = 1;
    [SerializeField] private Slider healthBar;
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private ParticleSystem hitParticle;
    private Transform particleFolder;
    private int currentHealth = 0;

    private PlayerCanvasUiController playerUi;
    private FoodsClasses getFood;
    private GameObject foodManager;
    private bool isDestroyed = false;
    [Header("AUDIO")]
    private AudioSource animalAudio;
    [SerializeField] private AudioClip getHitAC;
    [SerializeField] private AudioClip deadAC;
    private void Start()
    {
        healthBar.maxValue = maxHealth;
        particleFolder = GameObject.Find("ParticleManager").transform;
        foodManager = GameObject.Find("FoodManager");
        animalAudio = gameObject.GetComponent<AudioSource>();
        if (foodManager)
        {
            getFood = foodManager.GetComponent<FoodsClasses>();
        }
        playerUi = GameObject.FindObjectOfType<PlayerCanvasUiController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {

            ParticleSystem hitObject = Instantiate(hitParticle, transform.position, other.transform.rotation, particleFolder);
            hitObject.Play();
            Destroy(hitObject.gameObject, hitObject.main.startLifetime.constant);
            Destroy(other.gameObject);
            TakeDamged();
        }
    }
    public void TakeDamged()
    {
        currentHealth++;
        healthBar.value = currentHealth;
        animalAudio.PlayOneShot(getHitAC);

        if (currentHealth >= healthBar.maxValue)
        {
            if (isDestroyed == false)
            {
                isDestroyed = true;

                Invoke("DestroyGameObject", 0.1f);
                if (getFood)
                {
                    DropBoost();
                }
            }
        }
    }
    //code when em is destroyed
    private void OnDestroy()
    {
        if (isDestroyed == true)
        {
            playerUi.UpdateScore(point);
            animalAudio.PlayOneShot(deadAC);
        }
    }
    private void DestroyGameObject()
    {       
        Destroy(gameObject);  
    }

    private void DropBoost()
    {
        int ran = Random.Range(0, 4);
        GameObject spawnFood = getFood.FoodsBoost[ran];
        Instantiate(spawnFood, transform.position, UnityEngine.Quaternion.identity, foodManager.transform);
    }
}

