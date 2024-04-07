using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class boss1 : Entity
{
    public GameObject bossBar;
    Slider bossSlider;
    Animator animator;

    public Trigger activeTrigger;

    bool active = false;
    bool invincible = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        bossSlider = bossBar.GetComponent<Slider>();
        bossSlider.maxValue = maxHealth;
        bossBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(activeTrigger.entered)
        {
            active = true;
            bossBar.SetActive(true);
        }
        if (active) 
        {
            bossSlider.value = health;
            if (state == State.Died)
            {
                animator.SetTrigger("die");
                StartCoroutine(WaitTillDeath());
                
            }
        }
        
    }

    IEnumerator WaitTillDeath()
    {
        yield return new WaitForSeconds(1.3f);
        active = false;
        bossBar.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Weapon" && !invincible) 
        {
            Weapon weapon = collision.collider.GetComponent<Weapon>();
            SubtractHealth(weapon.damage);
            StartCoroutine(InvincibleState());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Weapon" && !invincible)
        {
            Weapon weapon = other.GetComponent<Weapon>();
            SubtractHealth(weapon.damage);
            StartCoroutine(InvincibleState());
        }
    }

    IEnumerator InvincibleState()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }
}
