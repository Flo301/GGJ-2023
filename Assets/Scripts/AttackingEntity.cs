using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEntity : MonoBehaviour
{
    public AttackData attackData;
    public float maxHP = 100;
    public float HP = 100;
    public HpBar HpBar;
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
        Debug.Log($"{transform.name} => DMG:{attack.Damage} HP:{HP}", gameObject);
        
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
            Destroy(gameObject);
        }
    }

    public void Attack()
    {
        if (attackCooldown > 0) return;

        Debug.Log($"{transform.name} => DoAttack", gameObject);
        attackCooldown = attackData.Cooldown;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, new Vector3(.1f, 2, attackData.Range), transform.forward, Quaternion.identity, attackData.Range);
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
