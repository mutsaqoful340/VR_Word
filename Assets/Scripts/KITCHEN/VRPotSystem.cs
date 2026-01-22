using UnityEngine;

public class VRPotSystem : MonoBehaviour
{
    [Header("Urutan Benar")]
    public string[] urutan = { "BAWANG", "CABAI", "TOMAT", "SELEDRI" };
    int index = 0;

    [Header("Spawn Sup")]
    public GameObject supPrefab;
    public Transform spawnPoint;

    [Header("Audio")]
    public AudioClip supJadiSFX;
    public float audioVolume = 1f;

    private void OnTriggerEnter(Collider other)
    {
        VRIngredient ing = other.GetComponent<VRIngredient>();
        if (ing == null) return;

        if (ing.namaBahan == urutan[index])
        {
            Debug.Log("BENAR: " + ing.namaBahan);
            Destroy(other.gameObject);
            index++;

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
        GameObject sup = Instantiate(
            supPrefab,
            spawnPoint.position,
            spawnPoint.rotation
        );

        // 🔊 AUDIO
        if (supJadiSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                supJadiSFX,
                spawnPoint.position,
                audioVolume
            );
        }

        // 🎆 PARTICLE
        ParticleSystem ps = sup.GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();

        Debug.Log("SUP JADI!");
    }
}
