using UnityEngine;

public class TextMoveToCamera : MonoBehaviour
{
    public Transform cameraTarget;
    public float speed = 1f;
    public float stopDistance = 2.8f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, cameraTarget.position);

        if (distance > stopDistance)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                cameraTarget.position,
                speed * Time.deltaTime
            );
        }
    }
}
