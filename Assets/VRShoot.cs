using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRShoot : MonoBehaviour
{
    public SimpleShoot simpleShoot;
    public InputActionProperty triggerAction;

    private XRGrabInteractable grabInteractable;
    private AudioSource audioSource;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        triggerAction.action.Enable();
    }

    void OnDisable()
    {
        triggerAction.action.Disable();
    }

    void Update()
    {
        if (!grabInteractable.isSelected)
            return;

        if (triggerAction.action.WasPressedThisFrame())
        {
            simpleShoot.StartShoot();
            audioSource.Play();
        }
    }
}
