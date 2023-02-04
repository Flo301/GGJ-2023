using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AttackingEntity
{
    private Animator Animator;

    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerStay(Collider collision)
    {
        var playerPos = GameManager.Instance.Player.transform.position;
        transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));

        if (Attack())
        {
            Animator.SetTrigger("Strong_Attack");
        }
    }

    void Harden()
    {
        WeaknessData projectileWeakness = getWeakness(EAttackTyp.Close);
        projectileWeakness.Factor = 1f;
    }

    void BecomeFireResistant()
    {
        WeaknessData projectileWeakness = getWeakness(EAttackTyp.Projectile);
        projectileWeakness.Factor = 0.5f;
    }
}
