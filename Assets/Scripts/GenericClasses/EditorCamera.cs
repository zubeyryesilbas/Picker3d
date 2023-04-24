using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    public void Focus(Transform tr)
    {
        transform.position = tr.position + -Vector3.right * 3 + Vector3.up  * 15 * tr.localScale.z;
        transform.eulerAngles = new Vector3(90 , 0 ,0);
    }
}
