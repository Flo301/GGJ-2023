using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] GameObject player;

    [SerializeField] float interval = 10f;

    [SerializeField] float minDistance = 10f;

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
                pos = new Vector3(Random.Range(-500, 500), Random.Range(-500, 500));
            } while (Vector3.Distance(player.transform.position, pos) < minDistance);
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
