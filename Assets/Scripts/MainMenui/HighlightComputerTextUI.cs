using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightComputerTextUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Text buttonText;
    private string baseText;

    [Header("Click Blink")]
    [SerializeField] private float blinkInterval = 0.1f;
    [SerializeField] private int blinkCount = 4;

    [Header("Sound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSound;
    [SerializeField] private AudioClip clickSound;

    private bool isBlinking;

    private void Awake()
    {
        baseText = buttonText.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isBlinking) return;
        buttonText.text = $"> {baseText} <";
        PlaySound(hoverSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isBlinking) return;
        buttonText.text = baseText;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBlinking) return;
        PlaySound(clickSound);
        StartCoroutine(BlinkRoutine());
    }

    private IEnumerator BlinkRoutine()
    {
        isBlinking = true;

        for (int i = 0; i < blinkCount; i++)
        {
            buttonText.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            buttonText.enabled = true;
            yield return new WaitForSeconds(blinkInterval);
        }

        isBlinking = false;
        buttonText.text = baseText;
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip == null) return;
        audioSource.PlayOneShot(clip);
    }

    private void OnDisable()
    {
        isBlinking = false;
        buttonText.text = baseText;
    }
}