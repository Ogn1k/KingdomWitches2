using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class pitchfork : Weapon
{
    Vector3 throwVector;
    Rigidbody2D rb;
    LineRenderer lr;
    Vector3 mousePos;
    PolygonCollider2D bc;
    SerializedProperty asd;
    bool canPickUp;
    public bool thrusted = false;

    public WeaponController controller;
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        lr = this.GetComponent<LineRenderer>();
        bc = this.GetComponent<PolygonCollider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        bc.enabled = false;

        
        transform.SetParent(GameObject.Find("Holster").transform);
        //controller = transform.gameObject.GetComponent<WeaponController>();
    }

    
    void Update()
    {
        if (!fired) 
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            gameObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);

            if (Input.GetButtonDown("Fire1"))
            {
                transform.SetParent(null);
                rb.constraints = RigidbodyConstraints2D.None;
                bc.enabled = true;
                CalculateThrowVector();
                SetArrow();
                Throw();
                StartCoroutine(PickUpDelay());
                fired = true;
                controller.NextWeapon();
                controller.weaponCount -= 1;
            }
        }
        else
        {
            if (Math.Abs(rb.velocity.y + rb.velocity.x) >= 5f)
            {
                bc.tag = "Weapon";
            }
            else
            {
                bc.tag = "Untagged";
            }
        }

    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(fired) 
        {
            if(col.gameObject.CompareTag("Player") && canPickUp)
            {
                rb.freezeRotation = false;
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                bc.enabled = false;
                transform.SetParent(GameObject.Find("Holster").transform);
                transform.position = col.transform.position + new Vector3(0,1,0);
                fired = false;
                thrusted = false;
                controller.NextWeapon();
                controller.weaponCount += 1;
            }
            if(col.gameObject.CompareTag("Enemy"))
            {
                Vector3 link = new Vector3(0,0,0);
                if (!thrusted)
                { 
                    link = transform.position;
                    thrusted = true;
                }
                rb.constraints = RigidbodyConstraints2D.FreezePosition;
                rb.freezeRotation = true;
               
                //transform.position = col.transform.position + link;
                transform.SetParent(col.gameObject.transform);

                if (col.gameObject.GetComponent<Entity>().state == Entity.State.Died)
                {
                    thrusted = false;
                    transform.SetParent(null);
                    rb.constraints = RigidbodyConstraints2D.None;
                    //bc.enabled = true;
                    rb.freezeRotation = false;
                }
            }
        }

    }

    void CalculateThrowVector()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 distance = mousePos - this.transform.position;
        throwVector = distance.normalized * 1000;
    }

    void SetArrow()
    {
        lr.positionCount = 2;
        lr.SetPosition(0, Vector3.zero);
        lr.SetPosition(1, throwVector.normalized/2);
        lr.enabled = true;
    }

    public void Throw()
    {
        
        rb.AddForce(throwVector);
    }

    IEnumerator PickUpDelay()
    {
        canPickUp = false;
        gameObject.layer = 0;
        rb.excludeLayers = LayerMask.GetMask("Player");
        yield return new WaitForSeconds(0.4f);
        canPickUp = true;
        gameObject.layer = 3;
        rb.excludeLayers = LayerMask.GetMask("Nothing");
    }
}
