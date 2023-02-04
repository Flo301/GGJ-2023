using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    AttackingEntity ae;
    Transform mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = transform.GetChild(0);
        ae = mesh.GetComponent<AttackingEntity>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerStay(Collider collision)
    {
        ae.Attack();
    }
}
