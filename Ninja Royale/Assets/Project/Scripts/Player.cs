using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Anchor Point variables")]
    [SerializeField] private GameObject focalPoint;
    [SerializeField] private GameObject dashTrail;
    [SerializeField] private float focalDistance;
    [SerializeField] private float focalSmoothness;
    [SerializeField] public KeyCode changeFocalSideKey;
    [SerializeField] private GameObject gameCamera;
    [SerializeField] public float dashThresHold;
    public bool isFocalPointOnLeft = false;
    public bool dash = false;
    private float dashDelta;
    // Use this for initialization
    private int forwardDashKeyCounter;
    Animator m_Animator;
    Rigidbody m_Rigidbody;
    private bool enableClimb = false;
    private RigidbodyConstraints originalConstraints;
    private bool firstDashTime;
    ParticleSystem dashParticles;
    private Collider _collider;

    void Awake()
    {
        originalConstraints = RigidbodyConstraints.FreezeRotation;
    }


    void Start () {
        firstDashTime = true;
        dash = false;
        forwardDashKeyCounter = 0;
        dashThresHold = 0.5f;
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        _collider = m_Rigidbody.GetComponent<Collider>();
        dashParticles = dashTrail.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

        checkSide();
        checkAttack();
        checkDash();

        CheckInteractionStatus();

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

    void PrepareForClimb ()
    {
        m_Rigidbody.WakeUp();
        m_Rigidbody.useGravity = true;
        m_Rigidbody.constraints = originalConstraints;
        m_Animator.SetBool("EnableClimb", true);
        m_Animator.SetFloat("Climbing", 2f, 0.1f, Time.deltaTime);
    }

    void CheckInteractionStatus()
    {
        RaycastHit hitInfo;

#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(gameCamera.transform.position, gameCamera.transform.position + gameCamera.transform.forward * 4.5f, Color.green);
#endif
        // check if it hits something
        if (Physics.Raycast(gameCamera.transform.position, gameCamera.transform.forward, out hitInfo, 4.5f))
        {
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                PrepareForClimb();
                m_Rigidbody.AddForce(0f, 0.24f, 0f, ForceMode.VelocityChange);

            } else if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.D))
            {
                PrepareForClimb();
                m_Rigidbody.AddForce(0.5f, 0.22f, 0f, ForceMode.VelocityChange);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.A)))
            {
                PrepareForClimb();
                m_Rigidbody.AddForce(-0.5f, 0.22f, 0f, ForceMode.VelocityChange);
            }
            else if (Input.GetKey(KeyCode.LeftShift) && (Input.GetKey(KeyCode.S)))
            {
                PrepareForClimb();
                m_Rigidbody.AddForce(0f, 0.17f, 0f, ForceMode.VelocityChange);
            }
            else if (Input.GetKey(KeyCode.LeftShift))
            {
                m_Rigidbody.Sleep();
                m_Rigidbody.useGravity = false;
                m_Animator.SetBool("EnableClimb", true);
                m_Animator.SetFloat("Climbing", 2f, 0.1f, Time.deltaTime);
                m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
            }
            else
            {
                m_Rigidbody.useGravity = false;
                m_Animator.SetBool("EnableClimb", false);
                m_Rigidbody.constraints = originalConstraints;
                m_Animator.SetFloat("Climbing", 0f, 0.1f, Time.deltaTime);
            }
        }
        else
        {
            m_Rigidbody.useGravity = true;
            m_Animator.SetBool("EnableClimb", false);
            m_Rigidbody.constraints = originalConstraints;
            m_Animator.SetFloat("Climbing", 0f, 0.1f, Time.deltaTime);
        }
    }

}
