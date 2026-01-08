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
            {
                Debug.Log("Masih salah");
                return;
            }
        }

        if (!sudahSpawn)
        {
            Instantiate(tomatPrefab, spawnPoint.position, Quaternion.identity);
            sudahSpawn = true;

            if (suaraBenar != null)
                suaraBenar.Play();

            Debug.Log("BENAR! TOMAT muncul");
        }
    }
}
