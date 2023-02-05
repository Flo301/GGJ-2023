
using UnityEngine;

[System.Serializable]
public struct PlayerAttack {
    public string Name;
    public bool isUnlocked;
    public float Price;
    public Sprite Icon;
    public GameObject WeaponMesh;
    public AttackData attackData;
    public RectTransform uiAttack;
}