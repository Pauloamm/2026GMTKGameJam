using UnityEngine;
using UnityEngine.Events;

public class LevelTimerManager : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private float baseTime = 120f;
    [SerializeField] private float timeBonusPerKill = 20f;

    private float remainingTime;
    private bool hasTimeExpired;

    public UnityEvent<float> OnTimerTick;
    public UnityEvent OnTimeExpired;

    private void Awake()
    {
        remainingTime = baseTime;

        EnemyBase[] enemies = FindObjectsByType<EnemyBase>();

        foreach (EnemyBase enemy in enemies)
        {
            enemy.OnDeath += HandleEnemyDeath;
        }
    }

    private void Update()
    {
        if (hasTimeExpired) return;

        remainingTime -= Time.deltaTime;
        OnTimerTick?.Invoke(remainingTime);

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            hasTimeExpired = true;
            OnTimeExpired?.Invoke();
        }
    }

    private void HandleEnemyDeath(EnemyBase enemy)
    {
        remainingTime += timeBonusPerKill;
    }
}