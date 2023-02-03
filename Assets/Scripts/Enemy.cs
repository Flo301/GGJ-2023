using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    float growth = .0f;
    [SerializeField] float max = 5f;

    Transform mesh;

    // Start is called before the first frame update
    void Start()
    {
        mesh = transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        growth = Mathf.Min(growth + Time.deltaTime, max);
        transform.localScale = new Vector3(growth, growth, growth);
    }
}
