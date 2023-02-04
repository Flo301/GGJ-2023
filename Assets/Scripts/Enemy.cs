using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    AttackingEntity ae;

    // Start is called before the first frame update
    void Start()
    {
        ae = GetComponent<AttackingEntity>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider collision)
    {
        ae.Attack();
    }

    void Harden()
    {
        WeaknessData projectileWeakness = ae.getWeakness(EAttackTyp.Projectile);
        projectileWeakness.Factor = 0.5f;
    }
}
