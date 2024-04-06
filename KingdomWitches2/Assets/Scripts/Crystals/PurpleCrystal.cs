using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurpleCrystal : MonoBehaviour
{
    public float buffTime = 60;
    public int negativeEffectChance = 10;
    System.Random random = new System.Random();

    private bool activated = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player" && !activated)
        {
            activated = true;
            activated = true;
            Debug.Log("test");
            Player player = collision.gameObject.GetComponentInParent<Player>();
            if (player != null)
            {
                if (random.Next(0, 100) < negativeEffectChance)
                {
                    player.ApplyDamage(1);
                }
                else
                {
                    player.StartCoroutine(player.addBuff("challenge", 0, buffTime));
                }
                Destroy(gameObject);
            }
        }
    }
}
