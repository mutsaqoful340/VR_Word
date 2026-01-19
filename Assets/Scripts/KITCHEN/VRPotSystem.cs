using UnityEngine;
using System.Collections;

public class VRPotSystem : MonoBehaviour
{
    [Header("Urutan Benar")]
    public string[] urutan = { "BAWANG", "CABAI", "TOMAT", "SELEDRI" };
    int index = 0;

    [Header("Spawn Sup")]
    public GameObject supPrefab;
    public Transform spawnPoint;

    [Header("Efek Sup")]
    public float idleSpeed = 2f;
    public float idleHeight = 0.05f;

    [Header("Audio")]
    public AudioClip supJadiSFX;   // suara sup jadi
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
        GameObject sup = Instantiate(supPrefab, spawnPoint.position, spawnPoint.rotation);

        // 🔊 PLAY AUDIO (TANPA AUDIOMANAGER)
        if (supJadiSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                supJadiSFX,
                spawnPoint.position,
                audioVolume
            );
        }

        // PLAY PARTICLE SYSTEM
        ParticleSystem ps = sup.GetComponentInChildren<ParticleSystem>();
        if (ps != null)
        {
            ps.Play();
        }

        // IDLE GERAK ATAS BAWAH
        StartCoroutine(IdleFloat(sup.transform));

        Debug.Log("SUP JADI!");
    }

    IEnumerator IdleFloat(Transform obj)
    {
        Vector3 startPos = obj.position;

        while (obj != null)
        {
            float yOffset = Mathf.Sin(Time.time * idleSpeed) * idleHeight;
            obj.position = startPos + new Vector3(0, yOffset, 0);
            yield return null;
        }
    }
}
