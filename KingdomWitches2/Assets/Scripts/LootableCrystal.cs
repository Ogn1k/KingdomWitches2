using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootableCrystal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();

            Destroy(gameObject);
        }
    }
}
