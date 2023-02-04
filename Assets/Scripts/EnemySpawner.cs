using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject player;
    [SerializeField] float interval = 3f;
    [SerializeField] float minDistanceFromPlayer = 7f;
    [SerializeField] float range = 50f;

    float delta = .0f;

    // Update is called once per frame
    void Update()
    {
        delta += Time.deltaTime;
        if (delta > interval) {
            delta = .0f;
            Vector3 position;
            do {
                position = new Vector3(Random.Range(-range, range), 0, Random.Range(-range, range));
            } while (!isValidPosition(position));
            GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
        }
    }

    bool isValidPosition(Vector3 position)
    {
        return Vector3.Distance(player.transform.position, position) > minDistanceFromPlayer;
    }
}
