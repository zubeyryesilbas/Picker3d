using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class TutorialManager : SingletonComponent<TutorialManager>
{
    [SerializeField] private List<TutorialData> _tutorialsData;
    private Action _tutorialPassedAction;
    private TutorialData _activeTutorialData;
    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        if(_activeTutorialData == null)
        {
            foreach(var tutorialData in _tutorialsData)
            {
                tutorialData.Tutorial.gameObject.SetActive(false);
            }
        }
    }
    public void ShowTutorial(TutorialType tutorialType)
    {   
        _activeTutorialData = _tutorialsData.FirstOrDefault(x => x.TutorialType == tutorialType);
        _activeTutorialData.Tutorial.gameObject.SetActive(true);
        _activeTutorialData.Tutorial.Show();
    }
    public void PassTutorial()
    {
        _activeTutorialData.Tutorial.Hide();
        _activeTutorialData.IsPassed = true;
    }
}
[Serializable]
public class TutorialData
{
    public TutorialType TutorialType;
    public BaseTutorial Tutorial;
    [HideInInspector] public bool IsPassed;
}
