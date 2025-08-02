using System;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
public class HealthSystem  
{
    [SerializeField] private float currentHealth;
    [SerializeField] private CharacterStat maxHealth = new CharacterStat();
    [SerializeField] private bool isAlive = true;
    public event Action EventEntityDeath;

    public float GetCurrentHealth {  get { return currentHealth; } }
    public float GetMaxHealth {  get { return maxHealth.Value; } }
    public float GetHealthPrecent { get { return (currentHealth / maxHealth.Value) * 100f; } }
    public HealthSystem(float maxHP)
    {
        maxHealth = new CharacterStat(maxHP);
        currentHealth = maxHealth.Value;
    }
    public HealthSystem()
    {
    }

    public void AddModifer(StatModifier mod)
    {
        maxHealth.AddModifier(mod);
    }
    public void AddHealth(float health)
    {
        if (isAlive) 
        {
            if (currentHealth + health >= maxHealth.Value)
            {
                currentHealth = maxHealth.Value;
            }
            else
            {
                currentHealth += health;
            }
        }
    }

    public void RemoveHealth(float health) 
    {
        if (isAlive)
        {
            currentHealth -= health;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                EventEntityDeath?.Invoke();
            }
        }
    }
}