using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonController : MonoBehaviour
{
   [SerializeField] private Button _nextLevelButton;
   [SerializeField] private Button _retryButton;
    private void Awake()
    {
            Initialize();
    }
    private void Initialize()
    {
        _nextLevelButton.onClick.AddListener(()=>{
          EventManager.LevelEvents.Level_Completed.Invoke();  
        });
        _retryButton.onClick.AddListener(()=>{
            EventManager.LevelEvents.Level_Failed.Invoke();
        });
    }
}
