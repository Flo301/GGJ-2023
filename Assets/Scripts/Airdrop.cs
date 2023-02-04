using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airdrop : MonoBehaviour
{
    public GameObject[] ContainingObjects;

    void OnCollisionEnter(Collision collision)
    {
        foreach (var obj in ContainingObjects)
        {
            Instantiate(obj, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
