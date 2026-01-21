using UnityEngine;
using System.Collections;

public class CreditOneByOne : MonoBehaviour
{
    public GameObject[] credits;
    public float displayTime = 3f;

    void OnEnable()
    {
        StartCoroutine(PlayCredits());
    }

    IEnumerator PlayCredits()
    {
        for (int i = 0; i < credits.Length; i++)
        {
            credits[i].SetActive(true);
            yield return new WaitForSeconds(displayTime);
            credits[i].SetActive(false);
        }
    }
}
