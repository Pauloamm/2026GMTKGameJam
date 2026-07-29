using System;
using Unity.VisualScripting;


public interface IDamageable
{
    event Action OnDamaged;
    void TakeDamage(int damage);
}