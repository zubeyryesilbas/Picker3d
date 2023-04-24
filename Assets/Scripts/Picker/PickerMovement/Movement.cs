using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement 
{
    private Transform _movementTransform;
    private float _constraint;
    private PickerProperities _pickerProperties;
    private bool _canMove;
    public Movement(Transform tr , float constraint , PickerProperities pickerProperties)
    {
        _movementTransform = tr;
        _constraint = constraint;
        _pickerProperties = pickerProperties;
    }

    public void Move(float xInput)
    {   
        if(_canMove)
        {
            var pos = _movementTransform.position;
            pos += xInput* Time.deltaTime * Vector3.right * _pickerProperties.SideMovementSpeed + 
            Vector3.forward * Time.deltaTime * _pickerProperties.ForwardMovementSpeed;
            if(!((pos.x > _constraint && xInput > 0) || (pos.x <-_constraint && xInput < 0)))
                _movementTransform.position = pos;
        }
    }
    public void StopMovement()
    {
        _canMove = false;
    }
    public void StartMovement()
    {
        _canMove = true;
    }
}
