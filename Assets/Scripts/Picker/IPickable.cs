using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{   
    Rigidbody Rigidbody{get;}
    void OnPlaced();
    void OnPicked();
    void OnUnLoaded();
    void Deactivate();
    void AddPickerForce(Vector3 force);

}
