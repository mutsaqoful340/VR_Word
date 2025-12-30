using UnityEngine;

public class NPCDialogTrigger : MonoBehaviour
{
    public NPCDialogController dialogController;

    public void OnQuestionButtonPressed()
    {
        if (dialogController != null)
        {
            Debug.Log("Tombol ? ditekan - buka dialog NPC");
            dialogController.OpenDialog();
        }
    }
}
