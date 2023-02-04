using UnityEngine;

[System.Serializable]
public struct AttackData
{
    public EAttackTyp Typ;
    public float Damage;
    public float Range;
    public float Cooldown;
    public Projectile ProjectileObj;
}

public enum EAttackTyp
{
    Close = 1,
    Radial = 2,
    Projectile = 3
}