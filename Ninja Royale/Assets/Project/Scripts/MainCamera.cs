using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class MainCamera : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject rotationAnchorObject;
    [SerializeField] private Vector3 followOffset;
    [SerializeField] private Vector3 followDashOffset;
    [SerializeField] private Vector3 translationOffset;

    [SerializeField] private float maxViewingAngle;
    [SerializeField] private float minViewingAngle;
    [SerializeField] private float rotationSensitivity;

    private Player player;


    private float verticalRotationAngle;
    

    // Use this for initialization
    void Start()
    {
        
        followOffset = new Vector3(1f, -2.5f, 6f);
        followDashOffset = new Vector3(1f, -5f, 12f);
        translationOffset = new Vector3(0f, 1f, 0f);
        player = GameObject.FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        //Make Camera look at target
        float yAngle = target.transform.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, yAngle, 0);
        Debug.Log("dash from camera: " + player.dash);

        if (!player.dash)
        {
            transform.position = target.transform.position - (rotation * followOffset);
            transform.LookAt(target.transform.position + translationOffset);
        }
        else
        {
            transform.position = target.transform.position - (rotation * followDashOffset);
            transform.LookAt(target.transform.position + translationOffset);
        }

        // Make camera look up or down
        verticalRotationAngle += Input.GetAxis("Mouse Y") * rotationSensitivity;
        verticalRotationAngle = Mathf.Clamp(verticalRotationAngle, minViewingAngle, maxViewingAngle);
        transform.RotateAround(rotationAnchorObject.transform.position, rotationAnchorObject.transform.right, -verticalRotationAngle);
        
        
    }
}