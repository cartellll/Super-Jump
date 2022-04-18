using System;
using System.Collections;
using System.Collections.Generic;
using SuperStars.Enums;
using SuperStars.Events;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool GetInput = true;
    public UpdateType UpdateType = UpdateType.Update;


    private void Update()
    {

        if (UpdateType != UpdateType.Update) return;
        if (!GetInput) return;

#if UNITY_EDITOR
        //Use Mouse Input
        if (Input.GetMouseButtonDown(0))
        {   
            OnInputStart(Input.mousePosition);
            return;
        }

        if (Input.GetMouseButton(0))
        {
            OnInput(Input.mousePosition);
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            OnInputEnd(Input.mousePosition);
            return;
        }
#else
        //Use Touch Input
        var touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
        
            OnInputStart(touch.position);
            return;
        }

        if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            OnInput(touch.position);
            return;
        }
        
        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            OnInputEnd(touch.position);
            return;
        }
#endif
    }

    private void OnInputStart(Vector3 input)
    {
        EventManager.TriggerEvent(InputEvents.Input_Start, input);
    }

    private void OnInput(Vector3 input)
    {
        EventManager.TriggerEvent(InputEvents.Input_Update, input);
    }

    private void OnInputEnd(Vector3 input)
    {
        EventManager.TriggerEvent(InputEvents.Input_End, input);
    }
}
