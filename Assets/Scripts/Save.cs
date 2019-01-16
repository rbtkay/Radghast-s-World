using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int playerLevel;
    public double playerSouls;
    public int maxHPPots, maxMPPots;
    public ScriptManager.State gameState;
    public float saveX, saveY, saveZ;
    public List<string> roundTowers;
    public List<string> darkTowers;
    public List<string> listNPC;
}