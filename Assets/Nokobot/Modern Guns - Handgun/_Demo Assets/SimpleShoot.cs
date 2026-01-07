using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")]
    [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")]
    [SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")]
    [SerializeField] private float ejectPower = 150f;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Klik mouse kiri untuk menembak
        if (Input.GetMouseButtonDown(0))
        {
            StartShoot();
        }
    }

    public void StartShoot()
    {
        if (gunAnimator != null)
            gunAnimator.SetTrigger("Fire");
    }

    // Dipanggil oleh Animation Event
    void Shoot()
    {
        // Play sound
        if (audioSource && shootSound)
            audioSource.PlayOneShot(shootSound);

        // Muzzle flash
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(
                muzzleFlashPrefab,
                barrelLocation.position,
                barrelLocation.rotation
            );
            Destroy(tempFlash, destroyTimer);
        }

        // Bullet
        if (!bulletPrefab) return;

        Rigidbody rb = Instantiate(
            bulletPrefab,
            barrelLocation.position,
            barrelLocation.rotation
        ).GetComponent<Rigidbody>();

        rb.AddForce(barrelLocation.forward * shotPower);
    }

    // Dipanggil oleh Animation Event
    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab) return;

        GameObject tempCasing = Instantiate(
            casingPrefab,
            casingExitLocation.position,
            casingExitLocation.rotation
        );

        Rigidbody rb = tempCasing.GetComponent<Rigidbody>();

        rb.AddExplosionForce(
            Random.Range(ejectPower * 0.7f, ejectPower),
            casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f,
            1f
        );

        rb.AddTorque(
            new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)),
            ForceMode.Impulse
        );

        Destroy(tempCasing, destroyTimer);
    }
}
