using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    public int damage = 1;
    private Player player;

    void Start()
    {
        player = Player.FindFirstObjectByType(typeof(Player)) as Player;
    }

    void Update()
    {
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            player.ApplyDamage(damage);
        }
    }
}
