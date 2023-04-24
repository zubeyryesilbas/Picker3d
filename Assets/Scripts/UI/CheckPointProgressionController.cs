using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CheckPointProgressionController: Popup
{   
    [SerializeField] private Canvas _mainCanvas;
    [SerializeField] private CheckPointViewPart _viewStartPrefab;
    [SerializeField] private CheckPointViewPart _viewConnectionPrefab;
    private int _progressionCounter;

    private List<CheckPointViewPart> _views = new List<CheckPointViewPart>();

    private void OnEnable()
    {
        EventManager.CheckPointEvents.CheckPointPassed += UpdateProgression;
        EventManager.CheckPointEvents.FinishPointReached += UpdateProgression;
    }
    private void  OnDisable()
    {
       EventManager.CheckPointEvents.CheckPointPassed -= UpdateProgression;
       EventManager.CheckPointEvents.FinishPointReached -= UpdateProgression;
    }
    
    public void Initialize( int levelNumber, CheckPointPlatform [] _checkPoints)
    {   _progressionCounter  = 0;
        foreach(var view in _views)
        {
            Destroy(view.gameObject);
        } 
        _views.Clear();
        _views = new List<CheckPointViewPart>();
        for(var i = 0 ; i< _checkPoints.Length + 2 ; i ++)
        {   
            CheckPointViewPart viewPart;
            if(i == 0  || i == _checkPoints.Length +1)
                viewPart = Instantiate(_viewStartPrefab);
            else
                viewPart = Instantiate(_viewConnectionPrefab);
           
           viewPart.transform.SetParent(transform);
           _views.Add(viewPart);
            viewPart.SetViewUnPassed();
        }

        var startView = _views.First().GetComponent<CheckPointViewStart>();
        startView.SetCheckPointText(levelNumber);
        var viewLast = _views.Last().GetComponent<CheckPointViewStart>();
        viewLast.SetCheckPointText(levelNumber +1);
        _views.First().SetViewPassed();
        AlignTransforms();
    }
    private void UpdateProgression()
    {   
        _progressionCounter +=1;
        Debug.Log(_progressionCounter);
        for(var i = 0 ; i < _views.Count(); i++)
        {  
            if(_progressionCounter < i)
                break;

           if(i == 0  || i == _views.Count() - 1)
           {
                _views[i].SetViewPassed();
           }
           else
           {
                 _views[i].SetViewPassed();
           }
        }
    }
    private void AlignTransforms()
    {   
        var center = Vector3.zero;
        var centerIndex =(Mathf.RoundToInt(_views.Count() / 2));
        var isOddNumber = true;
        if( _views.Count() % 2 == 0)
        {
            isOddNumber = true;
        }
        for(var i = 0 ; i < _views.Count() ; i ++)
        {
            var pos = center + (i - centerIndex) * Vector3.right * 170f;
            if(isOddNumber)
                pos += Vector3.right* 85f;
            _views[i].transform.localPosition = pos ;  
        }
    }
}
