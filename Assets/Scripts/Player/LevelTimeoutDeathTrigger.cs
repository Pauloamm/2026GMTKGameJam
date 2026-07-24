using UnityEngine;

public class LevelTimeoutDeathTrigger : MonoBehaviour
{
    [SerializeField] private LevelTimerManager levelTimerManager;
    [SerializeField] private PlayerLifeManager playerLifeManager;

    private void Awake()
    {
        levelTimerManager.OnTimeExpired.AddListener(playerLifeManager.ForceDeath);
    }
}