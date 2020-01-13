using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card#000", menuName = "MYCORP/Card", order = 1)]
public class Card : ScriptableObject
{
    public string dialog_string =  "dialog a afficher ?";
    public string AnswerLeft_string = "reponse gauche";
    public string AnswerRight_string = "reponse droite";
    public Character character;

    //███ LEFT ████
    public CategoryEffect[] left_categoryEffect;
    public Card[] left_cardListToAdd;

    //███ RIGHT ████
    public CategoryEffect[] right_categoryEffect;
    public Card[] right_cardListToAdd;

}

[System.Serializable]
public class CategoryEffect
{
    public int impact;
    public float value;
}