using UnityEngine;
using UnityEngine.InputSystem;

public class LevelIntroDialogManager : MonoBehaviour
{
    [SerializeField] private DialogBoxUI dialogBox;
    [SerializeField] private string introText;
    [SerializeField] private InputActionReference moveActionRef;
    [SerializeField] private float hideDelayAfterMoving = 1f;

    private bool hasStartedMoving;

    private void Awake()
    {
        dialogBox.Show(introText);
        moveActionRef.action.performed += OnPlayerMoved;
    }

    private void OnPlayerMoved(InputAction.CallbackContext context)
    {
        if (hasStartedMoving) return;

        hasStartedMoving = true;
        dialogBox.HideAfterSeconds(hideDelayAfterMoving);
        moveActionRef.action.performed -= OnPlayerMoved;
    }
}