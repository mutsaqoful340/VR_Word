using UnityEngine;

public class PlantBoxTrigger : MonoBehaviour
{
    [Header("Plant Box Settings")]
    public string visualID;
    public int targetCount = 3;

    [Header("Animator")]
    public Animator boxAnimator;

    [Header("Target Card Box (SHARED)")]
    public CardBox cardBox;

    // ===== SHARED (GABUNGAN SEMUA BOX) =====
    public static int totalCurrent = 0;
    public static int totalTarget = 20;   // TOTAL GABUNGAN
    private static bool cardUnlocked = false;

    // ===== PER BOX =====
    private int currentCount = 0;
    private bool isCompleted = false;
    private bool isBoxOpen = false;

    // Dipanggil saat wortel / semangka DIPEGANG
    public void OpenBox()
    {
        if (isCompleted || isBoxOpen) return;

        isBoxOpen = true;
        if (boxAnimator != null)
            boxAnimator.SetTrigger("Open");
    }

    // Dipanggil saat item MASUK ke box
    public void CloseBox()
    {
        if (isCompleted || !isBoxOpen) return;

        isBoxOpen = false;
        if (boxAnimator != null)
            boxAnimator.SetTrigger("Close");
    }

    


    void CompletePlantBox()
    {
        isCompleted = true;

        if (boxAnimator != null)
            boxAnimator.SetTrigger("Complete");
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isCompleted) return;

        PlantItem plant = other.GetComponent<PlantItem>();
        if (plant == null) return;

        if (plant.visualID != visualID) return;

        // ===== HITUNG =====
        currentCount++;
        totalCurrent++;

        // ===== ANIMASI =====
        CloseBox();

        if (currentCount >= targetCount)
            CompletePlantBox();

        // ===== CEK TOTAL GABUNGAN =====
        if (!cardUnlocked && totalCurrent >= totalTarget)
        {
            cardUnlocked = true;
            if (cardBox != null)
                cardBox.Unlock();
        }

        Destroy(other.gameObject);
    }

    

    // ===== OPTIONAL: RESET SAAT LEVEL DIULANG =====
    public static void ResetGlobal()
    {
        totalCurrent = 0;
        cardUnlocked = false;
    }
}
