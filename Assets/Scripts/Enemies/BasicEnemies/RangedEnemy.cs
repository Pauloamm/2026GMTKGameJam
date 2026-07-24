using UnityEngine;

public class RangedEnemy : EnemyBase
{
    [Header("Firing")]
    [SerializeField] private float firingRange = 5f;
    [SerializeField] private float fireCooldown = 2f;
    private float cooldownTimer;

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
        Debug.Log("PLAYER ENTROU PDOE DISPARAR");

        player = playerTransform;
    }

    public void HandlePlayerExitRange()
    {
        Debug.Log("PLAYER ENTROU PDOE DISPARAR");

        player = null;
    }

    private void Update()
    {

        cooldownTimer -= Time.deltaTime;

        if (player == null) return;



        if (cooldownTimer <= 0f)
        {
            Debug.Log("DISPAROU");
            launcher.Fire(player.position);
            cooldownTimer = fireCooldown;
        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //Gizmos.DrawLine(transform.position,new Vector3(transform.position.x + firingRange, transform.position.y, transform.position.z));
    }

}