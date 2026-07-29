using UnityEngine;

public class ShieldAudioController : MonoBehaviour
{
    [SerializeField] private ShieldManager shieldManager;
    [SerializeField] private AudioSource audioSource;

    [Header("Throw")]
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private float throwSoundStartTime = 0.1f;

    [Header("Recall")]
    [SerializeField] private AudioClip recallStartedSound;
    [SerializeField] private AudioClip recalledSound;

    private void Awake()
    {
        shieldManager.OnShieldThrown += HandleShieldThrown;
        shieldManager.OnShieldStartingRecall += HandleShieldStartingRecall;
        shieldManager.OnShieldRecalled += HandleShieldRecalled;
    }

    private void HandleShieldThrown()
    {
        PlayFromOffset(throwSound, throwSoundStartTime);
    }

    private void HandleShieldStartingRecall()
    {
        PlayOneShot(recallStartedSound);
    }

    private void HandleShieldRecalled()
    {
        PlayOneShot(recalledSound);
    }

    private void PlayFromOffset(AudioClip clip, float startTime)
    {
        if (clip == null) return;

        audioSource.clip = clip;
        audioSource.time = startTime;
        audioSource.Play();
    }

    private void PlayOneShot(AudioClip clip)
    {
        if (clip == null) return;

        audioSource.PlayOneShot(clip);
    }
}