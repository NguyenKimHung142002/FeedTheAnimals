using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class TextureResize : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor = 5;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2 (transform.localScale.x / scaleFactor ,  transform.localScale.z / scaleFactor);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.hasChanged && Application.isEditor && !Application.isPlaying)
        {
            GetComponent<Renderer>().sharedMaterial.mainTextureScale = new Vector2 (transform.localScale.x / scaleFactor , transform.localScale.z / scaleFactor);
            transform.hasChanged = false;
        }
    }
}
