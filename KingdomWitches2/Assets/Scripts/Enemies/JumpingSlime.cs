using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingSlime : Entity
{
    private int i;
    private bool flipRight;
    private Transform player;
    public bool canSeePlayer;
    public float speed = 3;
    public Transform[] movePoint;
    public float foundRadius = 6.5f;
    public float timeInvincible = 0.8f;
    public LayerMask groundLayer;
    
    private bool invincible;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Collider2D coll;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        player = (FindAnyObjectByType(typeof(Player)) as Player).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isNear())
        {

        }
    }

    private bool isGround()
    {
        RaycastHit2D raycastHit = Physics2D.CapsuleCast(coll.bounds.center, coll.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
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
