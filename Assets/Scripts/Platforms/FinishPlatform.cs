using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinishPlatform :BasePlatform
{
    private void OnTriggerEnter(Collider other)
    {
        var picker = other.GetComponent<Picker>();
        if(picker != null)
        {
            picker.OnFinishReached();
            transform.DOMoveY(transform.position.y + 14f , 0.5f);
            PopupManager.Instance.OpenPopup(PopupType.Level_Complete);
        }
    }
}
