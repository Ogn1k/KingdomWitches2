using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{

    Collider2D thiscollision = null;
    public bool entered = false;
    public bool disableOnEnter = false;

    private void Update()
    {
        if(disableOnEnter && entered)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Morph")
        {
            
            entered = true;
        }
        collision = thiscollision;
    }

    public Collider2D CollisionReturn()
    {
        return thiscollision;
    }
}
