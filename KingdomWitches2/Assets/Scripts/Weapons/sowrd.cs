using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class sowrd : Weapon
{

    Animator animator;
    private bool attackBlocked;
    public Transform player;

    public Transform holster;

    public int combo = 0;
    public float delay = 0.3f;
    public bool isAttack = false;

   // bool fired = false;

    void Start()
    {
        animator = GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponent<Player>().GetDirection() == Direction.LEFT) 
        { 
            transform.position = player.transform.position + new Vector3(0.29f, -0.12f, 0);
            holster.localScale = new Vector3(1, holster.localScale.y, holster.localScale.z);
        }
        else if(player.GetComponent<Player>().GetDirection() == Direction.RIGHT)
        {
            transform.position = player.transform.position + new Vector3(-0.29f, -0.12f, 0);
            holster.localScale = new Vector3(-1, holster.localScale.y, holster.localScale.z);
        }

        /*if(switched)
        {
            isAttack = false;
        }*/

        if (Input.GetButton("Fire1") && !switched)
        {
            isAttack = true;
            Attack();
        }
        else
        {
            isAttack = false;
            animator.SetBool("Attack", false);
            combo = 0;
            animator.SetBool("Attack2", false);
            attackBlocked = false;
        }
            
    }

    public void Attack()
    {
        if (attackBlocked) return;
        switch (combo)
        {
            case 0:
                animator.SetBool("Attack", true);
                combo = 1;
                break; 
            case 1:
                animator.SetBool("Attack", true);
                animator.SetBool("Attack2", true);
                combo = 0;
                break;
        }

        attackBlocked = true;
        StartCoroutine(DelayAttack());
        //isAttack = true;

    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);

        if(!isAttack) 
        {
            animator.SetBool("Attack", false);
            combo = 0;
        }
        animator.SetBool("Attack2", false); 
            
        
        attackBlocked = false;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if( collision.tag == "Enemy")
        {
            //babababa;
        }
    }
}
