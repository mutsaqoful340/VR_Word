using UnityEngine;
using TMPro;

public class NPCProximityText : MonoBehaviour
{
    public TextMeshProUGUI textUI;
    public float showDuration = 3f;

    private bool hasShown = false;

    void Start()
    {
        if (textUI != null)
            textUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasShown) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player dekat NPC - tampilkan teks");
            textUI.text = "Halo, aku Arisa";
            textUI.gameObject.SetActive(true);

            hasShown = true;
            Invoke(nameof(HideText), showDuration);
        }
    }

    void HideText()
    {
        textUI.gameObject.SetActive(false);
    }
}
