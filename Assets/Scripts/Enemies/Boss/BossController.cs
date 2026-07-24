using System.Collections;
using UnityEngine;

public class BossController : EnemyBase
{
    private enum AttackType { MeleeSwing, ArcVolley, GroundWaves, FallingProjectiles }

    [Header("Boss Settings")]
    [SerializeField] private Transform player;
    private bool hasEngaged;

    [Header("Attack Cooldown")]
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float phaseTwoAttackCooldown = 1.5f;
    [SerializeField] private float phaseTwoHealthThreshold = 0.5f;
    private float currentCooldown;
    private bool hasEnteredPhaseTwo;

    [Header("Melee Attack")]
    [SerializeField] private MeleeAttackBehaviour meleeAttack;

    [Header("Arc Volley")]
    [SerializeField] private ArcLaunchProjectileBehaviour projectileLauncher;
    [SerializeField] private int volleyShotCount = 6;
    [SerializeField] private float volleyShotDelay = 0.2f;
    [SerializeField] private float volleyWindup = 0.5f;

    [Header("Ground Waves")]
    [SerializeField] private GameObject groundWavePrefab;
    [SerializeField] private Transform groundWaveSpawnPoint;
    [SerializeField] private float groundWaveSpeed = 5f;
    [SerializeField] private float waveWindup = 0.6f;

    [Header("Falling Projectiles")]
    [SerializeField] private GameObject fallingProjectilePrefab;
    [SerializeField] private float fallingProjectileSpawnHeight = 8f;
    [SerializeField] private float fallingProjectileSpawnRangeX = 6f;
    [SerializeField] private float fallingProjectileExclusionRadius = 1.5f;
    [SerializeField] private int fallingProjectileCount = 6;
    [SerializeField] private float fallingProjectileSpawnInterval = 0.3f;
    [SerializeField] private float screamWindup = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        currentCooldown = attackCooldown;

        //Player DetectionZone events
        PlayerDetectionZone enemyDetectionZone;
        enemyDetectionZone = GetComponentInChildren<PlayerDetectionZone>();
        enemyDetectionZone.OnPlayerEnter.AddListener(HandlePlayerEnterRange);
    }

    public void HandlePlayerEnterRange(Transform playerTransform)
    {
        if (hasEngaged) return;

        hasEngaged = true;
        player = playerTransform;
        StartCoroutine(BossAttackLoop());
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (!hasEnteredPhaseTwo && currentHealth <= maxHealth * phaseTwoHealthThreshold)
        {
            hasEnteredPhaseTwo = true;
            currentCooldown = phaseTwoAttackCooldown;
        }
    }

    private IEnumerator BossAttackLoop()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(currentCooldown);

            if (currentHealth <= 0) yield break;

            AttackType attack = (AttackType)Random.Range(0, 4);  
            Debug.Log("PROXIMO ATAQUE DO BOSS É" + attack);
            yield return StartCoroutine(ExecuteAttack(attack));
        }
    }

    private IEnumerator ExecuteAttack(AttackType attack)
    {
        switch (attack)
        {
            case AttackType.MeleeSwing:
                meleeAttack.TryAttack();
                yield return new WaitUntil(() => !meleeAttack.IsAttacking);
                break;

            case AttackType.ArcVolley:
                yield return StartCoroutine(ArcVolleyRoutine());
                break;

            case AttackType.GroundWaves:
                yield return StartCoroutine(GroundWavesRoutine());
                break;

            case AttackType.FallingProjectiles:
                yield return StartCoroutine(FallingProjectilesRoutine());
                break;
        }
    }

    private IEnumerator ArcVolleyRoutine()
    {
        yield return new WaitForSeconds(volleyWindup);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        for (int i = 0; i < volleyShotCount; i++)
        {
            Vector2 fakeTarget = (Vector2)transform.position + new Vector2(direction * Random.Range(1.5f, 7f), Random.Range(0.5f, 2f));
            projectileLauncher.Fire(fakeTarget);
            yield return new WaitForSeconds(volleyShotDelay);
        }
    }

    private IEnumerator GroundWavesRoutine()
    {
        yield return new WaitForSeconds(waveWindup);

        SpawnGroundWave(-1f);
        SpawnGroundWave(1f);
    }

    private void SpawnGroundWave(float direction)
    {
        GameObject wave = Instantiate(groundWavePrefab, groundWaveSpawnPoint.position, Quaternion.identity);
        Rigidbody2D rb = wave.GetComponent<Rigidbody2D>();
        rb.linearVelocity = new Vector2(groundWaveSpeed * direction, 0f);

        Vector3 scale = wave.transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        wave.transform.localScale = scale;

        Collider2D[] collidersToIgnore = GetComponentsInChildren<Collider2D>();


        GroundMovingHazard hazard = wave.GetComponent<GroundMovingHazard>();
        hazard.IgnoreColliders(collidersToIgnore);

        AttackHitbox hitbox = wave.GetComponent<AttackHitbox>();
        hitbox.IgnoreColliders(collidersToIgnore);

    }

    private IEnumerator FallingProjectilesRoutine()
    {
        yield return new WaitForSeconds(screamWindup);

        for (int i = 0; i < fallingProjectileCount; i++)
        {
            float randomX = GetRandomFallingProjectileSpawnX();
            Vector2 spawnPosition = new Vector2(randomX, transform.position.y + fallingProjectileSpawnHeight);

            SpawnFallingProjectile(spawnPosition);

            yield return new WaitForSeconds(fallingProjectileSpawnInterval);
        }
    }

    private float GetRandomFallingProjectileSpawnX()
    {
        float offset = Random.Range(fallingProjectileExclusionRadius, fallingProjectileSpawnRangeX);
        float side = Random.value < 0.5f ? -1f : 1f;

        return transform.position.x + (offset * side);
    }

    private void SpawnFallingProjectile(Vector2 spawnPosition)
    {
        GameObject fallingProjectile = Instantiate(fallingProjectilePrefab, spawnPosition, Quaternion.identity);

        Collider2D[] collidersToIgnore = GetComponentsInChildren<Collider2D>();

        if (fallingProjectile.TryGetComponent<Projectile>(out Projectile projectileScript))
        {
            projectileScript.IgnoreColliders(collidersToIgnore);
        }

        if (fallingProjectile.TryGetComponent<AttackHitbox>(out AttackHitbox hitbox))
        {
            hitbox.IgnoreColliders(collidersToIgnore);
        }
    }

    private void Update()
    {
        if (!hasEngaged || player == null) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        transform.localScale = scale;
    }
}