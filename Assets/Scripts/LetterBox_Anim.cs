using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;
using UnityEngine.XR.Interaction.Toolkit;

public class LetterBox_Anim : MonoBehaviour
{
    [Header("LS Spawn Points")]
    public Transform[] LS_SpawnPoints;

    [Header("LS Prefabs")]
    public GameObject[] LS_Prefabs;
    
    [Header("LB Spawn Points")]
    public Transform[] LB_SpawnPoints;

    [Header("LB Prefabs")]
    public GameObject[] LB_Prefabs;

    [Header("Arrow Spawn Points (Also used as Pivots)")]
    public Transform[] Arrow_SpawnPoints;

    [Header("Arrow Prefabs")]
    public GameObject[] Arrow_Prefabs;

    [Header("Max Arrow Distance")]
    public float maxArrowDistance = 0.5f;

    [Header("Physics Settings")]
    public PhysicMaterial bouncyMaterial;
    public float despawnDelay = 1.5f;

    private List<GameObject> spawnedLSObjects = new List<GameObject>();
    private List<GameObject> spawnedLBObjects = new List<GameObject>();
    private List<GameObject> spawnedArrowObjects = new List<GameObject>();
    private Collider TriggerCollider;
    private bool hasSpawned = false;
    
    private GameObject[] ArrowObjects;
    private Dictionary<GameObject, Vector3> arrowInitialPositions = new Dictionary<GameObject, Vector3>();
    private bool arrowDistanceChecking = false;
    private bool puzzleTriggered = false;
    private Dictionary<GameObject, bool> arrowGrabbed = new Dictionary<GameObject, bool>();
    private bool arrowFollowingEnabled = true;

    void Start()
    {
        // Arrow setup will happen when they spawn in SpawnArrowsWithDelay
    }

    public void OnArrowGrabbed(GameObject arrow)
    {
        if (arrowGrabbed.ContainsKey(arrow))
        {
            arrowGrabbed[arrow] = true;
            Debug.Log($"Arrow {arrow.name} grabbed!");
        }
    }

    public void OnArrowReleased(GameObject arrow)
    {
        if (arrowGrabbed.ContainsKey(arrow))
        {
            arrowGrabbed[arrow] = false;
            Debug.Log($"Arrow {arrow.name} released!");
            
            // Only snap back if following is enabled (not in ragdoll mode)
            if (arrowFollowingEnabled)
            {
                SnapArrowToPivot(arrow);
            }
        }
    }

    void SnapArrowToPivot(GameObject arrow)
    {
        // Find the index of this arrow
        int arrowIndex = System.Array.IndexOf(ArrowObjects, arrow);
        
        if (arrowIndex >= 0 && arrowIndex < Arrow_SpawnPoints.Length && Arrow_SpawnPoints[arrowIndex] != null)
        {
            arrow.transform.position = Arrow_SpawnPoints[arrowIndex].position;
            arrow.transform.rotation = Arrow_SpawnPoints[arrowIndex].rotation;
            
            // Reset velocity if it has a Rigidbody
            Rigidbody rb = arrow.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            
            Debug.Log($"Arrow {arrow.name} snapped back to spawn point {arrowIndex}");
        }
        else
        {
            Debug.LogWarning($"No matching spawn point found for arrow {arrow.name} at index {arrowIndex}");
        }
    }

    void Update()
    {
        // Check arrow distances if enabled and puzzle not yet triggered
        if (arrowDistanceChecking && !puzzleTriggered)
        {
            CheckArrowDistances();
        }
        
        // Make arrows follow their pivots when not grabbed
        FollowPivotsWhenNotGrabbed();
    }

