using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CheckPointCounter : MonoBehaviour
{
    private int _targetCounter;
    private int _counter;
    private TextMeshPro _textMesh;
    private MeshRenderer _meshRenderer;
    private Vector3 _firstPos;
    private bool _checkPointPassed = false;
    private CheckPointPlatform _checkPointPlatform;
    private Picker _picker;
    [SerializeField] private Material _mat;

    public void Initialize(int target , CheckPointPlatform checkPointPlatform)
    {   
        _checkPointPlatform = checkPointPlatform;
        _counter = 0;
        _targetCounter = target;
        _textMesh = GetComponentInChildren<TextMeshPro>(true);
        _meshRenderer = GetComponent<MeshRenderer>();
        _textMesh.text = _counter +"/" + _targetCounter;
        _firstPos = new Vector3(transform.position.x , -3.43f , transform.position.z);
    }

    private void CheckPointPassed()
    {
        transform.DOLocalMoveY (-0.75f, 1f).OnComplete(()=>{
            _picker.CheckPointPassed();
            _checkPointPlatform.OnPassed();
        });
        _textMesh.enabled = false;
        _meshRenderer.material = _mat;
        _checkPointPassed = true;
        EventManager.CheckPointEvents.CheckPointPassed.Invoke();
    }
    public void SetPicker(Picker picker)
    {
        _picker = picker;
        Invoke(nameof(LevelFail), 2f);
    }
    private void OnCollisionEnter(Collision other)
    {
        var pickable = other.gameObject.GetComponent<IPickable>();

        if (pickable != null)
        {
            pickable.Deactivate();
            _counter +=1;
            _textMesh.text = _counter + "/" + _targetCounter;
            if(_counter >= _targetCounter)
            {   
                if(!_checkPointPassed)
                    CheckPointPassed();
            }
        }   
    }
    private void LevelFail()
    {
        if(!_checkPointPassed)
        {
            // EventManager.LevelEvents.Level_Failed.Invoke();
            PopupManager.Instance.OpenPopup(PopupType.Level_Fail);
            Debug.Log("Level Failed");
        }
    }
}

