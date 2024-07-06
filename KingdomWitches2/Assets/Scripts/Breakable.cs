using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Breakable : Entity
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            Weapon weapon = collision.GetComponent<Weapon>();
            SubtractHealth(weapon.damage);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Weapon")
        {
            Weapon weapon = collision.collider.GetComponent<Weapon>();
            SubtractHealth(weapon.damage);
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
