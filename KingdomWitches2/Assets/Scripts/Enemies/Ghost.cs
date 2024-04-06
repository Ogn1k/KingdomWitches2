using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Entity
{
    private int i;
    private bool flipRight;
    private Transform player;
    public bool canSeePlayer;
    public float speed = 3;
    public Transform[] movePoint;
    public float foundRadius = 6.5f;
    public float timeInvincible = 0.8f;
    private bool invincible;

    private SpriteRenderer sr;
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        player = (FindAnyObjectByType(typeof(Player)) as Player).transform;
    }
    
    void Update()
    {
        if (isNear())
        {
            Debug.Log("123");
        }
        if (isNear() && canSeePlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            if ((transform.position.x > player.position.x) && !flipRight)
            {
                flip();
            }
            else if ((transform.position.x < player.position.x) && flipRight)
            {
                flip();
            }
        }
        else if (movePoint.Length > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, movePoint[i].position, speed * Time.deltaTime);
            if ((transform.position.x > movePoint[i].position.x) && !flipRight)
            {
                flip();
            }
            else if ((transform.position.x < movePoint[i].position.x) && flipRight)
            {
                flip();
            }
        }
        else
        {
            return;
        }

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