    void FollowPivotsWhenNotGrabbed()
    {
        if (!arrowFollowingEnabled || ArrowObjects == null || Arrow_SpawnPoints == null) return;
        
        for (int i = 0; i < ArrowObjects.Length && i < Arrow_SpawnPoints.Length; i++)
        {
            GameObject arrow = ArrowObjects[i];
            Transform pivot = Arrow_SpawnPoints[i];
            
            if (arrow != null && pivot != null && arrowGrabbed.ContainsKey(arrow) && !arrowGrabbed[arrow])
            {
                // Smoothly follow the spawn point (pivot)
                arrow.transform.position = Vector3.Lerp(arrow.transform.position, pivot.position, Time.deltaTime * 10f);
                arrow.transform.rotation = Quaternion.Lerp(arrow.transform.rotation, pivot.rotation, Time.deltaTime * 10f);
            }
        }
    }

    void CheckArrowDistances()
    {
        foreach (var kvp in arrowInitialPositions)
        {
            GameObject arrow = kvp.Key;
            Vector3 initialPos = kvp.Value;

            if (arrow != null)
            {
                float distance = Vector3.Distance(arrow.transform.position, initialPos);
                
                if (distance >= maxArrowDistance)
                {
                    Debug.Log($"Arrow {arrow.name} moved {distance} units (threshold: {maxArrowDistance}).");
                    
                    // Snap arrow back immediately
                    SnapArrowToPivot(arrow);
                    
                    // Check if puzzle is correct before solving
                    if (WorldPuzzle.Instance != null && WorldPuzzle.Instance.CheckPuzzle())
                    {
                        Debug.Log("Puzzle is correct! Solving...");
                        puzzleTriggered = true;
                        arrowDistanceChecking = false;
                        WorldPuzzle.Instance.SolvePuzzle();
                        break;
                    }
                    else
                    {
                        Debug.Log("Arrow moved but puzzle not correct yet.");
                    }
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.CompareTag("PlayerHand"))
        {
            // Only spawn once
            if (!hasSpawned)
            {
                hasSpawned = true;
                StartCoroutine(SpawnArrowsWithDelay(0.1f));
                StartCoroutine(SpawnLSWithDelay(0.1f));
                StartCoroutine(SpawnLBWithDelay(0.1f));
                Debug.Log("Player entered the word container trigger.");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.CompareTag("PlayerHand"))
        {
            StartCoroutine(EnableRagdollWithDelay(0.1f));
            hasSpawned = false;
        }
    }

    IEnumerator SpawnArrowsWithDelay(float delay)
    {
        // Clear previous spawned arrows
        spawnedArrowObjects.Clear();
        
        // Spawn each arrow prefab at matching spawn point
        int count = Mathf.Min(Arrow_Prefabs.Length, Arrow_SpawnPoints.Length);
        ArrowObjects = new GameObject[count];
        
        for (int i = 0; i < count; i++)
        {
            if (Arrow_Prefabs[i] != null && Arrow_SpawnPoints[i] != null)
            {
                // Spawn the arrow
                GameObject spawnedArrow = Instantiate(Arrow_Prefabs[i], Arrow_SpawnPoints[i].position, Arrow_SpawnPoints[i].rotation);
                ArrowObjects[i] = spawnedArrow;
                spawnedArrowObjects.Add(spawnedArrow);

                // Store initial position
                arrowInitialPositions[spawnedArrow] = spawnedArrow.transform.position;
                arrowGrabbed[spawnedArrow] = false;
                
                // Setup XR Grab Interactable listeners
                XRGrabInteractable grabInteractable = spawnedArrow.GetComponent<XRGrabInteractable>();
                if (grabInteractable != null)
                {
                    grabInteractable.selectEntered.AddListener((args) => OnArrowGrabbed(spawnedArrow));
                    grabInteractable.selectExited.AddListener((args) => OnArrowReleased(spawnedArrow));
                }
                else
                {
                    Debug.LogWarning($"Arrow {spawnedArrow.name} doesn't have XRGrabInteractable component!");
                }

                yield return new WaitForSeconds(delay);
            }
        }
        
        // Enable distance checking after all arrows are spawned
        arrowDistanceChecking = true;
    }

    IEnumerator SpawnLSWithDelay(float delay)
    {
        // Clear previous spawned objects
        spawnedLSObjects.Clear();

        // Spawn each prefab at matching spawn point
        int count = Mathf.Min(LS_Prefabs.Length, LS_SpawnPoints.Length);
        
        for (int i = 0; i < count; i++)
        {
            if (LS_Prefabs[i] != null && LS_SpawnPoints[i] != null)
            {
                // Spawn the object
                GameObject spawnedObj = Instantiate(LS_Prefabs[i], LS_SpawnPoints[i].position, LS_SpawnPoints[i].rotation);
                spawnedLSObjects.Add(spawnedObj);

                // Play animation immediately if animator exists
                Animator anim = spawnedObj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = true;
                    // Animation will play automatically based on animator controller
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator SpawnLBWithDelay(float delay)
    {
        // Clear previous spawned objects
        spawnedLBObjects.Clear();

        // Spawn each prefab at matching spawn point
        int count = Mathf.Min(LB_Prefabs.Length, LB_SpawnPoints.Length);
        
        for (int i = 0; i < count; i++)
        {
            if (LB_Prefabs[i] != null && LB_SpawnPoints[i] != null)
            {
                // Spawn the object
                GameObject spawnedObj = Instantiate(LB_Prefabs[i], LB_SpawnPoints[i].position, LB_SpawnPoints[i].rotation);
                spawnedLBObjects.Add(spawnedObj);

                // Play animation immediately if animator exists
                Animator anim = spawnedObj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = true;
                    // Animation will play automatically based on animator controller
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator EnableRagdollWithDelay(float delay)
    {
        // Disable arrow following and distance checking to prevent buggy behavior during ragdoll
        arrowFollowingEnabled = false;
        arrowDistanceChecking = false;
        
        // Combine all lists
        List<GameObject> allSpawnedObjects = new List<GameObject>();
        allSpawnedObjects.AddRange(spawnedLSObjects);
        allSpawnedObjects.AddRange(spawnedLBObjects);
        allSpawnedObjects.AddRange(spawnedArrowObjects);

        foreach (GameObject obj in allSpawnedObjects)
        {
            if (obj != null)
            {
                // Get or add Rigidbody
                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = obj.AddComponent<Rigidbody>();
                }

                // Better physics settings to prevent jittering
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                rb.drag = 0.5f; // Add air resistance
                rb.angularDrag = 2f; // Slow down rotation

                // Enable ragdoll physics
                rb.isKinematic = false;
                rb.useGravity = true;

                // Add slight random rotation while falling (reduced for stability)
                Vector3 randomTorque = new Vector3(
                    Random.Range(-0.025f, 0.025f),
                    Random.Range(-0.025f, 0.025f),
                    Random.Range(-0.025f, 0.025f)
                );
                rb.AddTorque(randomTorque, ForceMode.Impulse);

                // Disable animator so physics takes over
                Animator anim = obj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = false;
                }

                // Set all colliders to non-trigger and apply bouncy material
                Collider[] colliders = obj.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                {
                    col.isTrigger = false;
                    
                    // Apply bouncy material if assigned
                    if (bouncyMaterial != null)
                    {
                        col.material = bouncyMaterial;
                    }
                }

                // Start despawn timer for this object
                StartCoroutine(DespawnAfterDelay(obj, despawnDelay));

                yield return new WaitForSeconds(delay);
            }
        }
    }

    IEnumerator DespawnAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (obj != null)
        {
            Debug.Log($"Despawning object: {obj.name}");
            Destroy(obj);
        }
    }

    // Called when puzzle is solved
    public void OnPuzzleSolved()
    {
        Debug.Log("Puzzle solved! Making LS and LB fall...");
        
        // Disable the trigger collider to prevent re-spawning
        Collider triggerCol = GetComponent<Collider>();
        if (triggerCol != null)
        {
            triggerCol.enabled = false;
            Debug.Log("Trigger collider disabled.");
        }
        
        // Make all spawned objects fall
        StartCoroutine(EnableRagdollWithDelay(0.1f));
    }
}