
using UnityEngine;

[System.Serializable]
public struct PlayerAttack {
    public string Name;
    public GameObject WeaponMesh;
    public AttackData attackData;
    public RectTransform uiAttack;
}