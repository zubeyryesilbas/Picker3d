using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Collector : MonoBehaviour
{     
   [SerializeField] private List<GameObject> _collectedObjects = new List<GameObject>();
    public void Initialize(Action checkPointAction)
    {
        checkPointAction += UnLoadCollecteds;
    }
    public void UnLoadCollecteds()
    {   
        Debug.Log("Unloaded");
        foreach(var pickable in _collectedObjects)
        {   
            if(pickable == null)
                continue;
            pickable.GetComponent<IPickable>().OnUnLoaded();
        }
        _collectedObjects.Clear();
        _collectedObjects = new List<GameObject>();
    }
    private void Update()
    {
        Debug.Log(_collectedObjects.Count);
    }
    private void FixedUpdate()
    {
        foreach(var pickable in _collectedObjects)
        {   
            if(pickable != null)
                pickable.GetComponent<IPickable>().AddPickerForce(GetComponentInParent<Rigidbody>().velocity);
        }
    }

    private void OnTriggerStay(Collider other)
    {   
        if(other.CompareTag("collectable"))
        {
            if(!_collectedObjects.Contains(other.gameObject))
            {
                _collectedObjects.Add(other.gameObject);
                other.GetComponent<IPickable>().OnPicked();
            }
        } 
    }
    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("collectable"))
        {
            if(_collectedObjects.Contains(other.gameObject))
            {
                _collectedObjects.Remove(other.gameObject);
            }
        }
    }
}
