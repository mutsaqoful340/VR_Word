using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSupGrabHook : MonoBehaviour
{
    XRGrabInteractable grab;
    VRPotSystem pot;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        pot = FindObjectOfType<VRPotSystem>(); // PREFAB SAFE
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        pot?.OnSupGrab();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        pot?.OnSupRelease();
    }
}
