using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public int maxHealth = 3;
    [SerializeField] public int health;

    public enum State
    {
        Living,
        Died
    };

    public State state;

    protected virtual void Awake()
    {
        health = maxHealth;
        state = State.Living;
    }

    protected void AddHealth(int m) { ModHealth(m); }
    protected void SubtractHealth(int m) { ModHealth(-m); }

    private void ModHealth(int m)
    {
        health += m;
        CapHealth();
    }

    private void CapHealth()
    {
        if (health <= 0)
        {
            health = 0;
            Die();
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    protected virtual void Die()
    {
        state = State.Died;
        Debug.Log("obj " + gameObject.name + " died");
        //
    }
}
