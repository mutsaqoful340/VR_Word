using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabIdleHook : MonoBehaviour
{
    XRGrabInteractable grab;
    GameRoundManager gameManager;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        // 🔥 PREFAB SAFE — cari di Scene saat runtime
        gameManager = FindObjectOfType<GameRoundManager>();
    }

    void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        if (gameManager != null)
            gameManager.OnGrab();
    }

    void OnReleased(SelectExitEventArgs args)
    {
        if (gameManager != null)
            gameManager.OnRelease();
    }
}
