using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem  explosionParticle;
    private Transform particleFolder;
    [SerializeField] private float explosionRadius = 5f;
    private void Start()
    {
        particleFolder = GameObject.Find("ParticleManager").transform;
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Animal"))
        {
            Explode();
            Destroy(gameObject);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    private void Explode()
    {
        //initiate explosion effect
        ParticleSystem explosion = Instantiate(explosionParticle, transform.position, Quaternion.identity, particleFolder);
        explosion.Play();
        Destroy(explosion.gameObject, explosion.main.startLifetime.constant);

        //find all the colliders within the explosion radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider obj in colliders)
        {
            DetectCollision animal = obj.gameObject.GetComponent<DetectCollision>();
            if (animal)
            {
                animal.TakeDamged();
        
            }
        }

        Destroy(gameObject);
    }
}
