using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AttackingEntity
{
    private Animator Animator;

    private float ElapsedTimeSincePlayerContact = 0;

    private float TimeToJump = 10;

    private bool moving = false;

    public float spread = 4;

    public float movementSpeed = 0.3f;

    public float diveSpeed = 0.1f;

    [Header("Sounds")]
    /// <summary>
    /// Source for the Attacking Sounds
    /// </summary>
    /// <returns></returns>
    private AudioSource Audio;

    /// <summary>
    /// Clip for Strong Attack
    /// </summary>
    [SerializeField] private AudioClip StrongAttackClip;

    // Add if Frantic Attack gets used
    // /// <summary>
    // /// Clip for Frantic Attack
    // /// </summary>
    // [SerializeField] private AudioClip FranticAttackCLip; 
    
    /// <summary>
    /// Seeks out all necessary Component at Awakening of this Instance.
    /// </summary>
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
