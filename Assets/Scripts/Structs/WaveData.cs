using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WaveData
{
    public string IntroText;
    public WaveEnemy[] Enemies;
}

[System.Serializable]
public struct WaveEnemy
{
    public Enemy Prefab;
    public int Amount;
}