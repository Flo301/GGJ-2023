using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Emit Single Attack Sound
/// </summary>
public class EmitAttackSound : EmitWeaponSoundBase
{
    /// <summary>
    /// Starts Listening to the Attack Event
    /// </summary>
    private void OnEnable() => SoundEventManager.OnAttack += SoundAttack;

    /// <summary>
    /// Stops Listening to the Attack Event
    /// </summary>
    private void OnDisable() => SoundEventManager.OnAttack -= SoundAttack;

    /// <summary>
    /// Plays the Sound of the Attacking Weapon
    /// </summary>
    void SoundAttack() => Source.Play();
}