using UnityEngine;
using System.Collections.Generic;

public class AttackingEntity : MonoBehaviour
{
    public AttackData selectedAttack;
    public float maxHP = 100;
    public float HP = 0;
    public float Resistance = 0;
    public HpBar HpBar;
    public HpBar StaminarBar;
    public List<WeaknessData> Weaknesses = new List<WeaknessData>();

    public LayerMask attackMask;
    public GameObject parent;
    public KeyCode debugAttackKey = KeyCode.None;

    private InfestionSpot infestationSpot;
    private float attackCooldown = 0f;

    // Update is called once per frame
    virtual protected void Update()
    {
        if (debugAttackKey != KeyCode.None && Input.GetKeyDown(debugAttackKey))
        {
            Attack();
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
            if (StaminarBar != null)
                StaminarBar.setHP(1 - (attackCooldown / selectedAttack.Cooldown));
        }
    }

    public void SetInfestationSpot(InfestionSpot _spot)
    {
        infestationSpot = _spot;
    }

    public void TakeDamage(AttackData attack)
    {
        float WeaknessFactor = getWeakness(attack.Typ).Factor;
        float ResistanceFactor = 1 - Mathf.Clamp(Resistance, 0, 1);
        float Damage = attack.Damage * WeaknessFactor * ResistanceFactor;
        // Debug.Log($"{attack.Damage} * {WeaknessFactor} * {ResistanceFactor} => {Damage}");
        // Debug.Log($"{transform.name} => DMG:{Damage} HP:{HP}", gameObject);

        TakeDamage(Damage);
    }

    public void TakeDamage(float value)
    {
        SetHp(HP - value);
    }

    public void SetHp(float hp)
    {
        HP = Mathf.Min(hp, maxHP);

        if (HpBar != null)
        {
            HpBar.setHP(HP / maxHP);
        }

        if (HP <= 0)
        {
            Die();
        }
    }

    public WeaknessData getWeakness(EAttackTyp type)
    {
        foreach (WeaknessData weakness in Weaknesses)
        {
            if (type == weakness.Type)
            {
                return weakness;
            }
        }
        WeaknessData newWeakness = new WeaknessData { Type = type, Factor = 1 };
        Weaknesses.Add(newWeakness);
        return newWeakness;
    }

    virtual protected void Die()
    {
        if (infestationSpot != null)
        {
            infestationSpot.OnEntityDie(this);
        }

        if (tag == "Player")
        {
            GameManager.Instance.OnPlayerDie();
        }
        else
        {
            GameManager.Instance.OnEnemyDie(this);
            Destroy(parent != null ? parent : gameObject);
        }
    }

    public bool Attack()
    {
        if (attackCooldown > 0) return false;
        attackCooldown = selectedAttack.Cooldown;

        //Debug.Log($"{transform.name} => DoAttack", gameObject);

        RaycastHit[] hits = null;
        switch (selectedAttack.Typ)
        {
            case EAttackTyp.Close:
                hits = Physics.BoxCastAll(transform.position, new Vector3(.1f, 2, selectedAttack.Range), transform.forward, Quaternion.identity, selectedAttack.Range, attackMask);
                break;
            case EAttackTyp.Radial:
                hits = Physics.SphereCastAll(transform.position, selectedAttack.Range, Vector3.one, .1f, attackMask);
                break;
            case EAttackTyp.Projectile:
                if (selectedAttack.ProjectileObj == null)
                {
                    Debug.LogError("Missing Projectile");
                    return false;
                }
                var pos = transform.position + transform.forward * 1.5f;
                pos.y = 1.5f;
                var projectile = GameObject.Instantiate(selectedAttack.ProjectileObj, pos, Quaternion.identity);
                projectile.Set(selectedAttack, this);
                projectile.transform.LookAt(projectile.transform.position + transform.forward);
                var rot = projectile.transform.rotation.eulerAngles;
                rot.x = 0;
                projectile.transform.rotation = Quaternion.Euler(rot);
                Destroy(projectile.gameObject, selectedAttack.Range);
                return true;
        }

        if (hits == null) return true;

        foreach (var hit in hits)
        {
            if (hit.collider.isTrigger) continue;
            var entity = hit.transform.GetComponent<AttackingEntity>();
            if (entity != null && entity != this)
            {
                entity.TakeDamage(selectedAttack);
            }
        }

        return true;
    }
}
