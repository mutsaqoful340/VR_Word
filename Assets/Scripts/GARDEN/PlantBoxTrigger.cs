using UnityEngine;

public class PlantBoxTrigger : MonoBehaviour
{
    [Header("Plant Box Settings")]
    public string visualID;
    public int targetCount = 3;

    [Header("Animator")]
    public Animator boxAnimator;

    [Header("Target Card Box")]
    public CardBox cardBox;

    private int currentCount = 0;
    private bool isCompleted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        PlantItem plant = other.GetComponent<PlantItem>();
        if (plant == null) return;

        if (plant.visualID != visualID) return;

        currentCount++;

        if (currentCount == 1 && boxAnimator != null)
            boxAnimator.SetTrigger("Open");

        if (currentCount >= targetCount)
            CompletePlantBox();

        Destroy(other.gameObject);
    }

    void CompletePlantBox()
    {
        isCompleted = true;

        if (boxAnimator != null)
            boxAnimator.SetTrigger("Complete");

        if (cardBox != null)
            cardBox.Unlock();
    }
}
