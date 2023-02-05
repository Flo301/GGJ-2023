using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class Enemy : AttackingEntity
{
    /// <summary>
    /// Animator for the Attacking Animations
    /// </summary>
    private Animator Animator;

    /// <summary>
    /// Source for the Attacking Sounds
    /// </summary>
    /// <returns></returns>
    private AudioSource Audio;

    [Header("Sounds")]
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
        Audio = GetComponent<AudioSource>();
        Audio.playOnAwake = false;
    }

    void OnTriggerStay(Collider collision)
    {
        var playerPos = GameManager.Instance.Player.transform.position;
        transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));

        if (Attack())
        {
            Animator.SetTrigger("Strong_Attack");
            Audio.clip = StrongAttackClip;
            Audio.Play();
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
