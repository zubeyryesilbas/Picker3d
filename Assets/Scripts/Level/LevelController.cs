using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;


public class LevelController
{
   public LevelInstance _levelInstance;
   private Transform _levelParentTransform;
   private Picker _picker;
   private CheckPointPlatform[] _checkPointplatforms;
   private CheckPointProgressionController _checkPointProgressionController;
   private int _currentLevelNumber;

   public LevelController( Vector3 pickerPos ,LevelInstance levelInstance , Transform levelParentTransform , Picker picker , CheckPointProgressionController checkPointProgressionController , int levelNumber)
   {  
        _currentLevelNumber = levelNumber;
        _levelParentTransform = levelParentTransform;
         _levelInstance = levelInstance;
        _checkPointplatforms = _levelInstance.GetComponentsInChildren<CheckPointPlatform>();
        PopupManager.Instance.OpenPopup(PopupType.Progression);
        _checkPointProgressionController = checkPointProgressionController;
        _checkPointProgressionController.Initialize(_currentLevelNumber  , _checkPointplatforms);
        _levelInstance.transform.SetParent(_levelParentTransform);
        _picker = picker;
        _picker.ForceMovement();
        _picker.transform.SetParent(_levelInstance.transform);
        _picker.transform.localPosition = Vector3.zero;

        //_picker.transform.localPosition = Vector3.zero;

   }
   public void ClearLevelContainer()
   {    
        MonoBehaviour.Destroy(_levelInstance.gameObject);
   }
}