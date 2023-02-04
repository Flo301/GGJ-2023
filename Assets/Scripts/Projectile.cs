using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Velocity;
    private AttackData attackData;
    private AttackingEntity Emitter;
    private Rigidbody Rigidbody;

    public void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void Set(AttackData _data, AttackingEntity _emitter)
    {
        attackData = _data;
        Emitter = _emitter;
    }

    void Update()
    {
        Rigidbody.velocity = transform.forward * Velocity;
    }

    void OnCollisionEnter(Collision collision)
    {
        var entity = collision.gameObject.GetComponent<AttackingEntity>();

        if (entity != null && (Emitter.attackMask.value & 1 << entity.gameObject.layer) > 0) //Check layer is in layermask
        {
            entity.TakeDamage(attackData);
        }

        Destroy(gameObject);
    }
}
