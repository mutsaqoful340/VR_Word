using UnityEngine;

public class NPCMoveByWaypoints : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 1.5f;
    public float rotateSpeed = 6f;
    public Animator animator;

    private int currentIndex = 0;
    private bool isMoving = false;

    void Update()
    {
        if (!isMoving || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        // ARAH KE TARGET
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        float distance = dir.magnitude;
        if (distance < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                StopMoving();
                return;
            }

            return;
        }

        // ROTASI HALUS KE ARAH TARGET
        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotateSpeed * 100f * Time.deltaTime
            );
        }

        // GERAK MAJU SETELAH ARAH CUKUP LURUS
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void StartMoving()
    {
        currentIndex = 0;
        isMoving = true;

        if (animator)
            animator.SetBool("isWalking", true);
    }

    void StopMoving()
    {
        isMoving = false;

        if (animator)
            animator.SetBool("isWalking", false);
    }
}
