using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(0f, 2f, -2f);
        transform.rotation = Quaternion.Euler(25f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}