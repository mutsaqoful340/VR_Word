using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public MenuPanelSwitcher menuSwitcher;
    public AudioSource audioSource;

    public void StartGame()
    {
        // Play sound (tidak terpengaruh panel hide)
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        if (menuSwitcher != null)
        {
            if (menuSwitcher.mainMenu != null)
                menuSwitcher.mainMenu.gameObject.SetActive(false);

            if (menuSwitcher.settingsMenu != null)
                menuSwitcher.settingsMenu.gameObject.SetActive(false);
        }

        Debug.Log("Semua panel di-hide, game siap dimulai!");
    }
}
