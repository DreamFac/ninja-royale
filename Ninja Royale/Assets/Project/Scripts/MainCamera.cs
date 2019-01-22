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

    [SerializeField] private float power = 0.1f;

    [SerializeField] private GameObject character;

    [SerializeField] private float dashCameraZ;
    [SerializeField] private float dashCameraZSmooth;

    private Player ninja;
    private float verticalRotationAngle;
    
    

    // Use this for initialization
    void Start()
    {
        
        followOffset = new Vector3(1f, -2.5f, 6f);
        translationOffset = new Vector3(0f, 1f, 0f);
        ninja = character.GetComponent<Player>();
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

        if (!ninja.forwardDashDoubleTap.trigger)
        {
            transform.position = target.transform.position - (rotation * followOffset);
            transform.LookAt(target.transform.position + translationOffset);
        }
        else
        {

            //Linear interpolation between transition
            followDashOffset = new Vector3(1f, -5f, dashCameraZ);
            Vector3 smoothDash = target.transform.position - (rotation * followDashOffset);
            transform.position = Vector3.Lerp(transform.position, smoothDash, dashCameraZSmooth);

            if (!ninja.IsSideDash())
            {
                //make camera shake
                transform.localPosition = transform.localPosition + Random.insideUnitSphere * power;
            }

            transform.LookAt(target.transform.position + translationOffset);
            
        }

        

        // Make camera look up or down
        verticalRotationAngle += Input.GetAxis("Mouse Y") * rotationSensitivity;
        verticalRotationAngle = Mathf.Clamp(verticalRotationAngle, minViewingAngle, maxViewingAngle);
        transform.RotateAround(rotationAnchorObject.transform.position, rotationAnchorObject.transform.right, -verticalRotationAngle);
        
        
    }
}