using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public List<Weapon> weaponsOwned;

    private float attackRate = .1f;

    private float nextAttack;

    private int equippedWeaponID;
    private Weapon equippedWeapon;
    Player player;
    SpriteRenderer weaponRenderer;
    Transform weaponTransform;


    void Start()
    {
        player = Player.instance;
        weaponRenderer = GetComponentInChildren<SpriteRenderer>();
        weaponTransform = weaponRenderer.transform;
        InitWeapon();
    }

    private void InitWeapon()
    {
        equippedWeapon = weaponsOwned[equippedWeaponID];
        weaponRenderer.sprite = equippedWeapon.sprite;
    }
    void Update()
    {
        SwitchingWeapons();
        Attack();
    }
    private void SwitchingWeapons()
    {
        int weaponIDMod = 0;

        if (Input.GetButtonDown("Fire3"))
            weaponIDMod = -1;
        else if (Input.GetButtonDown("Fire4"))
            weaponIDMod = 1;

        if(weaponIDMod != 0) 
        {    
            equippedWeaponID = (equippedWeaponID + weaponIDMod + weaponsOwned.Count) % weaponsOwned.Count;
            InitWeapon();
        }
    }

    private void Attack()
    {
        if(Input.GetButtonDown("Fire2")) { }
            //weeee


    }


}
