using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodsClasses : MonoBehaviour
{
    [SerializeField]
    private GameObject[] foodsBoost;
    public GameObject[] FoodsBoost
    {
        get { return foodsBoost; }
    }
}
