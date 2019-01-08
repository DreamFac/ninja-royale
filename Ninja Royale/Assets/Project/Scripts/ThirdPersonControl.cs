using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class ThirdPersonControl : MonoBehaviour
{
    private bool _jump;
    private bool _running;
    private bool _isAttack;
    private Vector3 _move;
    private Transform _cam;
    private Vector3 _camForward;
    private ThirdPersonMovement thirdPersonMovement;

    // Use this for initialization
    void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            _cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }
        thirdPersonMovement = GetComponent<ThirdPersonMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        //read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        if (_cam != null)
        {
            // calculate camera relative direction to move:
            _camForward = Vector3.Scale(_cam.forward, new Vector3(1f, 0f, 1f)).normalized;
            _move = v * _camForward + h * _cam.right;
        }
#if !MOBILE_INPUT
        _running = Input.GetKey(KeyCode.LeftShift);
#endif
        thirdPersonMovement.Move(_move, crouch, _jump, _running, _isAttack);
        _jump = false;
    }
}
