using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetAnswer : MonoBehaviour
{
    public bool isCorrect;

    Renderer rend;
    Color originalColor;

    void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    // ===== HOVER (LASER KENA, BELUM KLIK) =====
    public void OnHoverEnter()
    {
        // highlight lembut (bukan kedip keras)
        rend.material.color = originalColor * 1.2f;
    }

    public void OnHoverExit()
    {
        // balik normal
        rend.material.color = originalColor;
    }

    // ===== SELECT (TRIGGER DITEKAN) =====
    public void OnSelect()
    {
        Debug.Log("ONSELECT DIPANGGIL OLEH: " + gameObject.name +
                  " | isCorrect = " + isCorrect);

        GameManager.Instance.CheckAnswer(this);
    }
}
