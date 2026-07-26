using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Firing")]
    [SerializeField] private float firingRange = 5f;
    [SerializeField] private float fireCooldown = 2f;
    private float cooldownTimer;

    [Header("Animation")]
    [SerializeField] private Animator enemyAnimator;

    private IProjectileLauncher launcher;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        launcher = GetComponent<IProjectileLauncher>();

        //Player DetectionZone events
        PlayerDetectionZone enemyDetectionZone;
        enemyDetectionZone = GetComponentInChildren<PlayerDetectionZone>();
        enemyDetectionZone.OnPlayerEnter.AddListener(HandlePlayerEnterRange);
        enemyDetectionZone.OnPlayerExit.AddListener(HandlePlayerExitRange);
    }

    public void HandlePlayerEnterRange(Transform playerTransform)
    {
        player = playerTransform;
    }

    public void HandlePlayerExitRange()
    {
        player = null;
    }

    private void Update()
    {

        cooldownTimer -= Time.deltaTime;

        if (player == null) return;

        if (cooldownTimer <= 0f)
        {
            enemyAnimator.SetTrigger("AttackStarted");
            launcher.Fire(player.position);
            cooldownTimer = fireCooldown;
        }
    }

}