using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager 
{
  
    public struct InputEvents
    {
        public static Action<Vector3> InputMoved;
    }
    public struct CheckPointEvents
    {
        public static Action CheckPointPassed;
        public static Action FinishPointReached;
    }
    public struct LevelEvents
    {
        public static Action Level_Completed;
        public static Action Level_Failed;
    }
}
