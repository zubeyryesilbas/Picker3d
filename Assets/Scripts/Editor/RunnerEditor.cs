using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;

public class RunnerEditor : EditorWindow
{    
    [MenuItem("LevelEditor/RunnerEditor")]
    public static void ShowWindow(){
        GetWindow<RunnerEditor>("RunnerEditor");
    }
    [SerializeField] public LevelInstance  _levelsInstance;
    private LevelInstance _sceneInstanceLevel;
    private List<SpawnableObject> _spawnableObjects = new List<SpawnableObject>();
    private List<SpawnableObject> _runnerPlatforms;
    private int _selectedObjectIndex = 0;
    private int _selectedLevelIndex = 0;
    private int _selectedPlatformIndex = 0;
    [SerializeField]  private GameObject _currentSelection;
    private Color _currentObjectColor = Color.white;
    private Color _currentLevelPlatFormColor;
    private bool _showConfirmation;
    private string _lastLevelsName;
    private int _lastLevelNumber;
    private int _selectedSpawnedIndex;
    private GameObject _lastSpawnedObject;
    private EditorCamera _editorCamera;
    private float _selectedObjectXValue;
    private float _posX , _posY , _posZ , _rotX ,_rotY , _rotZ , _zScale;
    private Dictionary<IntegerType , int> _integerDictionary = new Dictionary<IntegerType, int>();
    private Rect _levelSection;
    private Rect _objectSection;
    private Rect _spawnSection;

