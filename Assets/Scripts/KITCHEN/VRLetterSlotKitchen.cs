using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRLetterSlotKitchen : MonoBehaviour
{
    [Header("Setting")]
    public string correctLetter;

    [HideInInspector]
    public string currentLetter = "";

    public void SetLetter(string letter)
    {
        currentLetter = letter;
    }

    public bool IsCorrect()
    {
        return currentLetter == correctLetter;
    }


}
