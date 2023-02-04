using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfestionSpot : MonoBehaviour
{
    public float distance;

    public GameObject EntityObject;
    public int minEntityAmount;
    public int maxEntityAmount;


    public GameObject RewardObject;
    public int minRewardAmount;
    public int maxRewardAmount;

    public GameObject Player;

    public float triggerDistance;

    private List<AttackingEntity> SpawnedEntities = new List<AttackingEntity>();

    public float forceSpawnTime;

    private float deltaTime = 0;

    Transform playerTransform;

    void Start()
    {
        playerTransform = Player.GetComponent<Transform>();
    }

    void Update()
    {
        if (SpawnedEntities.Count > 0)
        {
            return;
        }
        deltaTime += Time.deltaTime;
        float playerDistance = Vector3.Distance(transform.position, playerTransform.position);
        if (deltaTime > forceSpawnTime)
        {
            //Debug.Log($"{gameObject.name}: {deltaTime} > {forceSpawnTime}");
            StartInfestation();
        }
        else if (playerDistance < triggerDistance)
        {
            //Debug.Log($"{gameObject.name}: {distance} < {triggerDistance}");
            StartInfestation();
        }
    }

    void StartInfestation()
    {
        Debug.Log($"{gameObject.name}: Start Infestion");

        var spawnAmount = Random.Range(minEntityAmount, maxEntityAmount);

        for (int i = 0; i < spawnAmount; i++)
        {
            var origin = transform.position + Vector3.right * Random.Range(-distance, distance) + Vector3.forward * Random.Range(-distance, distance) + Vector3.up * 100;
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit))
            {
                Vector3 spawnPosition = new Vector3(hit.point.x, 0, hit.point.z);
                var enemy = Instantiate(EntityObject, spawnPosition, Quaternion.identity);
                var entity = enemy.GetComponentInChildren<AttackingEntity>();
                entity.SetInfestationSpot(this);
                SpawnedEntities.Add(entity);
            }
        }
    }

    public void OnEntityDie(AttackingEntity _entity)
    {
        SpawnedEntities.Remove(_entity);
        if (SpawnedEntities.Count <= 0)
        {
            OnDefeateSpot();
        }
    }

    private void OnDefeateSpot()
    {
        var spawnAmount = Random.Range(minRewardAmount, maxRewardAmount);

        for (int i = 0; i < spawnAmount; i++)
        {
            var spawnPoint = transform.position + Vector3.right * Random.Range(-distance, distance) + Vector3.forward * Random.Range(-distance, distance) + Vector3.up * 20;
            var enemy = Instantiate(RewardObject, spawnPoint, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
