// Desarrollado por Juan Ignacio Battelli
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DuckAudioHandler : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource movementSource;
    [SerializeField] private AudioSource vocalSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip flapSound;
    [SerializeField] private AudioClip fallingSound;
    [SerializeField] private AudioClip quackSound;

    [Header("Quack Settings")]
    [SerializeField] private float minQuackDelay = 2f;
    [SerializeField] private float maxQuackDelay = 6f;

    private Coroutine quackRoutine;

    public void StartFlyingAudio()
    {
        movementSource.clip = flapSound;
        movementSource.loop = true;
        movementSource.Play();

        if (quackRoutine == null)
        {
            quackRoutine = StartCoroutine(QuackRoutine());
        }
    }

    public void StartFallingAudio()
    {
        movementSource.clip = fallingSound;
        movementSource.loop = false;
        movementSource.Play();
    }

    public void StopAudio()
    {
        movementSource.Stop();

        if (quackRoutine != null)
        {
            StopCoroutine(quackRoutine);
            quackRoutine = null;
        }
    }

    private IEnumerator QuackRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minQuackDelay, maxQuackDelay);
            yield return new WaitForSeconds(waitTime);

            PlayQuack();
        }
    }

    private void PlayQuack()
    {
        vocalSource.pitch = Random.Range(0.9f, 1.1f);
        vocalSource.PlayOneShot(quackSound);
    }
}
