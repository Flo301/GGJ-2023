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

    private List<AttackingEntity> SpawnedEntities = new List<AttackingEntity>();

    void Start()
    {
        var spawnAmount = Random.Range(minEntityAmount, maxEntityAmount);

        for (int i = 0; i < spawnAmount; i++)
        {
            var origin = transform.position + Vector3.right * Random.Range(-distance, distance) + Vector3.forward * Random.Range(-distance, distance) + Vector3.up * 100;
            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit))
            {
                var enemy = Instantiate(EntityObject, hit.point, Quaternion.identity);
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
