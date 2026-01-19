using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    public GameObject menuUI;
    public InputActionProperty menuButtonAction;  // Assign in Inspector
    
    private bool isMenuOpen = false;

    void Start()
    {
        // Sync the initial state with the actual menu visibility
        if (menuUI != null)
        {
            menuUI.SetActive(isMenuOpen);
        }
    }

    void OnEnable()
    {
        menuButtonAction.action.Enable();
    }

    void OnDisable()
    {
        menuButtonAction.action.Disable();
    }

    void Update()
    {
        if (menuButtonAction.action.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        menuUI.SetActive(isMenuOpen);
    }
}
