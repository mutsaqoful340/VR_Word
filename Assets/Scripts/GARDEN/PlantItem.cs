using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlantItem : MonoBehaviour
{
    [Header("Visual Group ID")]
    public string visualID;

    [Header("Target Box")]
    public PlantBoxTrigger targetBox;

    [HideInInspector]
    public bool isCounted = false;

    private bool hasOpenedBox = false;

    void Awake()
    {
        if (GetComponent<XRGrabInteractable>() == null)
            gameObject.AddComponent<XRGrabInteractable>();
    }

    void OnEnable()
    {
        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
    }

    void OnDisable()
    {
        XRGrabInteractable grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.RemoveListener(OnGrab);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (hasOpenedBox) return;
        hasOpenedBox = true;

        if (targetBox != null && !targetBox.IsCompleted())
            targetBox.OpenBox();
    }
}
