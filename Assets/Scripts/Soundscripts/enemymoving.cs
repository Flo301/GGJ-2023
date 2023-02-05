using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Enemy))]
public class enemymoving : MonoBehaviour
{
    private Enemy ThisEnemy;

    private AudioSource Audio;

    [SerializeField] private AudioClip Clip;

    // Start is called before the first frame update
    void Start()
    {
        ThisEnemy = GetComponent<Enemy>();
        Audio.GetComponent<AudioSource>();
        Audio.clip = Clip;
        Audio.playOnAwake = false;
        Audio.loop = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ThisEnemy.moving) Audio.Play();
        else Audio.Stop();
    }
}