using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingEntity : MonoBehaviour
{
    public AttackData attackData;
    public float HP = 100;
    public KeyCode attackKey = KeyCode.None;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (attackKey != KeyCode.None && Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }

    public void TakeDamage(AttackData attack)
    {
        HP -= attack.Damage;
        Debug.Log($"{transform.name} => DMG:{attack.Damage} HP:{HP}", gameObject);

        if (HP <= 0)
        {
            Die();
        }
    }

    virtual protected void Die()
    {
        Destroy(gameObject);
    }

    protected void Attack()
    {
        Debug.Log($"{transform.name} => DoAttack", gameObject);
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.one * attackData.Range / 2f, transform.forward);
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
