using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ConstantDownwardVelocityLauncher : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 8f;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.down * fallSpeed;
    }
}