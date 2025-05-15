using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IDamageable
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    void TakeDamage(float amount);
    void Die();
}
