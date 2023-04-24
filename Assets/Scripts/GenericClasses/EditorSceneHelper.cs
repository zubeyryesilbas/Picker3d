using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorSceneHelper : MonoBehaviour
{
   private void Awake()
   {
        if(FindObjectOfType<EditorCamera>() != null)
        {
            FindObjectOfType<EditorCamera>().gameObject.SetActive(false);
        }
   }
}
