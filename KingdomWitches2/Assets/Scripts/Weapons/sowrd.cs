using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class sowrd : Weapon
{

    Animator animator;
    bool attackBlocked;

    public Transform player;
    public Transform holster;

    public int combo = 0;
    public float delay = .5f;

   // bool fired = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        tag = "Untagged";
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

        if (Input.GetButton("Fire1") && !switched)
        {
            if (attackBlocked) return;
            animator.SetInteger("combo", combo);
            switch(combo)
            {
                case 0:
                    animator.SetTrigger("AttackTr");
                    combo = 1;
                    break;
                case 1:
                    animator.SetTrigger("AttackTr2");
                    combo = 0;
                    break;
            }
            tag = "Weapon";
            attackBlocked = true;
            StartCoroutine(DelayAttack());
        }

    }

    public override void WeaponSwitched()
    {
        combo = 0;
        attackBlocked = false;
        //animator.ResetTrigger("AttackTr");
        //animator.ResetTrigger("AttackTr2");
    }

    IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
        tag = "Untagged";
    }

}
