using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Emit Rapid Fire Sound
/// </summary>
/// [RequireComponent(typeof(AudioSource))]
public class EmitRapidFire : EmitWeaponSoundBase
{
    /// <summary>
    /// Starts Listening to the Attack Event
    /// </summary>
    private void OnEnable() => SoundEventManager.OnRapidFire += SoundAttack;

    /// <summary>
    /// Stops Listening to the Attack Event
    /// </summary>
    private void OnDisable() => SoundEventManager.OnRapidFire -= SoundAttack;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Source.loop = true;
    }
    
    /// <summary>
    /// Plays the Sound of the Attacking Weapon
    /// </summary>
    void SoundAttack(bool value)
    {
        if(value) Source.Play();
        else Source.Stop();
    }

 
}
