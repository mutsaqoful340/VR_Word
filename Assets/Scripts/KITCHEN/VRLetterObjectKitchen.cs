using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRLetterObjectKitchen : MonoBehaviour
{
    [Header("Huruf")]
    public string letter;

    [Header("Audio")]
    public AudioSource suaraHuruf;

    private XRGrabInteractable grab;
    private Vector3 startPos;
    private Quaternion startRot;

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        VRLetterSlotKitchen slot = other.GetComponent<VRLetterSlotKitchen>();

        if (slot != null)
        {
            // CEK APAKAH BENAR
            if (slot.correctLetter == letter)
            {
                // BENAR → masuk
                slot.SetLetter(letter);

                transform.position = slot.transform.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                transform.parent = slot.transform;

                grab.enabled = false;

                if (suaraHuruf != null)
                    suaraHuruf.Play();
            }
            else
            {
                // SALAH → balik
                KembaliKeAwal();
            }
        }
    }

    void KembaliKeAwal()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
}
