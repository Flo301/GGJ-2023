using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject player;
    [SerializeField] float interval = 3f;
    [SerializeField] float minDistance = 3f;
    [SerializeField] float range = 5f;

    float delta = .0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta > interval) {
            delta = .0f;
            Vector3 pos;
            do {
                pos = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            } while (Vector3.Distance(player.transform.position, pos) < minDistance);
            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity);
        }
    }
}