    static Texture2D GetPrefabPreview(string path)
    {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        var editor = UnityEditor.Editor.CreateEditor(prefab);
        Texture2D tex = editor.RenderStaticPreview(path, null, 200, 200);
        EditorWindow.DestroyImmediate(editor);
        return tex;
    }
    private  void OnEnable()
    {   
        RegisterIndexes();
    }
    private void RegisterIndexes()
    {
         if(!_integerDictionary.ContainsKey(IntegerType.selecetedObject))
            _integerDictionary.Add(IntegerType.selecetedObject , _selectedObjectIndex);

        if(!_integerDictionary.ContainsKey(IntegerType.selectedPlatform ))
            _integerDictionary.Add(IntegerType.selectedPlatform , _selectedPlatformIndex);

        if(!_integerDictionary.ContainsKey(IntegerType.selectedLevelIndex))
            _integerDictionary.Add(IntegerType.selectedLevelIndex , _selectedLevelIndex);
        
        if(!_integerDictionary.ContainsKey(IntegerType.spawnedIndex))
            _integerDictionary.Add(IntegerType.spawnedIndex , _selectedSpawnedIndex);
        
    }
    private void OnGUI()
    {   
        if(Application.isPlaying)
            return;

        RegisterIndexes();
        DrawLevelSection();
        if(_editorCamera == null)
        {
            _editorCamera = FindObjectOfType<EditorCamera>();
            _editorCamera.gameObject.SetActive(false);
            _editorCamera.gameObject.SetActive(true);
        }
        if(_sceneInstanceLevel == null)
            _sceneInstanceLevel = FindObjectOfType<LevelInstance>();
        var platforms =  AssetManager.GetAllGameObjectsAtDirectory("platforms");
        var spawnableObjects = AssetManager.GetAllGameObjectsAtDirectory("spawnables");
        var levels = AssetManager.GetAllGameObjectsAtDirectory("levels");
        var hasSavedLevel = false;
        if(levels != null)
        {   
            if(levels.Length > 0)
            {   
                hasSavedLevel = true;
                levels = levels.OrderBy(level => level.name).ToArray();
                _lastLevelsName = levels.Last().name;
                _lastLevelNumber = int.Parse(Regex.Match(_lastLevelsName , @"(?<=level)\d+").Value);    
            }
            else
            {
                _lastLevelNumber = 0;
            }
        }
       
        GUILayout.BeginHorizontal();
        
        if(hasSavedLevel)
        {    
            GUILayout.Label("Completed Levels");
             if(_levelsInstance != null)
            {
                SerializedObject serializedObject = new SerializedObject(this);
                SerializedProperty serializedProperty = serializedObject.FindProperty("_levelsInstance");
                EditorGUILayout.PropertyField(serializedProperty , true);
                serializedObject.ApplyModifiedProperties();
            }
            _levelsInstance = ShowArrayOfGameObjects(levels , IntegerType.selectedLevelIndex).GetComponent<LevelInstance>();
             if(GUILayout.Button("Edit Level"))
            {
                EditLevel();
            }
        }
        else
        {   
            if(_sceneInstanceLevel == null)
                GUILayout.Label("There is no Level. Create New Level");
        }
        if(_sceneInstanceLevel != null)
        {
             GUILayout.Label("Editing " + _sceneInstanceLevel.name);
        }

        if(GUILayout.Button("New Level"))
        {
            CreateNewLevel();
        }
       if(_sceneInstanceLevel == null)
        { 
            GUILayout.EndHorizontal();
            return;
        }  
        if(GUILayout.Button("Save Level"))
        {
            SaveLevel();
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Platform Selection", EditorStyles.boldLabel);
        GUILayout.Label("Spawnable Selection", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        var showedPlatform = ShowArrayOfGameObjects(platforms , IntegerType.selectedPlatform).gameObject;
        var showedSpawnable = ShowArrayOfGameObjects(spawnableObjects , IntegerType.selecetedObject);
        
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        ShowPreviewOfPrefab(showedPlatform);
        ShowPreviewOfPrefab(showedSpawnable);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Spawn objects ", EditorStyles.boldLabel);
        GUILayout.Label("Spawn Platforms ", EditorStyles.boldLabel);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if(_selectedPlatformIndex> -1 && _selectedPlatformIndex > -1)
        {    
            if (GUILayout.Button("Spawn Platform"))
            {  
               var spawnable = platforms[_integerDictionary[IntegerType.selectedPlatform]];
               var objectNumber = 1;
               var countOfsameNamedObjects = 0;
                foreach (GameObject obj in _sceneInstanceLevel.GetObjects())
                {
                    if (obj.name.StartsWith(spawnable.name))
                    {   
                        countOfsameNamedObjects +=1;
                    }
                }
                objectNumber += countOfsameNamedObjects;
               var spawnedObject =  PrefabUtility.InstantiatePrefab(spawnable)as GameObject;
               spawnedObject.name = spawnable.name + objectNumber;
               spawnedObject.transform.parent = _sceneInstanceLevel.transform;
               _sceneInstanceLevel.AddObject(spawnedObject.gameObject);
                _integerDictionary[IntegerType.spawnedIndex] = _sceneInstanceLevel.GetObjects().Count - 1;
            }
        }
        if(_integerDictionary[IntegerType.selecetedObject] > -1 && _integerDictionary[IntegerType.selectedLevelIndex] > -1)
        {   
            if(_sceneInstanceLevel != null)
            if (GUILayout.Button("Spawn Object"))
            {   
               var spawnable = spawnableObjects[_integerDictionary[IntegerType.selecetedObject]];
               var objectNumber = 1;
               var countOfsameNamedObjects = 0;
                foreach (GameObject obj in _sceneInstanceLevel.GetObjects())
                {
                    if (obj.name.StartsWith(spawnable.name))
                    {   
                        countOfsameNamedObjects +=1;
                    }
                }
                objectNumber += countOfsameNamedObjects;
               var spawnedObject =  PrefabUtility.InstantiatePrefab(spawnable)as GameObject;
               spawnedObject.transform.parent = _sceneInstanceLevel.transform;
               spawnedObject.name = spawnable.name + objectNumber;
                _sceneInstanceLevel.AddObject(spawnedObject);
               spawnedObject.transform.parent = _sceneInstanceLevel.transform;
               _integerDictionary[IntegerType.spawnedIndex] = _sceneInstanceLevel.GetObjects().Count - 1;
            }
        }

        GUILayout.EndHorizontal();
        if(_sceneInstanceLevel != null)
        {   
            if(_sceneInstanceLevel.GetObjects() != null && _sceneInstanceLevel.GetObjects().Count() >0)
            {   
                GUILayout.Label("Select Spawned Object or Platform" , EditorStyles.boldLabel);
                ShowArrayOfGameObjects(_sceneInstanceLevel.GetObjects().ToArray() ,IntegerType.spawnedIndex);
                if(_sceneInstanceLevel.GetObjects().Count() >0)
                {  
                    GUILayout.Label("Game Object's position offsets");
                    if(_sceneInstanceLevel == null)
                        return;
                    var currentObject = _sceneInstanceLevel.GetObjects().ToArray()[_integerDictionary[IntegerType.spawnedIndex]];
                    var platform = currentObject.GetComponent<BasePlatform>();
                    _rotX = currentObject.transform.eulerAngles.x;
                    _rotY = currentObject.transform.eulerAngles.y;
                    _rotZ = currentObject.transform.eulerAngles.z;
                    _posX = currentObject.transform.localPosition.x / 5f;
                    _posY = currentObject.transform.localPosition.y / 10f;
                    _posZ = currentObject.transform.localPosition.z / 200f;
                    _zScale = currentObject.transform.localScale.z;

                    GUILayout.BeginHorizontal();
                    if(platform == null)
                    {
                        GUILayout.Label("X Offset");
                        GUILayout.Label("Y Offset");
                    }
                    
                    GUILayout.Label("Z Offset");
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    if(platform == null)
                    {
                         _posX = EditorGUILayout.Slider(_posX ,-1f , 1f);
                        _posY = EditorGUILayout.Slider(_posY ,0f , 1f);
                    }
                   
                    _posZ = EditorGUILayout.Slider(_posZ ,0f , 1f);
                    GUILayout.EndHorizontal();
                   
                    if(platform == null)
                    {   
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Game Object's rotation offsets");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("X Rotation");
                        GUILayout.Label("Y Rotation");
                        GUILayout.Label("Z Rotation");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        _rotX = EditorGUILayout.Slider(_rotX ,0f , 360f);
                        _rotY = EditorGUILayout.Slider(_rotY,0f , 360f);
                        _rotZ = EditorGUILayout.Slider(_rotZ ,0f , 360f);
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Platform's z scale");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Z Scale");
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal();
                        _zScale = EditorGUILayout.Slider(_zScale , 0.1f , 20f);
                        GUILayout.EndHorizontal();
                        var scale = currentObject.transform.localScale;    
                        scale.z = _zScale;
                        currentObject.transform.localScale = scale;
                        var checkPointPlatform =  platform.GetComponent<CheckPointPlatform>();
                        if(checkPointPlatform != null)
                        {
                            var amount = checkPointPlatform.RequiredObjectsToPass;
                            GUILayout.Label("Set Amount for passing checkPoint");
                            amount = EditorGUILayout.IntField(amount);
                            checkPointPlatform.RequiredObjectsToPass = amount;
                            EditorUtility.SetDirty(checkPointPlatform);
                            
                        }
                    }
                    if(_editorCamera != null)
                         _editorCamera.Focus(currentObject.transform);
                    GUILayout.BeginHorizontal();   
                    currentObject.transform.eulerAngles = new Vector3(_rotX , _rotY , _rotZ);
                    currentObject.transform.localPosition = new Vector3(_posX * 5 , _posY *10 , _posZ * 200f);
                    
                    GUILayout.EndHorizontal();
                    if(_sceneInstanceLevel != null)
                    if (GUILayout.Button("Delete object or platform selected"))
                    {   
                        if(PrefabUtility.IsPartOfAnyPrefab(_sceneInstanceLevel.gameObject))
                            PrefabUtility.UnpackPrefabInstance(_sceneInstanceLevel.gameObject,PrefabUnpackMode.OutermostRoot ,InteractionMode.UserAction );
                        
                        DestroyImmediate(currentObject);
                         _sceneInstanceLevel.RemoveObject(currentObject);
                    }
                }  
            }
        }
    }
    private GameObject ShowArrayOfPlatforms(BasePlatform [] platforms , IntegerType integerType)
    {   
        EditorGUILayout.BeginHorizontal();
        for (int i = 0; i < platforms.Length; i++)
        {     
            if (GUILayout.Toggle(i == _integerDictionary[integerType], platforms[i].name, "Button"))
            {
                _integerDictionary[integerType] = i;
            } 
            if(i % 4 == 0 && i != 0)
            {
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
            } 
        }
       EditorGUILayout.EndHorizontal();

        if (_integerDictionary[integerType] >= 0 && _integerDictionary[integerType] < platforms.Length)
        {
            SpawnableObject selectedObject = new SpawnableObject();
            selectedObject.Name = platforms[_integerDictionary[integerType]].name;
            selectedObject.Prefab = platforms[_integerDictionary[integerType]].gameObject;
            return selectedObject.Prefab;
        }
        return null;
    }
    private GameObject ShowArrayOfGameObjects(UnityEngine.Object[] objects, IntegerType integerType)
    {    
        EditorGUILayout.BeginHorizontal();
        
        string[] objectNames = new string[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            objectNames[i] = objects[i].name;
        }

        _integerDictionary[integerType] = EditorGUILayout.Popup(_integerDictionary[integerType], objectNames);

        EditorGUILayout.EndHorizontal();

        if (_integerDictionary[integerType] >= 0 && _integerDictionary[integerType] < objects.Length)
        {
            SpawnableObject selectedObject = new SpawnableObject();
            selectedObject.Name = objects[_integerDictionary[integerType]].name;
            if(integerType == IntegerType.selectedPlatform)
                selectedObject.Prefab = (GameObject)objects[_integerDictionary[integerType]];
            else
                selectedObject.Prefab = (GameObject)objects[_integerDictionary[integerType]];
            return selectedObject.Prefab;
        }
        return null;
    }

    private void ShowPreviewOfPrefab(GameObject prefabObject)
    {
       if (prefabObject != null)
        {
            EditorGUILayout.LabelField(prefabObject.name, EditorStyles.boldLabel);
            Texture2D texture = GetPrefabPreview(AssetDatabase.GetAssetPath(prefabObject));
            GUILayout.Box(texture , GUILayout.Height(64), GUILayout.Width(64));
        }
    }  
    private void OnFocus()
    {
        _currentSelection = Selection.activeGameObject;
       //SetColor(_currentObjectColor);
    }
    private void EditLevel()
    {
        if(_levelsInstance != null)
        {   
            if(_sceneInstanceLevel ==null)
             {
                 _sceneInstanceLevel = PrefabUtility.InstantiatePrefab(_levelsInstance) as LevelInstance;
                _sceneInstanceLevel.name = _levelsInstance.name;
             }
             else
             {
                ShowMessageBox(CreateSelectedInstanceAfterDelete , "Delete Current Level Instance at scene" , "Are you sure you want to delete level instance at scene  and create new one?");
             }
        }
    }
    private void OnLostFocus()
    {
        _currentSelection = null;
    }
    private void LoadLevel()
    {
        string filePath = EditorUtility.OpenFilePanel("Select level Instance model", "Resources/levels", "prefab");
        if (!string.IsNullOrEmpty(filePath))
        {
            // Remove the file extension from the file path before passing it to Resources.Load()
            if(_sceneInstanceLevel!= null)
                Destroy(_sceneInstanceLevel.gameObject);
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            _levelsInstance = Resources.Load<LevelInstance>("levels/" + fileName);
            EditLevel();
        }
    }
    private void SaveLevel()
    {
        SaveSelectedPrefabAsAsset();
    }

    void SaveSelectedPrefabAsAsset()
    {   
        if (_sceneInstanceLevel != null)
        {   
            var defaultPath = "Assets/Resources/levels/"; 
            var assetPath = EditorUtility.SaveFilePanelInProject("Save Prefab", _sceneInstanceLevel.name, "prefab", "Save prefab asset" , defaultPath);
            Debug.Log(assetPath);
            if (assetPath != "")
            {
                UnityEngine.Object prefab = PrefabUtility.SaveAsPrefabAsset(_sceneInstanceLevel.gameObject, assetPath);
                Debug.Log("Prefab saved as asset at " + assetPath);
            }
        }
    }
   
    private void DrawLevelSection()
    {
        _levelSection.x = 0;
        _levelSection.y = 0;
        _levelSection.width = Screen.width;
        _levelSection.height = 50;
    }
    private void CreateNewLevel()
    {   
        if(_sceneInstanceLevel != null)
            _showConfirmation = true;
        if(_showConfirmation)
        {
            ShowMessageBox(DeleteLevelInstanceAtScene , "Delete level instance at scene ", "Are you sure you want to delete level instance at scene?");
            _showConfirmation = false;
        }
        if(_sceneInstanceLevel == null)
        {
           CreateNewLevelInstance();
        }
    }
    private void CreateNewLevelInstance()
    {
        var levelInstanceObject = new GameObject();
        levelInstanceObject.name ="level" +( _lastLevelNumber + 1);
        _sceneInstanceLevel =  levelInstanceObject.AddComponent<LevelInstance>();
    }
    private void CreateSelectedLevelInstance()
    {
        _sceneInstanceLevel = PrefabUtility.InstantiatePrefab(_levelsInstance) as LevelInstance;
        _sceneInstanceLevel.name = _levelsInstance.name;
    }
    private void CreateSelectedInstanceAfterDelete()
    {   
        DeleteLevelInstanceAtScene();
        CreateSelectedLevelInstance();
    }
    private void DeleteLevelInstanceAtScene()
    {
        DestroyImmediate(_sceneInstanceLevel.gameObject);
    }
    private void ShowMessageBox(Action onYes ,string title, string message)
    {
        bool result = EditorUtility.DisplayDialog(title , message , "Yes", "No");
        if (result)
        {
            // User clicked "Yes" - delete the object
           onYes.Invoke();
        }
    }
    private void SetColor(Color color)
    {
        if(_currentSelection != null)
        {
            var renderer = _currentSelection.GetComponent<Renderer>();
            if(renderer != null)
                renderer.sharedMaterial.color = color;
        }
    }

    public enum IntegerType {
        selecetedObject,
        selectedPlatform,
        spawnedIndex,
        selectedLevelIndex,
    }
}
