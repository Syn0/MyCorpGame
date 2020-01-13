using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character#NoName", menuName = "MYCORP/Character", order = 2)]
public class Character : ScriptableObject
{
    public string Char_Name;
    public Color backgroundColor;
    public AudioClip DialogClip;
    public Sprite sprite;
    public bool hasEyes = true;
}
