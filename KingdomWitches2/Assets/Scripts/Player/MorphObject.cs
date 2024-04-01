using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New morph object", menuName = "Custom/Morph")]
public class MorphObject : ScriptableObject
{
    public new string name = "NAME";
    [TextArea(3, 5)]
    public string description = "DESCRIPTION";

    public GameObject morphPrefab;

    public string MorphTag;

    public AudioClip SoundPrefab;
}
