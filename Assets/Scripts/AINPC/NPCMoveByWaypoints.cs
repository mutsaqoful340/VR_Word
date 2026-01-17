using UnityEngine;

public class NPCMoveByWaypoints : MonoBehaviour
{
    [Header("Waypoint Sets")]
    public Transform[] stage1ToStage2;
    public Transform[] stage2ToStage3;
    // 🔹 TAMBAHAN STAGE 4 (AMAN)
    public Transform[] stage3ToStage4;
    public Transform[] stage4ToStage5;
    public Transform[] stage5ToStage6;
    public Transform[] stage6ToStage7;
    public Transform[] stage7ToStage8;
    public Transform[] stage8ToStage9;
    public Transform[] stage9ToStage10;
    public Transform[] stage10ToStage11;
    public Transform[] stage11ToStage12;

    [Header("Movement")]
    public float speed = 1.5f;
    public float rotateSpeed = 6f;
    public Animator animator;

    [Header("Dialog Ref")]
    public NPCDialogController dialogController;

    Transform[] currentWaypoints;
    int currentIndex;
    public bool isMoving;

    void Update()
    {
        if (!isMoving || currentWaypoints == null || currentWaypoints.Length == 0)
            return;

        Transform target = currentWaypoints[currentIndex];
        Vector3 dir = target.position - transform.position;
        dir.y = 0f;

        if (dir.magnitude < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= currentWaypoints.Length)
            {
                StopMoving();
            }
            return;
        }

        if (dir != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRot,
                rotateSpeed * 100f * Time.deltaTime
            );
        }

        transform.position += transform.forward * speed * Time.deltaTime;
    }

    // ================== MOVE API (LAMA) ==================

    public void MoveStage1ToStage2()
    {
        if (dialogController.currentStage != DialogStage.Stage1)
            return;

        currentWaypoints = stage1ToStage2;
        StartMove();
    }

    public void MoveStage2ToStage3()
    {
        if (dialogController.currentStage != DialogStage.Stage2)
            return;

        currentWaypoints = stage2ToStage3;
        StartMove();
    }

    // ================== MOVE API (BARU, OPTIONAL) ==================

    // 🔹 DIPANGGIL SAAT PINTU STAGE 4 TERBUKA
    public void MoveStage3ToStage4()
    {
        if (dialogController.currentStage != DialogStage.Stage3)
            return;

        if (stage3ToStage4 == null || stage3ToStage4.Length == 0)
            return;

        currentWaypoints = stage3ToStage4;
        StartMove();
    }

    public void MoveStage4ToStage5()
    {
        if (dialogController.currentStage != DialogStage.Stage4)
            return;

        if (stage4ToStage5 == null || stage4ToStage5.Length == 0)
            return;

        currentWaypoints = stage4ToStage5;
        StartMove();
    }

    public void MoveStage5ToStage6()
    {
        if (dialogController.currentStage != DialogStage.Stage5)
            return;

        if (stage5ToStage6 == null || stage5ToStage6.Length == 0)
            return;

        currentWaypoints = stage5ToStage6;
        StartMove();
    }

    public void MoveStage6ToStage7()
    {
        if (dialogController.currentStage != DialogStage.Stage6)
            return;

        if (stage6ToStage7 == null || stage6ToStage7.Length == 0)
            return;

        currentWaypoints = stage6ToStage7;
        StartMove();
    }

    public void MoveStage7ToStage8()
    {
        if (dialogController.currentStage != DialogStage.Stage7)
            return;

        if (stage7ToStage8 == null || stage7ToStage8.Length == 0)
            return;

        currentWaypoints = stage7ToStage8;
        StartMove();
    }

    public void MoveStage8ToStage9()
    {
        if (dialogController.currentStage != DialogStage.Stage8)
            return;

        if (stage8ToStage9 == null || stage8ToStage9.Length == 0)
            return;

        currentWaypoints = stage8ToStage9;
        StartMove();
    }

    public void MoveStage9ToStage10()
    {
        if (dialogController.currentStage != DialogStage.Stage9)
            return;

        if (stage9ToStage10 == null || stage9ToStage10.Length == 0)
            return;

        currentWaypoints = stage9ToStage10;
        StartMove();
    }

    public void MoveStage10ToStage11()
    {
        if (dialogController.currentStage != DialogStage.Stage10)
            return;

        if (stage10ToStage11 == null || stage10ToStage11.Length == 0)
            return;

        currentWaypoints = stage10ToStage11;
        StartMove();
    }

    public void MoveStage11ToStage12()
    {
        if (dialogController.currentStage != DialogStage.Stage11)
            return;

        if (stage11ToStage12 == null || stage11ToStage12.Length == 0)
            return;

        currentWaypoints = stage11ToStage12;
        StartMove();
    }


    void StartMove()
    {
        currentIndex = 0;
        isMoving = true;

        if (animator != null)
            animator.SetBool("isWalking", true);
    }

    void StopMoving()
    {
        isMoving = false;

        if (animator != null)
            animator.SetBool("isWalking", false);

        AdvanceStage();
    }

    // ================== STAGE CONTROL (EXTEND SAJA) ==================

    void AdvanceStage()
    {
        if (dialogController == null)
            return;

        if (dialogController.currentStage == DialogStage.Stage1)
        {
            dialogController.SetStage(DialogStage.Stage2);
        }
        else if (dialogController.currentStage == DialogStage.Stage2)
        {
            dialogController.SetStage(DialogStage.Stage3);
        }
        else if (dialogController.currentStage == DialogStage.Stage3)
        {
            dialogController.SetStage(DialogStage.Stage4);
        }
        else if (dialogController.currentStage == DialogStage.Stage4)
        {
            dialogController.SetStage(DialogStage.Stage5);
        }
        else if (dialogController.currentStage == DialogStage.Stage5)
        {
            dialogController.SetStage(DialogStage.Stage6);
        }
        else if (dialogController.currentStage == DialogStage.Stage6)
        {
            dialogController.SetStage(DialogStage.Stage7);
        }
        else if (dialogController.currentStage == DialogStage.Stage7)
        {
            dialogController.SetStage(DialogStage.Stage8);
        }
        else if (dialogController.currentStage == DialogStage.Stage8)
        {
            dialogController.SetStage(DialogStage.Stage9);
        }
        else if (dialogController.currentStage == DialogStage.Stage9)
        {
            dialogController.SetStage(DialogStage.Stage10);
        }
        else if (dialogController.currentStage == DialogStage.Stage10)
        {
            dialogController.SetStage(DialogStage.Stage11);
        }
        else if (dialogController.currentStage == DialogStage.Stage11)
        {
            dialogController.SetStage(DialogStage.Stage12);
        }

        Debug.Log("NPC berhenti & lanjut ke stage berikutnya");
    }

}
