using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New WEapon", menuName = "Item/Weapon")]
public class Weapon : ScriptableObject
{
    public new string name = "Name";
    [TextArea(3,5)]
    public string description = "Description";

    public Sprite sprite;

    public int handleOffsetX = -11;
    public int handleOffsetY = -1;

    public AudioClip soundPrefab;


}
