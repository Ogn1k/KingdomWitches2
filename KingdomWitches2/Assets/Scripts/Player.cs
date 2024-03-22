using System;
using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public CapsuleCollider2D coll;
    public CapsuleCollider2D breakColl;

    public float maxSpeed = 7f;
    public float jumpForce = 30f;
    public float dashForce = 15f;
    public float dashTime = 0.2f;
    public float dashCooldown = 1f;

    public int health = 3;

    public bool damageImmunity = false;
    private bool flipRight = true;
    private bool isDashing = false;
    private bool canDash = true;
    private bool isJumping = false;

    public LayerMask groundLayer;
    public LayerMask breakbleLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        breakColl.enabled = false;
    }

    void Update()
    {
        if (isDashing)
        {
            return;
        }

        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

        if (move > 0 && !flipRight)
        {
            flip();
        }
        else if (move < 0 && flipRight)
        {
            flip();
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && isGround())
        {
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space) && isJumping)
        {
            isJumping = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.01f);
        }
        if (isJumping && rb.velocity.y <= 0f)
        {
            isJumping = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine(dash());
        }
    }
    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private void flip()
    {
        flipRight = !flipRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private bool isGround()
    {
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(coll.bounds.center, coll.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }

    public IEnumerator dash()
    {
        if (canDash)
        {
            canDash = false;
            isDashing = true;
            coll.excludeLayers += breakbleLayer;
            breakColl.enabled = true;
            float originalGravity = rb.gravityScale;
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(transform.localScale.x * dashForce, 0f);
            StartCoroutine(dmgImmunity(dashTime + 0.15f));
            yield return new WaitForSeconds(dashTime);
            rb.gravityScale = originalGravity;
            isDashing = false;
            breakColl.enabled = false;
            coll.excludeLayers -= breakbleLayer;
            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }

    public IEnumerator damage(int dmg, float immunityTime)
    {
        if (damageImmunity == true)
        {
            yield break;
        }
        health -= dmg;
        if (health <= 0)
        {
            Destroy(gameObject);
            RestartLevel();
            yield break;
        }
        StartCoroutine(dmgImmunity(immunityTime));
    }

    private IEnumerator dmgImmunity(float time)
    {
        damageImmunity = true;
        yield return new WaitForSeconds(time);
        damageImmunity = false;
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
