using UnityEngine;

public class KeyBox : MonoBehaviour
{
    public DoorController_KeyBox door;
    public Transform keySnapPoint;

    private bool keyInserted = false;

    private void OnTriggerEnter(Collider other)
    {
        if (keyInserted) return;

        if (other.CompareTag("Key"))
        {
            keyInserted = true;

            // Posisikan key ke dalam kotak
            other.transform.position = keySnapPoint.position;
            other.transform.rotation = keySnapPoint.rotation;

            // Matikan physics
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
                rb.isKinematic = true;

            // Optional: matikan collider biar ga ke-trigger ulang
            Collider col = other.GetComponent<Collider>();
            if (col != null)
                col.enabled = false;

            Debug.Log("Kunci dimasukkan ke kotak");

            // 🔑 Buka pintu dari KeyBox
            if (door != null)
                door.OpenDoorFromKeyBox();
        }
    }
}
