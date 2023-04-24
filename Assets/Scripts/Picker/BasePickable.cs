using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class BasePickable :MonoBehaviour, IPickable
{
    public Rigidbody Rigidbody{get => _rigidbody;}
    private Rigidbody _rigidbody;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();  
        _rigidbody.isKinematic = true; 
    }
    public void OnPicked()
    {
        _rigidbody.isKinematic = false;
    }
    public void OnUnLoaded()
    {
        _rigidbody.DOMoveZ(_rigidbody.position.z + 8F , 1f);
    }
    public void OnPlaced()
    {
        _rigidbody.isKinematic = true;
    }
    public void Deactivate()
    {
        Destroy(gameObject);
    }
    public void AddPickerForce(Vector3 force)
    {
        _rigidbody.AddForce(force , ForceMode.Impulse);   
    }
}
