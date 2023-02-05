using System.Collections;
using UnityEngine;

public class Enemy : AttackingEntity
{
    private Animator Animator;

    private float ElapsedTimeSincePlayerContact = 0;

    private float TimeToJump = 10;

    public bool moving = false;

    public float spread = 4;

    public float movementSpeed = 0.3f;

    public float diveSpeed = 0.1f;
    public float diveDistanceMin = 5f;
    public float diveDistanceMax = 15f;

    [Header("Sounds")]
    /// <summary>
    /// Source for the Attacking Sounds
    /// </summary>
    /// <returns></returns>
    private AudioSource Audio;

    /// <summary>
    /// Clip for Strong Attack
    /// </summary>
    [SerializeField] private AudioClip StrongAttackClip;

    // Add if Frantic Attack gets used
    // /// <summary>
    // /// Clip for Frantic Attack
    // /// </summary>
    // [SerializeField] private AudioClip FranticAttackCLip; 
    
    /// <summary>
    /// Seeks out all necessary Component at Awakening of this Instance.
    /// </summary>
    void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    void OnTriggerStay(Collider collision)
    {
        var playerPos = GameManager.Instance.Player.transform.position;
        transform.LookAt(new Vector3(playerPos.x, transform.position.y, playerPos.z));

        if (Attack())
        {
            Animator.SetTrigger("Strong_Attack");
        }
    }

    void Harden()
    {
        WeaknessData projectileWeakness = getWeakness(EAttackTyp.Close);
        projectileWeakness.Factor = 1f;
    }

    void BecomeFireResistant()
    {
        WeaknessData projectileWeakness = getWeakness(EAttackTyp.Projectile);
        projectileWeakness.Factor = 0.5f;
    }

    override protected void Update()
    {
        if (!moving)
        {
            ElapsedTimeSincePlayerContact += Time.deltaTime;
            if (ElapsedTimeSincePlayerContact > TimeToJump)
            {
                StartCoroutine(MoveToPlayer());
                ElapsedTimeSincePlayerContact = 0;
                TimeToJump = Random.Range(5, 25);
            }
        }

        base.Update();
    }

    IEnumerator MoveToPlayer()
    {
        moving = true;
        Vector3 playerPos = GameManager.Instance.Player.transform.position
            + new Vector3(Random.Range(-spread, spread), 0, Random.Range(-spread, spread));

        Vector3[] vectors = new Vector3[4];
        float[] timeFactors = new float[3];
        vectors[0] = transform.position;
        timeFactors[0] = diveSpeed;
        vectors[1] = new Vector3(vectors[0].x, -8, vectors[0].z);
        timeFactors[1] = movementSpeed;
        vectors[2] = vectors[1] + ((playerPos - vectors[1]).normalized * Random.Range(diveDistanceMin, diveDistanceMax));
        vectors[2].y = -8;
        timeFactors[2] = diveSpeed;
        vectors[3] = new Vector3(vectors[2].x, 0, vectors[2].z);

        for (int i = 0; i < vectors.Length - 1; i++)
        {
            float elapsedTime = 0;
            float totalTime = Vector3.Distance(vectors[i], vectors[i + 1]) * timeFactors[i];
            while (elapsedTime < totalTime)
            {
                transform.position = Vector3.Lerp(vectors[i], vectors[i + 1], (elapsedTime / totalTime));
                elapsedTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        moving = false;
        yield return null;
    }
}
