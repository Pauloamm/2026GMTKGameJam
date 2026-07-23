using UnityEngine;
using UnityEngine.Events;

public class ThrownShieldRicochetBehaviour : MonoBehaviour
{
    [SerializeField] Collider2D shieldColliderForRicochet;
    public UnityEvent OnShieldRicochet;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("ESCUDO COLIDIU");
        //verifica se é inimigo, se pode levar dano, se sim dá
        IDamageable damageableEntity;
        if (collision.gameObject.TryGetComponent<IDamageable>(out damageableEntity))
            damageableEntity.TakeDamage(1); // dá valor 1 por agora, MUDART MAIS TARDE

        OnShieldRicochet.Invoke();


    }

}
