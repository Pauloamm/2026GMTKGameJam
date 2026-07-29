using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [Header("Audio Sources (2, for crossfading)")]
    [SerializeField] private AudioSource sourceA;
    [SerializeField] private AudioSource sourceB;
    private AudioSource activeSource;
    private AudioSource inactiveSource;

    [Header("Clips")]
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip bossMusic;
    [SerializeField] private AudioClip deathStinger;
    [SerializeField] private AudioClip victoryStinger;

    [Header("Fade Settings")]
    [SerializeField] private float normalToBossFadeDuration = 1.5f;
    [SerializeField] private float deathWinFadeDuration = 0.4f;

    [Header("References")]
    [SerializeField] private BossController boss;
    [SerializeField] private PlayerLifeManager playerLifeManager;

    private Coroutine fadeRoutine;

    [Header("Volume")]
    [SerializeField] private float maxVolume = 0.2f;

    private void Awake()
    {
        activeSource = sourceA;
        inactiveSource = sourceB;

        activeSource.clip = normalMusic;
        activeSource.loop = true;
        activeSource.volume = maxVolume;
        activeSource.Play();

        inactiveSource.loop = true;
        inactiveSource.volume = 0f;

        if (boss != null)
        {
            boss.OnBossEngaged+=StartBossMusic;
            boss.OnDeath += HandleBossDeath;
        }

        if (playerLifeManager != null)
        {
            playerLifeManager.OnDeath += HandlePlayerDeath;
        }
    }

    private void StartBossMusic()
    {
        CrossfadeTo(bossMusic, normalToBossFadeDuration);
    }

    private void HandleBossDeath(EnemyBase enemy)
    {
        CrossfadeTo(victoryStinger, deathWinFadeDuration);
    }

    private void HandlePlayerDeath()
    {
        CrossfadeTo(deathStinger, deathWinFadeDuration);
    }


    private void CrossfadeTo(AudioClip newClip, float duration)
    {
        if (newClip == null) return;

        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(CrossfadeRoutine(newClip, duration));
    }

    private IEnumerator CrossfadeRoutine(AudioClip newClip, float duration)
    {
        inactiveSource.clip = newClip;
        inactiveSource.volume = 0f;
        inactiveSource.Play();

        float startVolume = activeSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            inactiveSource.volume = Mathf.Lerp(0f, maxVolume, t);
            activeSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        inactiveSource.volume = maxVolume;
        activeSource.volume = 0f;
        activeSource.Stop();

        (activeSource, inactiveSource) = (inactiveSource, activeSource);
    }
}