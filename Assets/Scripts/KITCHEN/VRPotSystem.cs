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

    [Header("Efek Sup Idle")]
    public float idleSpeed = 2f;
    public float idleHeight = 0.05f;

    [Header("Audio")]
    public AudioClip supJadiSFX;
    public float audioVolume = 1f;

    // ===== XR IDLE CONTROL =====
    Transform supObj;
    Vector3 idleBasePos;
    bool isGrabbed = false;
    Coroutine idleCoroutine;

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

        supObj = sup.transform;

        // 🔑 SIMPAN POSISI WORLD (XR SAFE)
        idleBasePos = supObj.position;

        // Start idle sekali
        idleCoroutine = StartCoroutine(IdleFloat());

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

    IEnumerator IdleFloat()
    {
        while (supObj != null)
        {
            if (!isGrabbed)
            {
                float yOffset = Mathf.Sin(Time.time * idleSpeed) * idleHeight;
                supObj.position = idleBasePos + Vector3.up * yOffset;
            }
            yield return null;
        }
    }

    // ===== DIPANGGIL DARI XR =====
    public void OnSupGrab()
    {
        isGrabbed = true;
    }

    public void OnSupRelease()
    {
        isGrabbed = false;

        // 🔑 UPDATE BASE POSISI BARU
        if (supObj != null)
            idleBasePos = supObj.position;
    }
}
