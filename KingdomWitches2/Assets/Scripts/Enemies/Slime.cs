using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Slime : Entity
{
    private int i;
    private bool flipRight;
    private Transform player;
    public bool canSeePlayer;
    public float speed = 3;
    public Transform[] movePoint;
    public float foundRadius = 6.5f;
    public float timeInvincible = 0.8f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    private Collider2D coll;
    private bool invincible;
    private bool canJump = true;

    private SpriteRenderer sr;
    void Start()
    {
        coll = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = (FindAnyObjectByType(typeof(Player)) as Player).transform;
    }

    void Update()
    {
        if (isNear() && canSeePlayer)
        {
            move(player.transform);
        }
        else if (movePoint.Length > 0)
        {
            move(movePoint[i]);
            if (Vector2.Distance(transform.position, movePoint[i].position) < 0.3f)
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
        else
        {
            rb.velocity = new Vector2(0, 0);
        }

    }

    private void move(Transform target)
    {
        rb.velocity = new Vector2(transform.localScale.x * speed, rb.velocity.y);
        if (isWallInFront() && onGround() && canJump)
        {
            StartCoroutine(cooldown(0.7f));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        else if (onGround() && Math.Abs(transform.position.x - target.position.x) <= 2f && target.position.y - transform.position.y >= 3f && canJump)
        {
            StartCoroutine(cooldown(0.7f));
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
        if (Math.Abs(transform.position.x - target.position.x) < 1)
        {
            return;
        }
        if ((transform.position.x > target.position.x) && !flipRight)
        {
            flip();
        }
        else if ((transform.position.x < target .position.x) && flipRight)
        {
            flip();
        }
    }

    private bool isWallInFront()
    {
        RaycastHit2D frontRay;
        if (flipRight)
        {
            frontRay = Physics2D.Raycast(coll.bounds.center, Vector2.left, coll.bounds.size.x + .1f, groundLayer);
        }
        else
        {
            frontRay = Physics2D.Raycast(coll.bounds.center, Vector2.right, coll.bounds.size.x + .1f, groundLayer);
        }
        return frontRay.collider != null;
    }

    private bool onGround()
    {
        RaycastHit2D groundBox = Physics2D.Raycast(coll.bounds.center, Vector2.down, coll.bounds.size.y + .01f, groundLayer);
        return groundBox.collider != null;
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

    private IEnumerator cooldown(float cd)
    {
        canJump = false;
        yield return new WaitForSeconds(cd);
        canJump = true;
    }
}
