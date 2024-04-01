using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public List<Weapon> weaponsOwned;
    public int weaponCount;

    private float attackRate = .1f;

    private float nextAttack;

    public Vector2 PointerPosition { get; set; }

    private int equippedWeaponID;
    private Weapon equippedWeapon;
    Player player;


    void Start()
    {
        player = Player.instance;
        weaponCount = weaponsOwned.Count;
        if (weaponCount > 0)
        {
            InitWeapon();
        }
    }

    private void InitWeapon()
    {
        foreach (Weapon item in weaponsOwned)
        {
            if (!item.fired)
            {
                item.switched = true;
                item.gameObject.SetActive(false);
            }
        }
        equippedWeapon = weaponsOwned[equippedWeaponID];
        equippedWeapon.gameObject.SetActive(true);
        equippedWeapon.switched = false;
    }
    void Update()
    {
        
        SwitchingWeapons();
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
            equippedWeaponID = (equippedWeaponID + weaponIDMod + weaponCount) % weaponCount;
            InitWeapon();
        }
    }

    public void NextWeapon()
    {
        equippedWeaponID = (equippedWeaponID + 1 + weaponCount) % weaponCount;
        InitWeapon();
    }


}
