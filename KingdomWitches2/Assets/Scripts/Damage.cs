using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage = 1;
    public float immunityTime = 1.5f;
    private Player player;

    void Start()
    {
        player = Player.FindFirstObjectByType(typeof(Player)) as Player;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.StartCoroutine(player.damage(damage, immunityTime));
        }
    }
}
