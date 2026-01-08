using UnityEngine;

public class VRWordManagerKitchen : MonoBehaviour
{
    [Header("Slot Huruf")]
    public VRLetterSlotKitchen[] slots;

    [Header("Spawn")]
    public GameObject tomatPrefab;
    public Transform spawnPoint;

    [Header("Audio")]
    public AudioSource suaraBenar;

    bool sudahSpawn = false;

    public void CheckWord()
    {
        foreach (VRLetterSlotKitchen slot in slots)
        {
            if (!slot.IsCorrect())
                return; // ada yang salah → stop
        }

        if (!sudahSpawn)
        {
            Instantiate(
                tomatPrefab,
                spawnPoint.position,
                spawnPoint.rotation   // ✅ ikut rotasi spawn point
            );

            sudahSpawn = true;

            if (suaraBenar != null)
                suaraBenar.Play();

            Debug.Log("SEMUA BENAR - TOMAT MUNCUL");
        }
    }
}
