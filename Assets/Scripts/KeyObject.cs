using UnityEngine;

public class KeyObject : MonoBehaviour
{
    public static KeyObject instance;
    public static bool hasKey = false;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            hasKey = true;
            Debug.Log("Kunci diambil");

            // kunci disembunyikan, bukan dihancurkan
            gameObject.SetActive(false);
        }
    }
}
