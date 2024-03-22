using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    private int i;
    private bool flipRight;
    public float speed = 3;
    public Transform[] movePoint;
    public Transform player;
    public LayerMask playerLayer;
    public float foundRadius = 6.5f;

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
        else
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

        if (Vector2.Distance(transform.position, movePoint[i].position) < 0.2f)
        {
            if (i > 0)
            {
                i = 0;
            }
            else
            {
                i = 1;
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
        RaycastHit2D raycastHit = Physics2D.CircleCast(transform.position, foundRadius, Vector2.right, 0f, playerLayer);
        return raycastHit.collider != null;
    }

}