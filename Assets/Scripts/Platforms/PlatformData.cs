using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PlatformData 
{
   readonly public PlatformType platformType;
   public int RequiredCollectableCount;
   public void RemoveCollectableCount()
   {
        RequiredCollectableCount -= 1;
   }
}
