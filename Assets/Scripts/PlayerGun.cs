using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGun : MonoBehaviour
{
    private Camera mainCamera;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource gunAudioSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip shotSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
        gunAudioSource.clip = shotSound;
        gunAudioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            gunAudioSource.Play(); // Reemplazar por SFX
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
        }
    }
}
