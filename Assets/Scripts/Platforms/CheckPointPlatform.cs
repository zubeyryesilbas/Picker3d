using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CheckPointPlatform : BasePlatform
{
    public PlatformType platformType;
    public bool ISpassed = false;
    [SerializeField]  public int RequiredObjectsToPass;
    private CheckPointCounter _checkPointCounter;
    private Transform _gate1;
    private Transform _gate2;
    private Picker _picker;
    private void Awake()
    {
        Initialize();
    }
  
    private void Initialize()
    {
        _checkPointCounter = GetComponentInChildren<CheckPointCounter>();
        _checkPointCounter.Initialize(RequiredObjectsToPass , this );
        _gate1 = transform.Find("Gate1");
        _gate2 = transform.Find("Gate2");
    }

    private void  OnTriggerEnter(Collider other)
    {   
        _picker = other.GetComponent<Picker>();
        if(_picker != null)
        {
            _picker.OnCheckPointReached();
            _checkPointCounter.SetPicker(_picker);
        }   
    }
    public void OnPassed()
    {
        _gate1.transform.DORotate(new Vector3(-60,90,90), 1f);
        _gate2.transform.DORotate(new Vector3(60,90,90), 1f).OnComplete(()=>
        {
            _picker.CheckPointPassed();
            ISpassed = true;
        });
    }

}
