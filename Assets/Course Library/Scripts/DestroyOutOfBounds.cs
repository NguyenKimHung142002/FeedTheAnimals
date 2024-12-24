using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOutOfBounds : MonoBehaviour
{
    private GroundBound groundBound;
    private bool checkBound;
    private float underBound = -22f;
    void Start()
    {
        groundBound = GameObject.FindGameObjectWithTag("Ground").GetComponent<GroundBound>();
    }
    // Update is called once per frame
    void Update()
    {
        checkBound = groundBound.OutBound(this.gameObject);
        if (checkBound || this.transform.position.y < underBound) 
        {
            Destroy(gameObject);
        }

    }
}
