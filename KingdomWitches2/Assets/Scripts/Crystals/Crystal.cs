using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject LootableCrystal;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon")
        {
            GameObject LCrystal = Instantiate(LootableCrystal, transform.position, transform.rotation) as GameObject;
            Destroy(gameObject);
        }
    }
}
