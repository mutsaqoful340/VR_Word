using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PlantItem : MonoBehaviour
{
    [Header("Visual Group ID")]
    public string visualID;
    // Contoh: "RED", "BLUE", "STAR"

    [Header("Target Box")]
    public PlantBoxTrigger targetBox;

    private bool hasOpenedBox = false;

    void Awake()
    {
        // Pastikan item bisa di-grab
        if (GetComponent<XRGrabInteractable>() == null)
        {
            gameObject.AddComponent<XRGrabInteractable>();
        }
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
        Debug.Log("WORTEL DIPEGANG");

        if (hasOpenedBox) return;
        hasOpenedBox = true;

        if (targetBox != null)
            targetBox.OpenBox();
    }

}
