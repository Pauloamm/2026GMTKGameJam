using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogBoxUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text dialogText;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine activeRoutine;

    private void Awake()
    {
        //canvasGroup.alpha = 0f;
    }

    public void Show(string text)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        dialogText.text = text;
        canvasGroup.alpha = 1f;
    }

    public void HideAfterSeconds(float delay)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(HideRoutine(delay));
    }

    private IEnumerator HideRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);

        float startAlpha = canvasGroup.alpha;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, t / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}