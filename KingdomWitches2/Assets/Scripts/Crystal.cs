using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject LootableCrystal;

    private void OnDestroy()
    {
        GameObject LCrystal = Instantiate(LootableCrystal, transform.position, transform.rotation) as GameObject;
    }
}
