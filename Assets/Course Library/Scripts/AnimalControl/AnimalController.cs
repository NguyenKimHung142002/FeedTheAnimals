using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    private Vector3 moveDirection;
    private GameObject player;
    [SerializeField] private float speed = 15f;
    //[SerializeField] private float rotationSpeed = 960f;
    private Transform healthCanvas;
    private bool hitPlayer = false;
    private Vector3 lookAtPlayer;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        healthCanvas = transform.Find("Health Canvas");
        lookAtPlayer = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(Vector3.forward * Time.deltaTime * speed);
        // moveDirection = new Vector3 (player.transform.position.x - transform.position.x, 0, player.transform.position.z - transform.position.z);
        
        // moveDirection.Normalize();
        if (!hitPlayer)
        {            
            lookAtPlayer = player.transform.position;
            lookAtPlayer.y = 0;
            transform.LookAt(lookAtPlayer);
        }
        // transform.Translate(moveDirection * Time.deltaTime * speed);

        transform.Translate(Vector3.forward * Time.deltaTime * speed);

    }
    private void LateUpdate() {
        healthCanvas.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player"))
        {
            hitPlayer = true;
            Debug.Log("HitPlayer1");
        }
    }
}
