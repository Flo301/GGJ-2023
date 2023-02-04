using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public string Name;
    public Sprite Icon;
    public EAttackTyp Typ;
    public float Damage;
    public float Range;
    public float Cooldown;
}

public enum EAttackTyp
{
    Close = 1,
    Radial = 2,
}