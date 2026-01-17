using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Complete subtitle system with per-letter reveal and physics-based falling.
/// Combines PerLetterSubtitleManager + FallingLetter into one system.
/// </summary>
[RequireComponent(typeof(TMP_Text))]
public class SubtitleSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject letterPrefab;

    [Header("Appearance & Timing")]
    [SerializeField] private float perLetterRevealDelay = 0.05f;
    [SerializeField] private float fallDelayAfterReveal = 1.5f;
    [SerializeField] private float destroyDelayAfterFall = 4.0f;

    [Header("Physics")]
    [SerializeField] private float maxRotationTorque = 20f;
    [SerializeField] private float upwardLaunchForce = 3f;
    [SerializeField] private float horizontalForceRange = 2f;

    [Header("Letter Falling Settings")]
    [SerializeField] private float fadeOutDuration = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float flattenDuration = 0.3f;
    [SerializeField] private float failsafeTimeout = 5.0f;
    [SerializeField] private float groundOffset = 0.01f;

    [Header("Input References")]


    private TMP_Text layoutGuideText;
    private bool isSubtitleActive = false;

    void Awake()
    {
        Debug.Log($"SubtitleSystem.Awake() called on GameObject: {gameObject.name}");
        
        layoutGuideText = GetComponent<TMP_Text>();
        if (layoutGuideText == null)
        {
            Debug.LogError($"SubtitleSystem on GameObject '{gameObject.name}' needs a TMP_Text component on the same GameObject! Please add a TextMeshPro - Text component.");
        }
        else
        {
            layoutGuideText.color = new Color(0, 0, 0, 0);
            Debug.Log($"SubtitleSystem.Awake() on '{gameObject.name}': TMP_Text found and initialized ✓");
        }
    }

    void Update()
    {
        // Call StartSubtitle() from other scripts when needed
    }

    void LateUpdate()
    {
        if (Camera.main != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }

    public void StartSubtitle(string text, System.Action onComplete = null)
    {
        Debug.Log($"SubtitleSystem.StartSubtitle() called on '{gameObject.name}' with text: '{text}'");
        
        if (isSubtitleActive) return;
        
        // Lazy initialization if Awake hasn't run yet
        if (layoutGuideText == null)
        {
            Debug.LogWarning($"SubtitleSystem on '{gameObject.name}': layoutGuideText was null, attempting to get component now...");
            layoutGuideText = GetComponent<TMP_Text>();
            if (layoutGuideText != null)
            {
                layoutGuideText.color = new Color(0, 0, 0, 0);
                Debug.Log($"SubtitleSystem on '{gameObject.name}': Successfully initialized layoutGuideText ✓");
            }
        }
        
        // Validate required components
        if (letterPrefab == null)
        {
            Debug.LogError($"SubtitleSystem on '{gameObject.name}': letterPrefab is not assigned! Please assign it in the Inspector.");
            onComplete?.Invoke();
            return;
        }
        
        if (layoutGuideText == null)
        {
            Debug.LogError($"SubtitleSystem on '{gameObject.name}': layoutGuideText (TMP_Text) is missing even after trying to get it!");
            Debug.LogError($"GameObject '{gameObject.name}' has TMP_Text: {(GetComponent<TMP_Text>() != null ? "YES" : "NO")}");
            onComplete?.Invoke();
            return;
        }
        
        Debug.Log($"SubtitleSystem on '{gameObject.name}': All checks passed, starting coroutine ✓");
        StartCoroutine(SpawnLetters(text, onComplete));
    }

    private IEnumerator SpawnLetters(string text, System.Action onComplete = null)
    {
        isSubtitleActive = true;
        List<GameObject> spawnedLetters = new List<GameObject>();

        layoutGuideText.text = text;
        layoutGuideText.ForceMeshUpdate();
        TMP_TextInfo textInfo = layoutGuideText.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible) continue;

            yield return new WaitForSeconds(perLetterRevealDelay);

            // Get X position from character, but Y from the line's baseline for consistency
            float midX = (charInfo.vertex_BL.position.x + charInfo.vertex_BR.position.x) / 2f;
            
            // Use the line info to get consistent Y position for all chars on same line
            int lineIndex = charInfo.lineNumber;
            TMP_LineInfo lineInfo = textInfo.lineInfo[lineIndex];
            float lineY = lineInfo.baseline; // Use baseline for consistent alignment
            
            Vector3 localPos = new Vector3(midX, lineY, 0);
            Vector3 worldPos = transform.TransformPoint(localPos);

            GameObject letterInstance = Instantiate(letterPrefab, worldPos, this.transform.rotation);
            letterInstance.transform.SetParent(this.transform);

            Rigidbody rb = letterInstance.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            TMP_Text tmpText = letterInstance.GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = charInfo.character.ToString();
            }

            // Add the FallingLetter component with all settings
            FallingLetter fallingLetter = letterInstance.AddComponent<FallingLetter>();
            fallingLetter.Initialize(fadeOutDuration, groundLayer, flattenDuration, failsafeTimeout, groundOffset);

            spawnedLetters.Add(letterInstance);
        }

        yield return new WaitForSeconds(fallDelayAfterReveal);

        foreach (GameObject letter in spawnedLetters)
        {
            if (letter != null)
            {
                Vector3 worldPos = letter.transform.position;
                Quaternion worldRot = letter.transform.rotation;

                letter.transform.SetParent(null);
                letter.transform.position = worldPos;
                letter.transform.rotation = worldRot;

                Rigidbody rb = letter.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.mass = 0.5f;
                    
                    // Apply upward explosion force with random horizontal direction
                    Vector3 explosionForce = new Vector3(
                        Random.Range(-horizontalForceRange, horizontalForceRange),
                        upwardLaunchForce,
                        Random.Range(-horizontalForceRange, horizontalForceRange)
                    );
                    rb.velocity = explosionForce;
                    rb.angularVelocity = Vector3.zero;

                    Vector3 randomTorque = new Vector3(
                        Random.Range(-maxRotationTorque, maxRotationTorque),
                        Random.Range(-maxRotationTorque, maxRotationTorque),
                        Random.Range(-maxRotationTorque, maxRotationTorque)
                    );
                    rb.AddTorque(randomTorque, ForceMode.Impulse);
                }

                FallingLetter letterScript = letter.GetComponent<FallingLetter>();
                if (letterScript != null)
                {
                    letterScript.StartLifecycle(0f, destroyDelayAfterFall, maxRotationTorque);
                }
            }
        }

        yield return new WaitForSeconds(destroyDelayAfterFall + 1.0f);
        isSubtitleActive = false;
        onComplete?.Invoke();
    }

    // ===== NESTED CLASS: FallingLetter =====
    
    [RequireComponent(typeof(TMP_Text))]
    public class FallingLetter : MonoBehaviour
    {
        private float fadeOutDuration;
        private LayerMask groundLayer;
        private float flattenDuration;
        private float failsafeTimeout;
        private float groundOffset;

        private TMP_Text tmpText;
        private bool isFalling = false;
        private float maxTorque;
        private float destroyDelay;
        private float fallDelay;

        public void Initialize(float fadeOut, LayerMask ground, float flatten, float failsafe, float offset)
        {
            this.fadeOutDuration = fadeOut;
            this.groundLayer = ground;
            this.flattenDuration = flatten;
            this.failsafeTimeout = failsafe;
            this.groundOffset = offset;
            
            tmpText = GetComponent<TMP_Text>();
        }

        public void StartLifecycle(float fallDelay, float destroyDelay, float torque)
        {
            this.maxTorque = torque;
            this.destroyDelay = destroyDelay;
            this.fallDelay = fallDelay;

            StartCoroutine(FallAfterDelay());
        }

        private IEnumerator FallAfterDelay()
        {
            yield return new WaitForSeconds(this.fallDelay);
            DropLetter();
            StartCoroutine(DestroyAfterTimeout());
        }

        private void DropLetter()
        {
            isFalling = true;
            gameObject.AddComponent<BoxCollider>();
        }

        void OnCollisionEnter(Collision collision)
        {
            int collisionLayer = 1 << collision.gameObject.layer;
            if (isFalling && (collisionLayer & groundLayer.value) != 0)
            {
                isFalling = false;
                Vector3 impactPoint = collision.contacts[0].point;
                StartCoroutine(LandAndFade(impactPoint.y));
            }
        }

        private IEnumerator LandAndFade(float groundY)
        {
            BoxCollider bc = GetComponent<BoxCollider>();
            float halfHeight = (bc != null) ? bc.size.y / 2.0f : 0.05f;

            if (GetComponent<Rigidbody>()) Destroy(GetComponent<Rigidbody>());
            if (bc != null) Destroy(bc);

            Quaternion startRot = transform.rotation;
            Vector3 startPos = transform.position;

            float randomY = Random.Range(0f, 360f);
            Quaternion endRot = Quaternion.Euler(90, randomY, 0);
            Vector3 endPos = new Vector3(startPos.x, groundY + halfHeight + groundOffset, startPos.z);

            float timer = 0;
            while (timer < flattenDuration)
            {
                timer += Time.deltaTime;
                float t = timer / flattenDuration;
                float smoothT = t * t * (3f - 2f * t);

                transform.rotation = Quaternion.Slerp(startRot, endRot, smoothT);
                transform.position = Vector3.Lerp(startPos, endPos, smoothT);
                yield return null;
            }

            transform.rotation = endRot;
            transform.position = endPos;

            float stayOnGroundTime = this.destroyDelay - fadeOutDuration;
            if (stayOnGroundTime > 0)
            {
                yield return new WaitForSeconds(stayOnGroundTime);
            }

            yield return StartCoroutine(FadeOut());
            Destroy(gameObject);
        }

        private IEnumerator DestroyAfterTimeout()
        {
            yield return new WaitForSeconds(this.fallDelay + this.destroyDelay + failsafeTimeout);
            if (isFalling)
            {
                yield return StartCoroutine(FadeOut());
                Destroy(gameObject);
            }
        }

        private IEnumerator FadeOut()
        {
            if (GetComponent<Rigidbody>()) Destroy(GetComponent<Rigidbody>());
            if (GetComponent<BoxCollider>()) Destroy(GetComponent<BoxCollider>());

            Color originalColor = tmpText.color;
            Color targetColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            float timer = 0;

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                tmpText.color = Color.Lerp(originalColor, targetColor, timer / fadeOutDuration);
                yield return null;
            }
        }
    }
}
