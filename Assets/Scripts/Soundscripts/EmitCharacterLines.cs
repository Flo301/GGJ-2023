using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class CharacterLines : MonoBehaviour
{
    /// <summary>
    /// Source of the Dude
    /// </summary>
    [SerializeField] private AudioSource Source;
    
    /// <summary>
    /// Clip of the Sound of the Dude Flipping
    /// </summary>
    [SerializeField] private AudioClip FlippingClip;
    
    // Start is called before the first frame update
    void Start()
    {
        Source = GetComponent<AudioSource>();
        Source.playOnAwake = false;
    }

    /// <summary>
    /// Starts Listening to the Line Events
    /// </summary>
    private void OnEnable()
    {
        SoundEventManager.OnFlipping += FlippingTheBird;
    }

    /// <summary>
    /// Stops Listening to the Line Events
    /// </summary>
    private void OnDisable()
    {
        SoundEventManager.OnFlipping -= FlippingTheBird;
    }
    
    /// <summary>
    /// Makes the Dude say their Line when Flipping the Bird.
    /// </summary>
    void FlippingTheBird()
    {
        Debug.Log("yeahr");
        Source.clip = FlippingClip;
        Source.Play();
    }
}
