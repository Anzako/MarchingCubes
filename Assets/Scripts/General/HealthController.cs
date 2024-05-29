using System;
using UnityEngine;

public class HealthController : MonoBehaviour, IDamagable
{
    public event Action Damaged = delegate { };
    public event Action Killed = delegate { };

    // Health
    [SerializeField] private int maxHealth;
    [field: SerializeField] public int Health { get; private set; }

    private void Awake()
    {
        SetHealthToMax();
    }

    public void SetHealth(int healthAmount)
    {
        Health = healthAmount;
    }

    public virtual void SetHealthToMax()
    {
        SetHealth(maxHealth);
    }

    protected void AddHealth(int healthAmount)
    {
        Health += healthAmount;

        if (Health > maxHealth)
        {
            SetHealthToMax();
        }
    }

    public virtual void Damage(int damageAmount)
    {
        Health -= damageAmount;
        Damaged?.Invoke();
        if (Health <= 0)
        {
            Kill();
        }
    }

    protected virtual void Kill()
    {
        Killed?.Invoke();
    }
}


public interface IDamagable
{
    public void Damage(int damageAmount);
}
