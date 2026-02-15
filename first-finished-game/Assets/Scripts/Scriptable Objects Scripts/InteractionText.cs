using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Text", menuName = "Interaction/Text")]
public class InteractionText : ScriptableObject
{
    public string[] textToDisplay;
    public Sprite[] spritesToDisplay;

}
