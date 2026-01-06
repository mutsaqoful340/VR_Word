using UnityEngine;

public class BoxTrigger : MonoBehaviour
{
    [Header("Box Settings")]
    public string visualID;
    public Animator boxAnimator;

    [Header("Feedback")]
    public AudioSource openSound;

    private bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        PlantItem plant = other.GetComponent<PlantItem>();
        if (plant == null) return;

        if (plant.visualID == visualID)
        {
            AcceptPlant(other.gameObject);
        }
        else
        {
            RejectPlant(other.gameObject);
        }
    }

    void AcceptPlant(GameObject plant)
    {
        if (!isOpened)
        {
            isOpened = true;

            if (boxAnimator != null)
                boxAnimator.SetTrigger("Open");

            if (openSound != null)
                openSound.Play();
        }

        Destroy(plant);
    }

    void RejectPlant(GameObject plant)
    {
        Rigidbody rb = plant.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 2f, ForceMode.Impulse);
        }
    }
}
