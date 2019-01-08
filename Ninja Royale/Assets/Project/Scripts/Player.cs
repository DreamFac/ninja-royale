using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    bool _fire;
    Animator m_Animator;
    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) m_Animator.SetTrigger("Mouse Primary");
    }
}
