using UnityEngine;

public class AttackingEntity : MonoBehaviour
{
    public AttackData selectedAttack;
    public float maxHP = 100;
    public float HP = 100;
    public HpBar HpBar;

    public LayerMask attackMask;
    public GameObject parent;
    public KeyCode debugAttackKey = KeyCode.None;

    private float attackCooldown = 0f;

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
        // Debug.Log($"{transform.name} => DMG:{attack.Damage} HP:{HP}", gameObject);
        HP -= attack.Damage;
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
        attackCooldown = selectedAttack.Cooldown;
        // Debug.Log($"{transform.name} => DoAttack", gameObject);

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
                entity.TakeDamage(selectedAttack);
            }
        }
    }
}
