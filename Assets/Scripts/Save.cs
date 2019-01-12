using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int savePointIndex;
    public int playerLevel;
    public ScriptManager.State gameState;
    public float saveX, saveY, saveZ;
    
}