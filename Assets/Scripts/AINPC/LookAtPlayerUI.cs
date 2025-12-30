using UnityEngine;

public class LookAtPlayerUI : MonoBehaviour
{
    public Transform playerCamera;

    void LateUpdate()
    {
        if (playerCamera == null) return;
        transform.LookAt(playerCamera);
        transform.Rotate(0, 180f, 0);
    }
}
