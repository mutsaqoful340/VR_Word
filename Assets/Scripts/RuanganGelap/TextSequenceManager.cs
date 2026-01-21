using UnityEngine;
using System.Collections;

public class TextSequenceManager : MonoBehaviour
{
    public GameObject[] textObjects;
    public float[] displayTimes;

    void Start()
    {
        StartCoroutine(PlaySequence());
    }

    IEnumerator PlaySequence()
    {
        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].SetActive(true);
            yield return new WaitForSeconds(displayTimes[i]);
            textObjects[i].SetActive(false);
        }
    }
}
