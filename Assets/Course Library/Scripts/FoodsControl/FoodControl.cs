using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum FoodsType
{
    Apple = 3,
    RottenApple = 4,
    BulletBanana = 2,
    BulletBomb = 1
}
public class FoodControl : MonoBehaviour
{
    [SerializeField]
    private FoodsType foodType;
    public int FoodType{
        get { return (int)foodType; }
    }
    [SerializeField] private float boostDuration = 5;
    public float BoostDuration {
        get { return boostDuration; }
    }
    [SerializeField] private float rotateSpeed = 20;
    [SerializeField] private ParticleSystem foodParticle;
    [SerializeField] private Material foodMaterialColor;
    public Material FoodMaterialColor{
        get { return foodMaterialColor;}
    }
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float suckRange = 8f;


    private Vector3 distance;
    private GameObject player;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        foodParticle.Play();
        player = GameObject.Find("Player");
        rb = gameObject.GetComponent<Rigidbody>();

        //getMaterial
        Renderer assignMaterial = foodParticle.GetComponent<Renderer>();
        assignMaterial.material = foodMaterialColor;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (player != null)
        {
            MoveObjectTowardPlayer();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, suckRange);
    }
    private void MoveObjectTowardPlayer()
    {
        distance = player.transform.position - transform.position;
        Vector3 moveDirection = distance.normalized;
        if (distance.magnitude < suckRange)
        {
            if (rb.velocity.magnitude <= moveSpeed)
            {
                rb.velocity = moveDirection * moveSpeed;
            }
            else if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = Vector3.zero;
            }
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void LateUpdate()
    {
        transform.Rotate(Vector3.up * rotateSpeed * Time.fixedDeltaTime);
    }
}
