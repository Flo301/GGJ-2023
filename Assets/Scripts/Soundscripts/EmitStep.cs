using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Emit Single Attack Sound
/// </summary>
public class EmitStep : EmitWeaponSoundBase
{
    /// <summary>
    /// Starts Listening to the Attack Event
    /// </summary>
    private void OnEnable() => SoundEventManager.OnStep += SoundStep;

    /// <summary>
    /// Stops Listening to the Attack Event
    /// </summary>
    private void OnDisable() => SoundEventManager.OnStep -= SoundStep;

    /// <summary>
    /// Plays the Sound of the Attacking Weapon
    /// </summary>
    void SoundStep(float delay)
    {
        Debug.Log("yeah");
        Source.PlayDelayed(delay);
    }
}