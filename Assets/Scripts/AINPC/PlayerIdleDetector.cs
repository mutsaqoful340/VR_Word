using UnityEngine;

public class PlayerIdleDetector : MonoBehaviour
{
    public Transform playerCamera;
    public float idleThreshold = 8f;

    private Vector3 lastPos;
    private float idleTimer = 0f;

    void Start()
    {
        if (playerCamera != null)
            lastPos = playerCamera.position;
    }

    void Update()
    {
        if (playerCamera == null) return;

        float movement = Vector3.Distance(playerCamera.position, lastPos);

        if (movement < 0.01f)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0f;
            lastPos = playerCamera.position;
        }

        if (IsPlayerIdle())
        {
            Debug.Log("Player diam, NPC siap bantu");
        }
    }

    // NPC "bertanya": player lagi diam?
    public bool IsPlayerIdle()
    {
        return idleTimer >= idleThreshold;
    }

    // Reset kalau player mulai interaksi
    public void ResetIdle()
    {
        idleTimer = 0f;
    }
}
