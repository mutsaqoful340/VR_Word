using UnityEngine;

public class RandomFloat : MonoBehaviour
{
    public float range = 0.15f;
    public float speed = 0.5f;

    Vector3 startPos;
    Vector3 targetPos;

    void Start()
    {
        startPos = transform.localPosition;
        SetNewTarget();
    }

    void SetNewTarget()
    {
        targetPos = startPos + new Vector3(
            Random.Range(-range, range),
            Random.Range(-range, range),
            Random.Range(-range, range)
        );
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            Time.deltaTime * speed
        );

        if (Vector3.Distance(transform.localPosition, targetPos) < 0.02f)
        {
            SetNewTarget();
        }
    }
}
