
using UnityEngine;

[System.Serializable]
public struct PlayerAttack {
    public string Name;
    public bool isUnlocked;
    public float Prize;
    public GameObject WeaponMesh;
    public AttackData attackData;
    public RectTransform uiAttack;
}