using UnityEngine;

public class AttackingEntity : MonoBehaviour
{
    public AttackData attackData;
    public float maxHP = 100;
    public float HP = 0;
    public float Resistance = 0;
    public HpBar HpBar;
    public WeaknessData[] Weaknesses;

    public LayerMask attackMask;
    public GameObject parent;
    public KeyCode debugAttackKey = KeyCode.None;

    private float attackCooldown = 0;

    // Update is called once per frame
    void Update()
    {
        if (debugAttackKey != KeyCode.None && Input.GetKeyDown(debugAttackKey))
        {
            Attack();
        }

        if (attackCooldown > 0)
        {
            attackCooldown -= Time.deltaTime;
        }
    }

    public void TakeDamage(AttackData attack)
    {
        float res = 1 - Mathf.Clamp(Resistance, 0, 1);
        float Factor = 1;
        foreach (WeaknessData weakness in Weaknesses)
        {
            if (attack.Typ == weakness.Type) {
                Factor = weakness.Factor;
                break;
            }
        }
        float Damage = attack.Damage * Factor * res;
        HP -= Damage;
        if (HpBar != null)
            HpBar.setHP(HP / maxHP);

        if (HP <= 0)
        {
            Die();
        }
    }

    virtual protected void Die()
    {
        if (tag == "Player")
        {
            GameManager.Instance.OnPlayerDie();
        }
        else
        {
            GameManager.Instance.OnEnemyDie();
            Destroy(parent != null ? parent : gameObject);
        }
    }

    public void Attack()
    {
        if (attackCooldown > 0) return;
        attackCooldown = attackData.Cooldown;
        // Debug.Log($"{transform.name} => DoAttack", gameObject);

        RaycastHit[] hits = null;
        switch (attackData.Typ)
        {
            case EAttackTyp.Close:
                hits = Physics.BoxCastAll(transform.position, new Vector3(.1f, 2, attackData.Range), transform.forward, Quaternion.identity, attackData.Range, attackMask);
                break;
            case EAttackTyp.Radial:
                hits = Physics.SphereCastAll(transform.position, attackData.Range, Vector3.one, .1f, attackMask);
                break;
            case EAttackTyp.Projectile:
                if (attackData.ProjectileObj == null)
                {
                    Debug.LogError("Missing Projectile");
                    return;
                }
                var projectile = GameObject.Instantiate(attackData.ProjectileObj, transform.position + transform.forward * 1.5f, Quaternion.identity);
                projectile.Set(attackData, this);
                projectile.transform.LookAt(projectile.transform.position + transform.forward);
                Destroy(projectile.gameObject, attackData.Range);
                return;
        }

        if (hits == null) return;

        foreach (var hit in hits)
        {
            var entity = hit.transform.GetComponent<AttackingEntity>();
            if (entity != null && entity != this)
            {
                entity.TakeDamage(attackData);
            }
        }
    }
}
