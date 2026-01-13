using UnityEngine;

public class StartGameButton : MonoBehaviour
{
    public MenuPanelSwitcher menuSwitcher; // Drag MenuPanelSwitcher dari inspector

    public void StartGame()
    {
        if (menuSwitcher != null)
        {
            // Nonaktifkan semua panel
            if (menuSwitcher.mainMenu != null)
                menuSwitcher.mainMenu.gameObject.SetActive(false);

            if (menuSwitcher.settingsMenu != null)
                menuSwitcher.settingsMenu.gameObject.SetActive(false);
        }

        // Di sini bisa tambahkan logika lain untuk mulai game
        // misal aktifkan gameplay, spawn player, dll.
        Debug.Log("Semua panel di-hide, game siap dimulai!");
    }
}