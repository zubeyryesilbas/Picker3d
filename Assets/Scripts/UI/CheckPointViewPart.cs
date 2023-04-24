using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CheckPointViewPart : MonoBehaviour
{
    [SerializeField] private Image _checkPointImage;
    public void SetViewUnPassed()
    {
        _checkPointImage.color = Color.gray;
    }
    public void SetViewPassed()
    {
        _checkPointImage.color = Color.green;
    }
}
