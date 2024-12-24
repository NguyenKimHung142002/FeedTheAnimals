using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
public class BossController : MonoBehaviour
{
    private Vector3 moveDirection;
    private GameObject player;
    [SerializeField] private float speed = 15f;
    private Transform healthCanvas;

    private float distance;
    [SerializeField] private float moveRange = 20;
    private float closeRange = 7;

    [Header("Fire")]
    [SerializeField] private GameObject projectilePrefabs;
    [SerializeField] private Transform projectileParent;
    private Vector3 offset = new Vector3(0, 4f, 4.5f);

    private Vector3 guessAttack = new Vector3(0, 0, 5f);
    
    [SerializeField] private float fireRate = 3f;
    private bool canAttack = false;
    private PlayerControler checkPlayerMoving;
    private Vector3 lookAtPlayer;
    [SerializeField] private float multiPositionGuess = 5;
    [SerializeField] private bool allowAttack = false;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            checkPlayerMoving = player.GetComponent<PlayerControler>();
        }
        healthCanvas = transform.Find("Health Canvas");
        StartCoroutine(BossAttack());
        lookAtPlayer = player.transform.position;
    }
    void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);

    }

    private IEnumerator BossAttack()
    {
        while (allowAttack)
        {   
            //Khi nhan vat di chuyen thi add offset de dan gan vi tri nhan vat dang dung hon
            if (checkPlayerMoving.isMoving)
            {
                transform.LookAt(player.transform.position + (checkPlayerMoving.MoveDirection * multiPositionGuess));
                Debug.Log("Is Moving");
            }    
            
            if (canAttack)
            {
                Vector3 addOffset = transform.rotation * offset;
                Instantiate(projectilePrefabs, transform.position + addOffset, transform.rotation, projectileParent);
            }
        
            canAttack = false;
            yield return new WaitForSeconds(fireRate);
            canAttack = true;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        lookAtPlayer = player.transform.position;
        lookAtPlayer.y = 0;
        transform.LookAt(lookAtPlayer);

        if (distance >= moveRange)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
        } else if (distance + closeRange < moveRange )
        {
            transform.Translate(Vector3.back * Time.deltaTime * speed);
        }
    }
    private void LateUpdate()
    {
        transform.LookAt(lookAtPlayer);
        healthCanvas.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

}
