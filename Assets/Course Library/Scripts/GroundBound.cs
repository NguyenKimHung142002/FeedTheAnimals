using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class UpdateBoundEvent : UnityEvent<float, float, float, float>
{

}
public class GroundBound : MonoBehaviour
{
    #region 
    private Renderer groundRender;
    private float groundHeight;
    private float groundWidth;
    private float oneSixthHeight;
    private float oneSixthWidth;
    private float topBound;
    private float bottomBound;
    private float leftBound;
    private float rightBound;

    private Vector3 currentPosition;
    private GameObject player;
    public UpdateBoundEvent updateBoundEvent;
    #endregion

    void Start()
    {
        groundRender = gameObject.GetComponent<Renderer>();
        if (groundRender)
        {
            //Get Height and Withd of ground and its 1/9 center
            groundHeight = groundRender.bounds.size.z;
            groundWidth = groundRender.bounds.size.x;
            oneSixthHeight = groundHeight / 6;
            oneSixthWidth = groundWidth / 6;

            topBound = gameObject.transform.position.z + groundHeight / 2;
            bottomBound = gameObject.transform.position.z - groundHeight / 2;
            leftBound = gameObject.transform.position.x - groundWidth / 2;
            rightBound = gameObject.transform.position.x + groundWidth / 2;

            updateBoundEvent.Invoke(topBound, rightBound, leftBound, bottomBound);

        }
        currentPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        //update bound after update position
        bool resultOutCenterBound = OutCenterBound();
        if (resultOutCenterBound)
        {
            topBound = gameObject.transform.position.z + groundHeight / 2;
            bottomBound = gameObject.transform.position.z - groundHeight / 2;
            leftBound = gameObject.transform.position.x - groundWidth / 2;
            rightBound = gameObject.transform.position.x + groundWidth / 2;

            updateBoundEvent.Invoke(topBound, rightBound, leftBound, bottomBound);
        }
    }
    public float TopBound
    {
        get { return topBound; }
    }
    public float BottomBound
    {
        get { return bottomBound; }
    }
    public float LeftBound
    {
        get { return leftBound; }
    }
    public float RightBound
    {
        get { return rightBound; }
    }
    // check if object outside of ground
    public bool OutBound(GameObject other)
    {
        if (other.transform.position.z > topBound || other.transform.position.z < bottomBound || other.transform.position.x < leftBound || other.transform.position.x > rightBound)
        {
            return true;
        }
        return false;
    }
    //middle 1/9 bound of ground
    bool OutCenterBound()
    {

        if (player.transform.position.x < (leftBound + groundWidth / 3))
        {
            transform.position = new Vector3(transform.position.x - groundWidth / 3, transform.position.y, transform.position.z);
            return true;
        }
        else if (player.transform.position.x > (rightBound - groundWidth / 3))
        {
            transform.position = new Vector3(transform.position.x + groundWidth / 3, transform.position.y, transform.position.z);
            return true;
        }
        else if (player.transform.position.z > (topBound - groundHeight / 3))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + groundHeight / 3);
            return true;
        }
        else if (player.transform.position.z < (bottomBound + groundHeight / 3))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - groundHeight / 3);
            return true;
        }
        return false;
    }
}
