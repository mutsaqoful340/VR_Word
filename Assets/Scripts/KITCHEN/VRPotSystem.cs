using UnityEngine;

public class VRPotSystem : MonoBehaviour
{
    [Header("Urutan Benar")]
    public string[] urutan = { "BAWANG", "CABAI", "TOMAT", "SELEDRI" };
    int index = 0;

    [Header("Sup di Scene")]
    public GameObject supObject; // Sup sudah ada di scene

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
                AktifkanSup();
            }
        }
        else
        {
            Debug.Log("SALAH URUTAN!");
            ing.Kembali();
        }
    }

    void AktifkanSup()
    {
        // ✅ AKTIFKAN SUP
        supObject.SetActive(true);

        // 🔊 AUDIO
        if (supJadiSFX != null)
        {
            AudioSource.PlayClipAtPoint(
                supJadiSFX,
                supObject.transform.position,
                audioVolume
            );
        }

        // 🎆 PARTICLE
        ParticleSystem ps = supObject.GetComponentInChildren<ParticleSystem>();
        if (ps != null) ps.Play();

        Debug.Log("SUP JADI!");
    }
}
