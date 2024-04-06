using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueCrystal : MonoBehaviour
{
    public int movespeedUp = 3;
    public float buffTime = 60;
    public int negativeEffectChance = 10;
    System.Random random = new System.Random();

    private bool activated = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !activated)
        {
            activated = true;
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                if (random.Next(0, 100) < negativeEffectChance)
                {
                    player.ApplyDamage(1);
                }
                else
                {
                    player.StartCoroutine(player.addBuff("speed", movespeedUp, buffTime));
                }
            Destroy(gameObject);
            }
        }
    }
}
