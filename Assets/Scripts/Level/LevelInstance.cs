using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstance : MonoBehaviour
{
    [SerializeField] private List<GameObject> _objectsIncluded = new List<GameObject>();
    public FinishPlatform _finishPlatform;
    private void Awake()
    {
        _finishPlatform = GetComponentInChildren<FinishPlatform>();
    }
    public void AddObject(GameObject gameObj)
    {
        if(!_objectsIncluded.Contains(gameObject))
        {
            _objectsIncluded.Add(gameObj);
        }
    }
    public void RemoveObject(GameObject gameObj)
    {    
        _objectsIncluded.RemoveAll(x => x ==null);
        if(_objectsIncluded.Contains(gameObject))
        {
            _objectsIncluded.Remove(gameObj);
        }
        _objectsIncluded.RemoveAll(x => x ==null); 
       
    }
    public List<GameObject> GetObjects()
    {
        return _objectsIncluded;
    }
}
