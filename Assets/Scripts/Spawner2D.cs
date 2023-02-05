using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner2D : MonoBehaviour
{
    public float range;
    public GameObject[] objs;

    public float cooldown;

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = 0.2f;
            Vector3 pos = transform.position + (Vector3.right * Random.Range(-1f, 1f) * range);

            var obj = objs[Random.Range(0, objs.Length - 1)];
            var _new = Instantiate(obj, pos, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            _new.transform.localScale = Vector2.one * Random.Range(.4f, 1.2f);
        }
    }
}
