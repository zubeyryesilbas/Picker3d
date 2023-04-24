using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private Vector3 _anchorPosition;
    private Vector3 _deltaPos;
    public Vector3 DeltaPos{get => _deltaPos;}
    public float InputX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        InputX = 0;
        if(Input.GetMouseButtonDown(0))
        {   
            _anchorPosition = Input.mousePosition;
           // EventManager.InputEvents.InputMoved.Invoke(_deltaPos);
        }
        else if(Input.GetMouseButton(0))
        {
            InputX = (Input.mousePosition.x - _anchorPosition.x);
            if(Mathf.Abs(InputX) > 1)
                InputX = Mathf.Sign(InputX);
            else
                InputX = 0f;
            _anchorPosition = Input.mousePosition;
        }
    }
}
