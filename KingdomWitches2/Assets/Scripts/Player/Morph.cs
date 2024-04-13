using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morph : MonoBehaviour
{
    public List<MorphObject> morphs;
    public List<GameObject> morphObjects;
    public int morhpCount;
    
    GameObject currentMorph;
    int morphID;
    Vector3 playerPosition;
    
    public Transform player;
    public FollowCamera fcamera;

    Player playerCpt;

    //public Morph instance;

    void Start()
    {
        //instance = this;
        playerPosition = player.position;
        morhpCount = morphs.Count;
        playerCpt = player.GetComponent<Player>();

        morphObjects.Clear();
        for (int i = 0; i < morhpCount; i++)
        {
            if (morphs[i].MorphTag == "Player")
            {
                morphObjects.Add(player.gameObject);
            }
            else
            {
                morphObjects.Add(Instantiate(morphs[i].morphPrefab, new Vector3(0, 0, 0), Quaternion.identity));
                morphObjects[i].SetActive(false);
            }
        }
        
        if (morhpCount > 0)
        {
            InitMorphs();
        }
    }

    private void Update()
    {
        NextMorph();   
    }

    private void InitMorphs()
    {
        foreach (GameObject morph in morphObjects)
        {
            if (morph.tag == "Player")
                morph.transform.Find("Holster").GetComponent<WeaponController>().HideWeapons();
            morph.gameObject.SetActive(false);
        }
        currentMorph = morphObjects[morphID];
        currentMorph.transform.position = playerPosition;
        if (currentMorph.tag != "Player")
        {
            player.GetComponent<Player>().canDash = true;
            currentMorph.tag = "Morph";
        }
            
        currentMorph.transform.rotation = new Quaternion(0, 0, 0, 0);
        fcamera.playerTransform = currentMorph.transform;
        currentMorph.gameObject.SetActive(true);
    }

    public void NextMorph()
    {
        if (Input.GetButtonDown("R") && !playerCpt.isDashing && playerCpt.health > 0 )
        {
            //
            playerPosition = currentMorph.transform.position;
            morphID = (morphID + 1 + morhpCount) % morhpCount;
            InitMorphs();
        }
    }
}
