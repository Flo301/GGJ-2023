using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Incrases Beat when HP is Low
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class beatpitch : MonoBehaviour
{
    [SerializeField] private AudioSource Audio;
    // Start is called before the first frame update
    void Start()
    {
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Player.AttackingEntity.HP <= 0)
        {
            Audio.pitch = .2f;
        }
        else
        {
            var Multiplier = GameManager.Instance.Player.AttackingEntity.maxHP -
                             GameManager.Instance.Player.AttackingEntity.HP;
            Multiplier /= GameManager.Instance.Player.AttackingEntity.maxHP;

            Audio.pitch = 1 + Multiplier;
        }
    }
}
