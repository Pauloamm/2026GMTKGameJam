using UnityEngine;

[RequireComponent(typeof(MeleeAttackBehaviour))]
public class MeleeEnemy : EnemyBase
{
    private enum MovementState { Wander, Chase }

    //Behaviour Componenets


    [Header("Chase Settings")]
    [SerializeField] private EnemyWanderingBehaviour wanderingBehaviour;
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float attackRange = 1.2f;

    [Header("Wall Check")]
    [SerializeField] private Vector2 wallCheckOffset;
    [SerializeField] private float wallCheckDistance = 0.2f;
    [SerializeField] private LayerMask wallLayer;

    private Rigidbody2D rb;
    private MeleeAttackBehaviour attackBehaviour;
    private MovementState currentState = MovementState.Wander;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();

        //Player DetectionZone events
        PlayerDetectionZone enemyDetectionZone;
        enemyDetectionZone = GetComponentInChildren<PlayerDetectionZone>();
        enemyDetectionZone.OnPlayerEnter.AddListener(HandlePlayerEnterRange);
        enemyDetectionZone.OnPlayerExit.AddListener(HandlePlayerExitRange);

        rb = GetComponent<Rigidbody2D>();
        attackBehaviour = GetComponent<MeleeAttackBehaviour>();
    }

    public void HandlePlayerEnterRange(Transform playerTransform)
    {
        player = playerTransform;
        currentState = MovementState.Chase;
        wanderingBehaviour.enabled = false;
    }

    public void HandlePlayerExitRange()
    {
        player = null;
        currentState = MovementState.Wander;
        wanderingBehaviour.enabled = true;
    }

    private void Update()
    {

        if (currentState != MovementState.Chase || attackBehaviour.IsAttacking) return;

        //Check attack range
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            attackBehaviour.TryAttack();
        }
    }

    private void FixedUpdate()
    {
        if (currentState != MovementState.Chase) return;

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        if (IsWallAhead(direction))
        {
            HandlePlayerExitRange();
            return;
        }

        rb.linearVelocityX = chaseSpeed * direction;
        FaceDirection(direction);
    }

    private bool IsWallAhead(float direction)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(wallCheckOffset.x * direction, wallCheckOffset.y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, wallCheckDistance, wallLayer);
        return hit.collider != null;
    }

}