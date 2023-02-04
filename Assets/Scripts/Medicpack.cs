using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medicpack : MonoBehaviour
{
    public float hp = 100f;

    void OnTriggerEnter(Collider collision)
    {
        var entity = collision.GetComponent<AttackingEntity>();
        if (entity != null && entity.tag == "Player")
        {
            entity.TakeDamage(-hp);
            Destroy(gameObject);
        }
    }
}
