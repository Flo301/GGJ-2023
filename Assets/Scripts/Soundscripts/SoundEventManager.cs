using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SoundEventManager
{
    /// <summary>
    /// Attacking Sound Event.
    /// </summary>
    public delegate void AttackSound();

    /// <summary>
    /// Attacking Sound Event.
    /// </summary>
    public static event AttackSound OnAttack;

    /// <summary>
    /// Flipping Sound Event.
    /// </summary>
    public delegate void FlippingSound();

    /// <summary>
    /// Attacking Sound Event.
    /// </summary>
    public static event FlippingSound OnFlipping;
    
    /// <summary>
    /// Flipping Sound Event.
    /// </summary>
    public delegate void SteppingSound(float delay);

    /// <summary>
    /// Attacking Sound Event.
    /// </summary>
    public static event SteppingSound OnStep;
    
    /// <summary>
    /// Flipping Sound Event.
    /// </summary>
    public delegate void RapidFireSound(bool status);

    /// <summary>
    /// Attacking Sound Event.
    /// </summary>
    public static event RapidFireSound OnRapidFire;

    /// <summary>
    /// Emit Attack Sound
    /// </summary>
    public static void EmitAttack() => OnAttack();

    /// <summary>
    /// Emit Flipping Sound
    /// </summary>
    public static void EmitFlipping() => OnFlipping();
    
    /// <summary>
    /// Emit Stepping Sound
    /// </summary>
    public static void EmitStepping(float delay) => OnStep(delay);
    
    /// <summary>
    /// Emit RepidFire Sound
    /// </summary>
    public static void EmitRapidFire(bool status) => OnRapidFire(status);
    
}