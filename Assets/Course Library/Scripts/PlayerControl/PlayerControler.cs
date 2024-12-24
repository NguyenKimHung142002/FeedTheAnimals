using System;
using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    #region Variable
    private bool isGameOver = false;
    private float getHorizontalInput;
    private float getVerticalInput;
    private Vector3 moveDirection;
    public Vector3 MoveDirection
    {
        get { return moveDirection; }
    }
    public bool isMoving;
    private Rigidbody rb;

    private bool canDash = true;
    [SerializeField] private float dashingPower = 20f;
    private float dashingTime = 0.3f;
    private float dashingCooldown = 1;
    [SerializeField] private GameObject trailRenderObject;

    [SerializeField] private ParticleSystem runParticle;
    private Animator playerAnim;
    private TrailRenderer tr;

    private Camera cam;
    [SerializeField] private float speed = 20f;
    //[SerializeField] private float rotationSpeed = 960f;

    [Header("PROJECTILE")]
    [SerializeField] private int boosterType = 2;
    public int BoosterType
    {
        set { boosterType = value; }
    }
    [SerializeField] private GameObject[] projectilePrefabs;
    [SerializeField] private Transform projectileParent;

    private GroundBound ground;
    private Vector3 lookPos;

    [Header("SHOT GUN ")]
    [SerializeField] private float fireRate = 0.75f;
    [SerializeField] private int numberOfBullet = 6;
    [SerializeField] private float shotgunRange = 3;
    [SerializeField] private float sreadRange = 35f;
    private float nextFire = 0f;
    private Vector3 offset = new Vector3(0, 2.2f, 1.5f);

    [Header("Audio")]
    [SerializeField] private AudioSource dashSound;
    [SerializeField] private AudioSource throwSound;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        rb = gameObject.GetComponent<Rigidbody>();
        ground = GameObject.FindWithTag("Ground").GetComponent<GroundBound>();
        cam = Camera.main;
        tr = trailRenderObject.GetComponent<TrailRenderer>();
        playerAnim = gameObject.GetComponent<Animator>();

        // handle event from get hit
        PlayerGetHit eventGameOverHandle = gameObject.GetComponent<PlayerGetHit>();
        eventGameOverHandle.eventGameOver.AddListener(PlayerKnockDown);

        runParticle.Stop();

        if (tr)
        {
            tr.emitting = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isGameOver == false)
        {
            //ReboundPosition();
            // key down to shoot
            if (Input.GetMouseButtonDown(0) && Time.time > nextFire)
            {
                Vector3 projectilePros = transform.rotation * offset;
                throwSound.Play();
                if (boosterType == 1)
                {
                    Instantiate(projectilePrefabs[1], transform.position + projectilePros, transform.rotation, projectileParent);
                }
                else if (boosterType == 2)
                {
                    for (int i = 0; i < numberOfBullet; i++)
                    {
                        shotgunShoot(projectilePros);
                    }
                }
                else
                {
                    Instantiate(projectilePrefabs[0], transform.position + projectilePros, transform.rotation, projectileParent);
                }
                nextFire = Time.time + fireRate;
            }
            LookAtMouse();
        }
    }

    private void shotgunShoot(Vector3 projectilePros)
    {
        float spreadRangeRandom = UnityEngine.Random.Range(-sreadRange, sreadRange);
        Quaternion spread = Quaternion.Euler(0, spreadRangeRandom, 0) * transform.rotation;

        GameObject shotgunBullet = Instantiate(projectilePrefabs[2], transform.position + projectilePros, spread, projectileParent);
        Destroy(shotgunBullet, shotgunRange);
    }

    private void FixedUpdate()
    {
        if (isGameOver == false)
        {
            MoveCharacter();

            //dash
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.LeftShift)) && canDash)
            {
                StartCoroutine(Dash());
                
            }
        }

    }
    private IEnumerator Dash()
    {
        canDash = false;
        isMoving = true;
        tr.emitting = true;
        dashSound.Play();
        rb.velocity = moveDirection * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        rb.velocity = Vector3.zero;
        tr.emitting = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
        isMoving = false;

    }

    #region look at mouse
    void LookAtMouse()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lookPos = hit.point;
        }
        Vector3 lookDir = lookPos - transform.position;
        lookDir.y = 0;
        transform.LookAt(transform.position + lookDir, Vector3.up);
    }

    #endregion
    void MoveCharacter()
    {
        // get key to move player
        getHorizontalInput = Input.GetAxisRaw("Horizontal");
        getVerticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(getHorizontalInput, 0, getVerticalInput);
        moveDirection.Normalize();

        if (getHorizontalInput != 0 || getVerticalInput != 0)
        {
            isMoving = true;
            runParticle.Play();
            playerAnim.SetFloat("Speed_f", 1);
            transform.Translate(moveDirection * Time.deltaTime * speed, Space.World);
        }
        else
        {
            isMoving = false;
            playerAnim.SetFloat("Speed_f", 0);
            runParticle.Stop();
        }
    }

    void PlayerKnockDown()
    {
        playerAnim.SetBool("Death_b", true);
        isGameOver = true;
        runParticle.Stop();
    }


}
