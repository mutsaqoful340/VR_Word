using UnityEngine;

public class VRPotSystem : MonoBehaviour
{
    [Header("Urutan Benar")]
    public string[] urutan = { "BAWANG", "CABAI", "TOMAT", "SELEDRI" };

    int index = 0;

    [Header("Spawn Sup")]
    public GameObject supPrefab;
    public Transform spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        VRIngredient ing = other.GetComponent<VRIngredient>();

        if (ing == null) return;

        // CEK URUTAN
        if (ing.namaBahan == urutan[index])
        {
            Debug.Log("BENAR: " + ing.namaBahan);

            Destroy(other.gameObject); // masuk panci
            index++;

            // SEMUA SELESAI
            if (index >= urutan.Length)
            {
                SpawnSup();
            }
        }
        else
        {
            Debug.Log("SALAH URUTAN!");
            ing.Kembali();
        }
    }

    void SpawnSup()
    {
        Instantiate(supPrefab, spawnPoint.position, spawnPoint.rotation);
        Debug.Log("SUP JADI!");
    }
}
