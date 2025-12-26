using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TargetAnswer : MonoBehaviour
{
    public bool isCorrect;

    public void OnHit()
    {
        if (isCorrect)
        {
            GetComponent<Renderer>().material.color = Color.green;
            Debug.Log("BENAR");
        }
        else
        {
            GetComponent<Renderer>().material.color = Color.red;
            Debug.Log("SALAH");
        }
    }
}
