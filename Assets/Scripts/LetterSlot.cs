using UnityEngine;

public class LetterSlot : MonoBehaviour
{
    public int slotIndex;          // 0=A, 1=B, 2=C
    public string currentLetter = "";
    public Transform snapPoint;

    [Header("Indicator")]
    public GameObject dotIndicator; // 🔵 objek titik "..."

    public bool isFilled => !string.IsNullOrEmpty(currentLetter);

    private WorldPuzzle assignedPuzzle = null;

    void Start()
    {
        // Register with WorldPuzzle
        if (assignedPuzzle != null)
        {
            // Use manually assigned puzzle
            assignedPuzzle.RegisterSlot(this);
            Debug.Log($"Slot {slotIndex} registered to assigned puzzle: {assignedPuzzle.gameObject.name}");
        }
        else if (WorldPuzzle.Instance != null)
        {
            WorldPuzzle.Instance.RegisterSlot(this);
        }
        else
        {
            Debug.LogWarning("WorldPuzzle not found! Make sure it exists in the scene.");
        }
        
        UpdateDotIndicator();
    }

    // Call this to manually assign which WorldPuzzle to register to
    public void SetWorldPuzzle(WorldPuzzle puzzle)
    {
        assignedPuzzle = puzzle;
        // If already started, register now
        if (gameObject.activeInHierarchy && enabled)
        {
            puzzle.RegisterSlot(this);
            Debug.Log($"Slot {slotIndex} manually registered to: {puzzle.gameObject.name}");
        }
    }

    void OnDestroy()
    {
        // Unregister when destroyed
        if (assignedPuzzle != null)
        {
            assignedPuzzle.UnregisterSlot(this);
        }
        else if (WorldPuzzle.Instance != null)
        {
            WorldPuzzle.Instance.UnregisterSlot(this);
        }
    }

    public bool PlaceLetter(string letter, GameObject letterObject)
    {
        if (isFilled)
            return false;

        // Place any letter (no correctness check)
        currentLetter = letter;
        Debug.Log($"Slot {slotIndex} received letter: '{currentLetter}'");

        letterObject.transform.SetParent(snapPoint);
        letterObject.transform.localPosition = Vector3.zero;
        letterObject.transform.localRotation = Quaternion.identity;

        UpdateDotIndicator(); // 🔥 HILANGKAN TITIK

        return true;
    }

    public void ClearSlot()
    {
        Debug.Log($"ClearSlot called on Slot {slotIndex} - was '{currentLetter}'");
        currentLetter = "";
        UpdateDotIndicator(); // 🔥 MUNCULKAN TITIK LAGI
    }

    void UpdateDotIndicator()
    {
        if (dotIndicator)
            dotIndicator.SetActive(!isFilled);
    }
}
