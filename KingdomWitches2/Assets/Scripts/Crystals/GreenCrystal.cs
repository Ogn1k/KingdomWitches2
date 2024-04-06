using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenCrystal : MonoBehaviour
{
    public int jumpUp = 3;
    public float buffTime = 60;
    public int negativeEffectChance = 10;
    System.Random random = new System.Random();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                if (random.Next(0, 100) < negativeEffectChance)
                {
                    player.ApplyDamage(1);
                }
                else
                {
                    player.StartCoroutine(player.addBuff("jump", jumpUp, buffTime));
                }
            Destroy(gameObject);
            }
        }
    }
}
