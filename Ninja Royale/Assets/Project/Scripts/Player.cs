using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    [Header("Focal point variables")]
    [SerializeField] private GameObject focalPoint;
    [SerializeField] private float focalDistance;
    [SerializeField] private float focalSmoothness;
    [SerializeField] public KeyCode changeFocalSideKey;
    [SerializeField] public float dashThresHold;
    public bool isFocalPointOnLeft = false;
    public bool dash = false;
    private float dashDelta;
    // Use this for initialization
    private int forwardDashKeyCounter;
    Animator m_Animator;


    void Start () {
        forwardDashKeyCounter = 0;
        m_Animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }
	
	// Update is called once per frame
	void Update () {

        checkSide();
        checkAttack();
        checkDash();
       
        float targetX = focalDistance * (isFocalPointOnLeft ? -1 : 1);
        float smoothX = Mathf.Lerp(focalPoint.transform.localPosition.x, targetX, focalSmoothness * Time.deltaTime);
        focalPoint.transform.localPosition = new Vector3(smoothX, focalPoint.transform.localPosition.y, focalPoint.transform.localPosition.z);
    }

    void checkSide()
    {
        if (Input.GetKeyDown(changeFocalSideKey))
        {
            isFocalPointOnLeft = !isFocalPointOnLeft;
            m_Animator.SetBool("SwitchSide", isFocalPointOnLeft);
        }
    }

    void checkAttack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            m_Animator.SetTrigger("MainAttack");
        }
    }

    void checkDash()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("dash counter: " + forwardDashKeyCounter);
            if (!dash)
            {
                
                float keyPressedDelta = Time.time - dashDelta;

                if (keyPressedDelta < dashThresHold)
                {
                    forwardDashKeyCounter = forwardDashKeyCounter + 1;
                    if (forwardDashKeyCounter == 1)
                    {
                        dash = true;
                    }
                }
                
                dashDelta = Time.time;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (dash && forwardDashKeyCounter == 1)
            {
                dash = false;
                forwardDashKeyCounter = 0;
                dashDelta = 0;
            }

        }
    }

}
