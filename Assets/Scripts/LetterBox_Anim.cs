using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.PlayerLoop;

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


    [Header("Physics Settings")]
    public PhysicMaterial bouncyMaterial;
    public float despawnDelay = 1.5f;

    private List<GameObject> spawnedLSObjects = new List<GameObject>();
    private List<GameObject> spawnedLBObjects = new List<GameObject>();
    private Collider TriggerCollider;

    public void OnTriggerEnter(Collider other)
    {
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(SpawnLSWithDelay(0.1f));
            StartCoroutine(SpawnLBWithDelay(0.1f));
            Debug.Log("Player entered the word container trigger.");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // Check if it's the player or a child of the player
        if (other.CompareTag("Player") || other.transform.root.CompareTag("Player"))
        {
            StartCoroutine(EnableRagdollWithDelay(0.1f));
        }
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
        // Combine both lists
        List<GameObject> allSpawnedObjects = new List<GameObject>();
        allSpawnedObjects.AddRange(spawnedLSObjects);
        allSpawnedObjects.AddRange(spawnedLBObjects);

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
                    Random.Range(-0.3f, 0.3f),
                    Random.Range(-0.3f, 0.3f),
                    Random.Range(-0.3f, 0.3f)
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