using UnityEngine;
using UnityEngine.UI;

public class PlayerHeartsUIManager : MonoBehaviour
{
    [SerializeField] private PlayerLifeManager playerLifeManager;

    [Header("Heart Images (in order, left to right)")]
    [SerializeField] private Image[] heartImages;

    [Header("Sprites")]
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite halfHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    private void Awake()
    {
        playerLifeManager.OnHealthChanged.AddListener(UpdateHearts);
    }

    private void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartValue = currentHealth - (i * 2);

            if (heartValue >= 2)
                heartImages[i].sprite = fullHeartSprite;
            else if (heartValue == 1)
                heartImages[i].sprite = halfHeartSprite;
            else
                heartImages[i].sprite = emptyHeartSprite;
        }
    }
}