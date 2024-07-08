using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct Drop
{
    public GameObject dropItem;
    public int dropChance;
    public int dropAmount;
}

public class Entity : MonoBehaviour
{
    public int maxHealth = 3;
    [SerializeField] public int health;

    [SerializeField] public List<Drop> dropList = new List<Drop>();

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
        if (dropList != null) ///дроп с моба
            foreach (var item in dropList)
            {
                System.Random rNum = new System.Random();
                if (rNum.Next(0, 100) < item.dropChance)
                {
                    for (int i = 0; i < item.dropAmount; i++)
                    {
                        GameObject drop = Instantiate(item.dropItem, transform.position, transform.rotation) as GameObject;
                    }
                }
            }

        //
    }
}
