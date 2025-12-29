using UnityEngine;

public class NPCVisualController : MonoBehaviour
{
    public Transform player;
    public Renderer npcRenderer;

    [Header("Rotation")]
    public float rotationSpeed = 2f;

    [Header("Idle Motion")]
    public float floatSpeed = 1f;
    public float floatHeight = 0.04f;

    [Header("Glow")]
    public Color glowColor = Color.cyan;
    public float glowIntensity = 0.3f;

    private Vector3 startPos;
    private bool playerNear;

    void Start()
    {
        startPos = transform.position;
        npcRenderer.material.EnableKeyword("_EMISSION");
        npcRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    void Update()
    {
        IdleMotion();

        if (playerNear)
        {
            LookAtPlayer();
            Glow(true);
        }
        else
        {
            Glow(false);
        }
    }

    void IdleMotion()
    {
        float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }

    void LookAtPlayer()
    {
        if (!player) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * rotationSpeed
        );
    }

    void Glow(bool active)
    {
        Color target = active ? glowColor * glowIntensity : Color.black;
        npcRenderer.material.SetColor("_EmissionColor", target);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            playerNear = false;
    }
}
