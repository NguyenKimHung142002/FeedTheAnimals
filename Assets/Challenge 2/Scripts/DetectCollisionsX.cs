using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollisionsX : MonoBehaviour
{
    private int score = 0;
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        score++;
        Debug.Log("Score: " + score);
    }
}
