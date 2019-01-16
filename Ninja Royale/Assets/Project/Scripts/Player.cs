using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour {
    [Header("Focal point variables")]
    [SerializeField] private GameObject focalPoint;
    [SerializeField] private GameObject dashTrail;
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
    private bool firstDashTime;
    ParticleSystem dashParticles;


    void Start () {
        firstDashTime = true;
        dash = false;
        forwardDashKeyCounter = 0;
        dashThresHold = 0.5f;
        m_Animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        dashParticles = dashTrail.GetComponent<ParticleSystem>();
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
            
            if (!dash)
            {
                
                float keyPressedDelta = firstDashTime? dashThresHold+1: Time.time - dashDelta;

                if (keyPressedDelta < dashThresHold )
                {
                    forwardDashKeyCounter = forwardDashKeyCounter + 1;
                    if (forwardDashKeyCounter == 1)
                    {
                        dash = true;
                        dashParticles.Play();
                        ParticleSystem.EmissionModule em = dashParticles.emission;
                        em.enabled = true;
                    }
                }
                
                dashDelta = Time.time;
                Debug.Log("dash counter: " + forwardDashKeyCounter);
                firstDashTime = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (dash && forwardDashKeyCounter == 1)
            {
                dash = false;
                forwardDashKeyCounter = 0;
                dashDelta = 0;
                dashParticles.Stop();
                ParticleSystem.EmissionModule em = dashParticles.emission;
                em.enabled = false;
            }

        }
    }

}
