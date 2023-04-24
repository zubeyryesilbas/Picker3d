using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Picker : MonoBehaviour
{   
    private Movement _movement;
    private Action _onMovementStarted;
    private Collector _collector;
    private InputController _inputController;
    private bool _movementEnabled;
    private PickerProperities _pickerProperites;
    private void Awake()
    {
       Initialize();
    }
    // Update is called once per frame
    private void FixedUpdate()
    {   
        if(!_movementEnabled)
        {
            if(_inputController.InputX != 0)
            {
                _movementEnabled = true;
                _movement.StartMovement();
            }
        }
        else
        {
            _movement.Move(_inputController.InputX);
        }
    }
    private void Initialize()
    {  
        _pickerProperites = AssetManager.GetPickerProperitiesAtDirectory("input");
        _movement = new Movement(transform , 3.2f , _pickerProperites);
         _inputController = FindObjectOfType<InputController>();
       _collector = GetComponentInChildren<Collector>();
    }
    public void OnCheckPointReached()
    {
        _movement.StopMovement();
        _collector.UnLoadCollecteds();
       
    }
    public void OnFinishReached()
    {
       _movement.StopMovement();
       EventManager.CheckPointEvents.CheckPointPassed.Invoke();
    }
     
    public void CheckPointPassed()
    {
        _movement.StartMovement();
    }
    public void ForceMovement()
    {
        _movementEnabled = false;
        _movement.StartMovement();
    }
   
    
}
