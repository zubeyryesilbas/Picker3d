using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
public class BaseTutorial : MonoBehaviour
{  
    public void Hide()
    {   
        gameObject.SetActive(false);
    }
    public void Show()
    {   
        gameObject.SetActive(true);
    }
}
