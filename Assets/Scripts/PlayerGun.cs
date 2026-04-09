// Desarrollado por Juan Ignacio Battelli
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    // Eventos
    public static event Action<int> OnAmmoChanged;
    public static event Action OnAmmoEmpty;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource gunAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shotSound;

    private Camera mainCamera;
    private int maxAmmo = 3;
    private int currentAmmo;
    private bool isLocked;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        gunAudioSource.clip = shotSound;
        gunAudioSource.loop = false;
        isLocked = false;
        Reload();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocked && Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (currentAmmo > 0)
            {
                Shoot();
            }
        }
    }

    private void OnEnable()
    {
        GameManager.OnNewDuck += Reload; // Cuando cambia de pato recarga
        DuckBehaviour.OnDuckUnavailable += LockGun;
    }

    private void OnDisable()
    {
        GameManager.OnNewDuck -= Reload;
        DuckBehaviour.OnDuckUnavailable -= LockGun;
    }

    void Shoot()
    {
        currentAmmo--;
        gunAudioSource.Play();
        OnAmmoChanged?.Invoke(currentAmmo);

        Vector2 mouseScreenPosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out DuckBehaviour duck))
            {
                duck.TakeHit();
            }
        }

        if (currentAmmo <= 0)
        {
            OnAmmoEmpty?.Invoke();
        }
    }

    public void Reload()
    {
        isLocked = false;
        currentAmmo = maxAmmo;
        OnAmmoChanged?.Invoke(currentAmmo);
    }

    private void LockGun()
    {
        isLocked = true;
    }
}
