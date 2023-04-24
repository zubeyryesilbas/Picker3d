using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class GameManager :SingletonComponent<GameManager>
{   
    private int _currentLevelIndex;
    [SerializeField] private LevelInstance[]  _levelInstanceModels;
    private LevelController _levelController;
    [SerializeField] private Transform _levelParentTransform;
    [SerializeField] Picker _picker;
    [SerializeField] CheckPointProgressionController _checkPointProgressController;
    private int _lastPlayedLevelIndex;
    private List<LevelInstance> _activeInstances = new List<LevelInstance>();
    private List<LevelInstance> _playedLevels = new List<LevelInstance>();
    private Vector3 _startPosition;
    [SerializeField] private bool _isEditMode;
    private void OnEnable()
    {
        EventManager.LevelEvents.Level_Completed += OnLevelCompleted;
        EventManager.LevelEvents.Level_Failed += OnLevelFailed;
    }
    private void OnDisable()
    {   
        EventManager.LevelEvents.Level_Completed -= OnLevelCompleted;
        EventManager.LevelEvents.Level_Failed -= OnLevelFailed;
    }
    private void OnLevelFailed()
    {   
        LevelInstance instance;
        if(_currentLevelIndex < _levelInstanceModels.Length)
            instance = _levelInstanceModels[_currentLevelIndex];
        else
            instance = _levelInstanceModels[UnityEngine.Random.Range(0 , _levelInstanceModels.Length)];
        
        var level = Instantiate(instance);
        level.transform.SetParent(_levelParentTransform);
        Vector3 pos = Vector3.zero;
        if(_currentLevelIndex > 0)
        {
            pos = _activeInstances[_currentLevelIndex].transform.position;
        }
        _picker.transform.SetParent(null);
          _levelController.ClearLevelContainer();
        _activeInstances[_currentLevelIndex] = level;
        level.transform.position = pos;
        _picker.gameObject.SetActive(false);
        StartNewLevel(_currentLevelIndex , level);
        _picker.gameObject.SetActive(true);
    }
    private void OnLevelCompleted()
    {   
        //_levelController.ClearLevelContainer();
        _currentLevelIndex +=1;
        var levelInstance = _activeInstances[_currentLevelIndex];
        _playedLevels.Add(levelInstance);
        StartNewLevel(_currentLevelIndex , levelInstance);
        if(_currentLevelIndex % 5 == 0)
        {
           AddNewAcitveLevels(5);
        }
        
    }
    protected override void Awake()
    {
       Initialize();
    }
    private void Start()
    {   
        var level = _activeInstances[_currentLevelIndex];
        StartNewLevel(_currentLevelIndex , level);
    }

    private void Initialize()
    {   
        if(!_isEditMode)
            _levelInstanceModels = AssetManager.GetLevelInstances("levels");
        else
            _levelInstanceModels = FindObjectsOfType<LevelInstance>();

        var sceneInstance = FindObjectOfType<LevelInstance>();
        AddNewAcitveLevels(10);
        sceneInstance.gameObject.SetActive(false);
    }
    private void AddNewAcitveLevels(int levelCount)
    {   
        var count = _activeInstances.Count();
         for(var i = count ; i < levelCount + count ; i ++)
        {   
            LevelInstance instance;
            if(i < _levelInstanceModels.Length)
            {   
                instance = Instantiate(_levelInstanceModels[i]);
            }
            else
            {
                instance = Instantiate(_levelInstanceModels[UnityEngine.Random.Range(0 , _levelInstanceModels.Length)]);
            }
            instance.transform.SetParent(_levelParentTransform);
            _activeInstances.Add(instance);
            if(i == 0)
            {
                instance.transform.localPosition = Vector3.zero;
            }
            else
            {
                instance.transform.position = _activeInstances[i-1]._finishPlatform.transform.position;
            }
                
        }
    }
    private void StartNewLevel(int levelIndex , LevelInstance levelInstance)
    {   
        _levelController = new LevelController(levelInstance.transform.position ,levelInstance , _levelParentTransform , _picker , _checkPointProgressController , _currentLevelIndex +1);
    }
}
  
