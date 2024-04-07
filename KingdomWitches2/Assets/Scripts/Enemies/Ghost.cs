using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Entity
{
    public bool canSeePlayer;
    public float speed = 3;
    public float foundRadius = 6.5f;
    public float timeInvincible = 0.8f;
    public Transform[] movePoint;
    
    private int i;
    private float minFoundRadius;
    private float maxFoundRadius;
    private bool flipRight;
    private Transform player;
    private bool invincible;

    private SpriteRenderer sr;
    void Start()
    {
        minFoundRadius = foundRadius;
        maxFoundRadius = foundRadius * 1.3f;
        sr = GetComponent<SpriteRenderer>();
        player = (FindAnyObjectByType(typeof(Player)) as Player).transform;
    }
    
    void Update()
    {
        if (isNear() && canSeePlayer)
        {
            foundRadius = maxFoundRadius;
            move(player);
        }
        else if (movePoint.Length > 0)
        {
            foundRadius = minFoundRadius;
            move(movePoint[i]);
            if (Vector2.Distance(transform.position, movePoint[i].position) < 0.2f)
            {
                if (i == movePoint.Length - 1)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
        }
    }

    private void move(Transform target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if ((transform.position.x > target.position.x) && !flipRight)
        {
            flip();
        }
        else if ((transform.position.x < target.position.x) && flipRight)
        {
            flip();
        }
    }

    private void flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private bool isNear()
    {
        return Vector2.Distance(transform.position, player.position) < foundRadius; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Weapon" && !invincible)
        {
            Weapon weapon = collision.GetComponent<Weapon>();
            health -= weapon.damage;
            if (health <= 0)
            {
                Destroy(gameObject); 
                return;
            }
            StartCoroutine(SetInvincible());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Weapon" && !invincible)
        {
            Weapon weapon = collision.collider.GetComponent<Weapon>();
            health -= weapon.damage;
            if (health <= 0)
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(SetInvincible());
        }
    }

    private IEnumerator SetInvincible()
    {

        invincible = true;
        float elapsedTime = 0f;
        speed /= 3;

        while (elapsedTime < timeInvincible)
        {

            sr.enabled = !sr.enabled;
            elapsedTime += .04f;
            yield return new WaitForSeconds(.04f);

        }

        sr.enabled = true;
        invincible = false;
        speed *= 3;

    }
}