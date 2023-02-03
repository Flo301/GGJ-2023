[System.Serializable]
public struct AttackData
{
    public EAttackTyp Typ;
    public float Damage;
    public float Range;
    public float Cooldown;
}

public enum EAttackTyp
{
    Close = 1,
}