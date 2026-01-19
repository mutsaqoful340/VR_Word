using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class VRLetterObjectKitchen : MonoBehaviour
{
    [Header("Huruf")]
    public string letter;

    [Header("Audio")]
    public AudioSource suaraHuruf;

    [Header("Manager")]
    public VRWordManagerKitchen manager;

    private XRGrabInteractable grab;
    private Vector3 startPos;
    private Quaternion startRot;

    private bool sudahDiputar = false; // ⛔ cegah spam suara

    private void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();

        startPos = transform.position;
        startRot = transform.rotation;

        // 🔊 Event saat huruf di-grab
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    private void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrab);
        grab.selectExited.RemoveListener(OnRelease);
    }

    // ===============================
    // 🔊 SUARA SAAT PERTAMA DI-GRAB
    void OnGrab(SelectEnterEventArgs args)
    {
        if (suaraHuruf != null && !sudahDiputar)
        {
            suaraHuruf.Stop();
            suaraHuruf.Play();
            sudahDiputar = true;
        }
    }

    // 🔁 Reset supaya bisa bunyi lagi kalau dilepas & ambil ulang
    void OnRelease(SelectExitEventArgs args)
    {
        sudahDiputar = false;
    }
    // ===============================

    private void OnTriggerEnter(Collider other)
    {
        VRLetterSlotKitchen slot = other.GetComponent<VRLetterSlotKitchen>();

        if (slot != null)
        {
            if (slot.correctLetter == letter)
            {
                // ✅ BENAR
                slot.SetLetter(letter);

                transform.position = slot.transform.position;
                transform.rotation = Quaternion.Euler(0, -90, 0);
                transform.SetParent(slot.transform);

                grab.enabled = false;

                manager.CheckWord();
            }
            else
            {
                // ❌ SALAH
                KembaliKeAwal();
            }
        }
    }

    void KembaliKeAwal()
    {
        transform.SetParent(null);
        transform.position = startPos;
        transform.rotation = startRot;

        if (grab != null)
            grab.enabled = true;
    }
}
