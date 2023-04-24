using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AssetManager 
{
    public static GameObject[] GetAllGameObjectsAtDirectory(string directory)
    {
        var objects = Resources.LoadAll<GameObject>(directory);
        return objects;
    }
    public static ScriptableObject GetScriptableObjectAtDirectory(string directory)
    {
        var scriptableObject = Resources.Load<ScriptableObject>(directory);
        return scriptableObject;
    }
    public static PickerProperities GetPickerProperitiesAtDirectory(string directory)
    {
        var pickerProperities = Resources.Load<PickerProperities>(typeof(PickerProperities).Name);
        return pickerProperities;
    }
    public static LevelInstance [] GetLevelInstances(string directory)
    {
        var levelInstances = Resources.LoadAll<LevelInstance>(directory);
        return levelInstances;
    }
    public static LevelInstance  GetLevelInstanceWithName(string name)
    {
        var levelInstances = Resources.Load<LevelInstance>(name);
        return levelInstances;
    }
    public static Picker GetPicker(string directory)
    {
        var picker = Resources.Load<Picker>(typeof(Picker).Name);
        return picker;
    }
}
