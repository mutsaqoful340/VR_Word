using UnityEngine;

public class KeyObject : MonoBehaviour
{
    public static bool hasKey = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            hasKey = true;
            Debug.Log("Kunci diambil");

            Destroy(gameObject); // kunci hilang setelah diambil
        }
    }
}
