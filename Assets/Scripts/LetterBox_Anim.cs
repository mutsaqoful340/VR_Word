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

    [Header("Trigger Delays")]
    public float onTriggerEnterDelay = 0.1f;
    [Tooltip("Grace period before objects drop after player exits. Re-entering cancels the timer.")]
    public float exitGracePeriod = 2.0f;

    [Header("Final Door Mode")]
    [Tooltip("If checked, objects spawn immediately, don't fall, arrows are hidden, and door opens when word is correct.")]
    public bool isFinalDoor = false;
    public WorldPuzzle worldPuzzle;
    public DoorController doorController;
    [Tooltip("How often to check if word is correct (in seconds) when in Final Door mode")]
    public float wordCheckInterval = 0.5f;

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
    private Coroutine exitTimerCoroutine = null;
    private Coroutine wordCheckCoroutine = null;
    private bool doorOpened = false;

    void Start()
    {
        // Validate worldPuzzle is assigned
        if (worldPuzzle == null)
        {
            Debug.LogError("WorldPuzzle is not assigned! Please assign it in the Inspector.");
        }
        
        // If in Final Door mode, spawn everything immediately
        if (isFinalDoor)
        {
            if (doorController == null)
            {
                Debug.LogError("Final Door Mode is enabled but DoorController is not assigned! Please assign the door in the Inspector.");
            }
            
            if (worldPuzzle != null)
            {
                // Make sure the worldPuzzle is enabled and set as Instance so slots can register
                worldPuzzle.gameObject.SetActive(true);
                Debug.Log($"Final Door Mode: WorldPuzzle set active - {worldPuzzle.gameObject.name}");
            }
            
            hasSpawned = true;
            // Add a small delay to ensure WorldPuzzle.Start() runs first and Instance is set
            StartCoroutine(SpawnObjectsForFinalDoorDelayed());
        }
        // Arrow setup will happen when they spawn in SpawnArrowsWithDelay
    }
    
    IEnumerator SpawnObjectsForFinalDoorDelayed()
    {
        // Wait one frame to ensure WorldPuzzle's Start() has run
        yield return null;
        yield return StartCoroutine(SpawnObjectsForFinalDoor());
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
        // In Final Door mode, don't check arrow distances or follow pivots
        if (!isFinalDoor)
        {
            // Check arrow distances if enabled and puzzle not yet triggered
            if (arrowDistanceChecking && !puzzleTriggered)
            {
                CheckArrowDistances();
            }
            
            // Make arrows follow their pivots when not grabbed
            FollowPivotsWhenNotGrabbed();
        }
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
                    if (worldPuzzle != null && worldPuzzle.CheckPuzzle())
                    {
                        Debug.Log("Puzzle is correct! Solving...");
                        puzzleTriggered = true;
                        arrowDistanceChecking = false;
                        worldPuzzle.SolvePuzzle();
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
        // Skip trigger logic if in Final Door mode (objects already spawned)
        if (isFinalDoor) return;

        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.CompareTag("PlayerHand"))
        {
            // Cancel exit timer if player re-enters before objects drop
            if (exitTimerCoroutine != null)
            {
                StopCoroutine(exitTimerCoroutine);
                exitTimerCoroutine = null;
                Debug.Log("Player re-entered. Exit timer cancelled.");
            }
            
            // Only spawn once
            if (!hasSpawned)
            {
                hasSpawned = true;
                StartCoroutine(SpawnArrowsWithDelay(onTriggerEnterDelay));
                StartCoroutine(SpawnLSWithDelay(onTriggerEnterDelay));
                StartCoroutine(SpawnLBWithDelay(onTriggerEnterDelay));
                Debug.Log("Player entered the word container trigger.");
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Skip trigger logic if in Final Door mode
        if (isFinalDoor) return;

        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player") || other.CompareTag("PlayerHand"))
        {
            // Start grace period timer - player has time to re-enter before objects drop
            if (exitTimerCoroutine == null)
            {
                exitTimerCoroutine = StartCoroutine(ExitGracePeriodTimer());
                Debug.Log($"Player exited. Starting {exitGracePeriod}s grace period timer.");
            }
        }
    }
    
    IEnumerator ExitGracePeriodTimer()
    {
        yield return new WaitForSeconds(exitGracePeriod);
        
        // Timer completed - player didn't re-enter, so drop all objects
        Debug.Log("Grace period expired. Dropping objects.");
        StartCoroutine(EnableRagdollWithDelay(0.1f));
        hasSpawned = false;
        exitTimerCoroutine = null;
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
        
        Debug.Log($"Spawning {count} LS objects (slots) in normal mode...");
        
        for (int i = 0; i < count; i++)
        {
            if (LS_Prefabs[i] != null && LS_SpawnPoints[i] != null)
            {
                // Spawn the object
                GameObject spawnedObj = Instantiate(LS_Prefabs[i], LS_SpawnPoints[i].position, LS_SpawnPoints[i].rotation);
                spawnedLSObjects.Add(spawnedObj);
                Debug.Log($"Spawned LS object {i}: {spawnedObj.name}");

                // Manually assign the worldPuzzle to each slot
                LetterSlot slot = spawnedObj.GetComponent<LetterSlot>();
                if (slot != null && worldPuzzle != null)
                {
                    slot.SetWorldPuzzle(worldPuzzle);
                }

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
        
        Debug.Log("All LS objects spawned. Checking WorldPuzzle reference...");
        if (worldPuzzle != null)
        {
            Debug.Log($"WorldPuzzle reference found: {worldPuzzle.gameObject.name}");
        }
        else
        {
            Debug.LogError("WorldPuzzle reference is NULL! Please assign it in the Inspector. Slots won't work properly!");
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

    // Spawns all objects for Final Door mode
    IEnumerator SpawnObjectsForFinalDoor()
    {
        Debug.Log("Final Door Mode: Spawning objects immediately...");
        
        // Spawn LS objects without delay, with kinematic rigidbodies
        int lsCount = Mathf.Min(LS_Prefabs.Length, LS_SpawnPoints.Length);
        for (int i = 0; i < lsCount; i++)
        {
            if (LS_Prefabs[i] != null && LS_SpawnPoints[i] != null)
            {
                GameObject spawnedObj = Instantiate(LS_Prefabs[i], LS_SpawnPoints[i].position, LS_SpawnPoints[i].rotation);
                spawnedLSObjects.Add(spawnedObj);

                // Manually assign the worldPuzzle to each slot
                LetterSlot slot = spawnedObj.GetComponent<LetterSlot>();
                if (slot != null && worldPuzzle != null)
                {
                    slot.SetWorldPuzzle(worldPuzzle);
                }

                // Make it kinematic so it doesn't fall
                Rigidbody rb = spawnedObj.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = spawnedObj.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;
                rb.useGravity = false;

                // Enable animator
                Animator anim = spawnedObj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = true;
                }
            }
        }

        // Spawn LB objects without delay, with kinematic rigidbodies
        int lbCount = Mathf.Min(LB_Prefabs.Length, LB_SpawnPoints.Length);
        for (int i = 0; i < lbCount; i++)
        {
            if (LB_Prefabs[i] != null && LB_SpawnPoints[i] != null)
            {
                GameObject spawnedObj = Instantiate(LB_Prefabs[i], LB_SpawnPoints[i].position, LB_SpawnPoints[i].rotation);
                spawnedLBObjects.Add(spawnedObj);

                // Make it kinematic so it doesn't fall
                Rigidbody rb = spawnedObj.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb = spawnedObj.AddComponent<Rigidbody>();
                }
                rb.isKinematic = true;
                rb.useGravity = false;

                // Enable animator
                Animator anim = spawnedObj.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.enabled = true;
                }
            }
        }

        // Don't spawn arrows - they're not needed in Final Door mode
        Debug.Log("Final Door Mode: Arrows not spawned (not needed).");

        yield return null;

        // Start checking the word periodically
        if (wordCheckCoroutine == null)
        {
            wordCheckCoroutine = StartCoroutine(CheckWordPeriodically());
        }
    }

    // Continuously check if the word is correct in Final Door mode
    IEnumerator CheckWordPeriodically()
    {
        Debug.Log("Final Door Mode: Starting periodic word check...");
        
        while (!doorOpened)
        {
            yield return new WaitForSeconds(wordCheckInterval);

            // Check if puzzle is correct using the direct reference
            if (worldPuzzle != null && worldPuzzle.CheckPuzzle())
            {
                Debug.Log("Final Door Mode: Word is correct! Opening door...");
                OpenDoorDirectly();
                doorOpened = true;
                break;
            }
        }
    }

    // Open the door directly without spawning a key
    void OpenDoorDirectly()
    {
        if (doorController != null)
        {
            // Directly trigger the door animation
            if (doorController.doorAnimator != null)
            {
                doorController.doorAnimator.SetTrigger("Open");
                Debug.Log("Final Door Mode: Door opened!");
            }

            // Play door sound if available
            if (doorController.doorAudioSource != null)
            {
                doorController.doorAudioSource.Play();
            }

            // Invoke door opened events
            doorController.onDoorOpened?.Invoke();
        }
        else
        {
            Debug.LogWarning("Final Door Mode: DoorController not assigned! Cannot open door.");
        }
    }
}