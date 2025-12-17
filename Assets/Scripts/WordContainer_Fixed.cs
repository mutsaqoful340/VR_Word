using UnityEngine;
using System.Collections.Generic;
using System.Linq;
// Note: UnityEngine.XR.Interaction.Toolkit.Interactables is not strictly needed here

public class WordContainer_Fixed : MonoBehaviour
{
    public Animator WordContainerAnim;


    public void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered by: {other.gameObject.name}, Tag: {other.tag}");
        
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            WordContainerAnim.SetTrigger("PlayerIN");
            Debug.Log("Player entered the word container trigger.");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log($"Trigger exited by: {other.gameObject.name}, Tag: {other.tag}");
        
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            WordContainerAnim.SetTrigger("PlayerOUT");
        }
    }
}