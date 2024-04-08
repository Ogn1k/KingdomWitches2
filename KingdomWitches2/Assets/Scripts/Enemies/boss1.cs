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

    public GameObject bulletPrefab;
    public float timer = 0;
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
        switch (animator.GetInteger("stage"))
        { 
            case 0:
                break;
            case 1:
                timer += Time.deltaTime;
                transform.Rotate(Vector3.forward, Time.deltaTime *30);
                if(timer >= 2)
                {
                    Stage1();
                    timer = 0;
                }    
                break;
        }

    }

    void Stage1()
    {
        float timer1=0;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        timer1 += Time.deltaTime;
        //bullet.transform.Translate(Vector3.right * 20 * Time.deltaTime);
        if (timer1 >= 3f)
        {
            Destroy(bullet);
            timer1= 0;
        }
 
            
            //bullet.GetComponent<Rigidbody2D>().AddForce(Vector3.right);
            
    }

    IEnumerator InvincibleState()
    {
        invincible = true;
        yield return new WaitForSeconds(1);
        invincible = false;
    }
}
