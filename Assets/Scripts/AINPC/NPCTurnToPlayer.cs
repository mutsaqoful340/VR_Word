using UnityEngine;

public class NPCTurnToPlayer : MonoBehaviour
{
    public Transform playerCamera;
    public float rotateSpeed = 2f;
    public float activeDistance = 2.5f;

    void Update()
    {
        if (playerCamera == null) return;

        float distance = Vector3.Distance(
            transform.position,
            playerCamera.position
        );

        if (distance <= activeDistance)
        {
            Vector3 direction = playerCamera.position - transform.position;
            direction.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotateSpeed * Time.deltaTime
            );
        }
    }
}
