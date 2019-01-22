using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTap {

    private KeyCode key;
    private float keyPressDelta;
    private float tapThreshold;
    private float triggerDelta;
    private int keyCounter;

    public bool trigger;
    private bool firstDashTime;

    private float pressDelta;
    


    public DoubleTap(KeyCode key, float tapThreshold)
    {
        this.key = key;
        this.tapThreshold = tapThreshold;

        keyCounter = 0;
        pressDelta = 0;
        trigger = false;
        firstDashTime = true;
    }

    public void CheckDoubleTapUp(Action activate, Action deactivate)
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("pressing key: "+key.ToString());
            if (!trigger)
            {
                
                float keyPressedDelta = firstDashTime ? tapThreshold + 1 : Time.time - triggerDelta;

                if (keyPressedDelta < tapThreshold)
                {
                    Debug.Log("allowed threshold");
                    keyCounter = keyCounter + 1;
                    if (keyCounter == 1)
                    {
                        Debug.Log("trigger activated");
                        trigger = true;
                        activate();
                    }
                }

                triggerDelta = Time.time;
                firstDashTime = false;
            }
        }
        if (Input.GetKeyUp(key))
        {
            if (trigger && keyCounter == 1)
            {
                Debug.Log("trigger deactivated");
                trigger = false;
                keyCounter = 0;
                triggerDelta = 0;
                deactivate();
            }

        }
    }

    public void CheckDoubleTapTime(Action activate, Action deactivate, float delta)
    {
        if (Input.GetKeyDown(key))
        {
            Debug.Log("pressing key: " + key.ToString());
            if (!trigger)
            {

                float keyPressedDelta = firstDashTime ? tapThreshold + 1 : Time.time - triggerDelta;

                if (keyPressedDelta < tapThreshold)
                {
                    Debug.Log("allowed threshold");
                    keyCounter = keyCounter + 1;
                    if (keyCounter == 1)
                    {
                        Debug.Log("trigger activated");
                        trigger = true;
                        pressDelta = Time.time;
                        activate();
                    }
                }

                triggerDelta = Time.time;
                firstDashTime = false;
            }
        }
        if (Time.time - pressDelta > delta)
        {
            if (trigger && keyCounter == 1)
            {
                Debug.Log("trigger deactivated");
                trigger = false;
                keyCounter = 0;
                triggerDelta = 0;
                pressDelta = 0;
                deactivate();
            }
            

        }

        
    }


}
