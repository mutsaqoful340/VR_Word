using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TargetAnswer[] allTargets;

    private void Awake()
    {
        Instance = this;
    }

    public void CheckAnswer(TargetAnswer target)
    {
        if (target.isCorrect)
        {
            Correct(target);
        }
        else
        {
            Wrong(target);
        }
    }

    void Correct(TargetAnswer target)
    {
        // hentikan gerak
        var move = target.GetComponent<SideMoveLoop>();
        if (move != null) move.enabled = false;

        // perbesar pelan
        target.transform.localScale *= 1.2f;

        // ubah warna lembut
        target.GetComponent<Renderer>().material.color = new Color(0.6f, 1f, 0.6f);

        // sembunyikan target lain
        foreach (var t in allTargets)
        {
            if (t != target)
            {
                t.gameObject.SetActive(false);
            }
        }

        Debug.Log("BENAR");
    }

    void Wrong(TargetAnswer target)
    {
        // redupkan tanpa menghukum
        var r = target.GetComponent<Renderer>();
        Color c = r.material.color;
        r.material.color = new Color(c.r, c.g, c.b, 0.3f);

        Debug.Log("SALAH");
    }
}
