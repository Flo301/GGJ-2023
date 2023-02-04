using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float growth = .0f;
    [SerializeField] float growthSpeed = .1f;
    [SerializeField] float max = 5f;
    AttackingEntity ae;
    Transform mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = transform.GetChild(0);
        mesh.localScale = new Vector3(growth, growth, growth);
        ae = mesh.GetComponent<AttackingEntity>();
    }

    // Update is called once per frame
    void Update()
    {
        growth = Mathf.Min(growth + Time.deltaTime * growthSpeed, max);
        mesh.localScale = new Vector3(growth, growth, growth);
    }

    void OnTriggerStay(Collider collision)
    {
        ae.Attack();
    }
}
