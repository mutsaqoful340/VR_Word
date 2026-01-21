using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangerWithDelay : MonoBehaviour
{
    public string sceneName;
    public float delay;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ChangeSceneAfterDelay(sceneName, delay));
        }
    }

    public void ChangeSceneWithDelay(string sceneName, float delay)
    {
        StartCoroutine(ChangeSceneAfterDelay(sceneName, delay));
    }

    private IEnumerator ChangeSceneAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
