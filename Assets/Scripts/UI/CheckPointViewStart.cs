using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CheckPointViewStart : CheckPointViewPart
{
    [SerializeField] private TextMeshProUGUI _levelText;
    public void SetCheckPointText(int levelNumber)
    {
        _levelText.gameObject.SetActive(true);
        _levelText.text = "" + levelNumber;
    }
    
}
