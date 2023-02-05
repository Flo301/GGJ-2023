using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class for Weapon Sounds Emiter
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EmitWeaponSoundBase : MonoBehaviour
{
    /// <summary>
    /// Clip of the Sound of the Weapon
    /// </summary>
    [SerializeField] protected AudioClip Clip;

    /// <summary>
    /// Source of the Weapon Audio
    /// </summary>
    [SerializeField] protected AudioSource Source;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.playOnAwake = false;
        Source.clip = Clip;
    }
}
