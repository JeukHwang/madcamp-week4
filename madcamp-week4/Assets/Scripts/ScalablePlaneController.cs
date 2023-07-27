using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalablePlaneController : MonoBehaviour
{
    public int width;
    public int height;

    // Start is called before the first frame update
    void Start()
    {
        applyProperty();
    }

    void applyProperty()
    {
        transform.position = new Vector3(width - 1, transform.position.y, height - 1);
        transform.localScale = new Vector3(width / 5f, transform.localScale.y, height / 5f);
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(width / 8f, height / 8f);
    }

    // Update is called once per frame
    void Update()
    {
        applyProperty();
    }
}
