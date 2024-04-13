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
    GameObject bulletSpawners;

    public Trigger activeTrigger;

    bool active = false;
    bool invincible = false;

    
    public GameObject bulletPrefab;
    public float timer = 0;
    void Start()
    {
        animator =  GetComponent<Animator>();
        bossSlider = bossBar.GetComponent<Slider>();
        bulletSpawners = GameObject.Find("BulletSpawner");

        bulletSpawners.SetActive(false);
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

        BossActing();


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
    
    void BossActing()
    {

        if (health >= 8) animator.SetInteger("stage", 0);
        if (8 > health && health > 4)
        {
            animator.SetTrigger("wakeup"); animator.SetInteger("stage", 2);
        }
        if(health <= 4) animator.SetInteger("stage", 1);

        switch (animator.GetInteger("stage"))
        { 
            case 0:
                
                bulletSpawners.SetActive(false);
                break;
            case 1:
                //transform.Rotate(Vector3.forward, Time.deltaTime *30);
                //Stage1();
                break;
            case 2:
                bulletSpawners.SetActive(false);

                break;
        }

    }

    void Stage1()
    {
        bulletSpawners.SetActive(true);
    }

    public void Jumped()
    {
        animator.SetTrigger("jumped");
        bulletSpawners.SetActive(true);
    }

    public void AttackEnd()
    {
        animator.SetTrigger("attackEnd");
        bulletSpawners.SetActive(false);
    }

    IEnumerator InvincibleState()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }
}
