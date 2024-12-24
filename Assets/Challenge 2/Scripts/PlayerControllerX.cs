using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject dogPrefab;
    [SerializeField] private float fireRate = 0.5f;
    private float nextFire = 0f;
    GameObject dog;
    // Update is called once per frame
    void Update()
    {
        // On spacebar press, send dog
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire && dog == null)
        {
            nextFire = Time.time + fireRate;
            dog = Instantiate(dogPrefab, dogPrefab.transform.position, dogPrefab.transform.rotation);
            Debug.Log("Dog: " + dog);
        }
    }
}
