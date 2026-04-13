// Desarrollado por Juan Ignacio Battelli
using UnityEngine;
using DuckHunt.Global;
using TMPro;
using System;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [Header("Ducks and Ammo")]
    [SerializeField] private HitBarUI hitBar;
    [SerializeField] private AmmoUI ammoUI;

    [Header("Score and Round")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI roundText;

    [Header("Score Settings")]
    [SerializeField] private float countDuration = 1f; // Cuánto tarda la animación
    private int currentDisplayedScore = 0;
    private Coroutine scoreCoroutine;

    // SINGLETON
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameEvents.OnUIUpdateRequired += hitBar.UpdateDuckStatus;
        GameEvents.OnAmmoChanged += ammoUI.UpdateAmmo;
        GameEvents.OnScoreChanged += UpdateScoreDisplay;
        GameEvents.OnRoundStarted += UpdateRoundDisplay;
    }
    private void OnDisable()
    {
        GameEvents.OnUIUpdateRequired -= hitBar.UpdateDuckStatus;
        GameEvents.OnAmmoChanged -= ammoUI.UpdateAmmo;
        GameEvents.OnScoreChanged -= UpdateScoreDisplay;
        GameEvents.OnRoundStarted -= UpdateRoundDisplay;
    }

    private void UpdateRoundDisplay(int roundNumber)
    {
        // El formato "D2" rellena con ceros a la izquierda (01)
        roundText.text = roundNumber.ToString("D2");
    }

    private void UpdateScoreDisplay(int currentScore)
    {
        // 1. Si ya había una animación corriendo, la frenamos para que no peleen
        if (scoreCoroutine != null)
        {
            StopCoroutine(scoreCoroutine);
        }

        // 2. Iniciamos la nueva animación
        scoreCoroutine = StartCoroutine(CountScoreRoutine(currentScore));
    }

    private IEnumerator CountScoreRoutine(int targetScore)
    {
        int startScore = currentDisplayedScore;
        float elapsed = 0f;

        while (elapsed < countDuration)
        {
            elapsed += Time.deltaTime;

            // Calculamos el progreso (de 0 a 1)
            float progress = elapsed / countDuration;

            // Interpolación entre el score anteriory el nuevo
            currentDisplayedScore = (int)Mathf.Lerp(startScore, targetScore, progress);

            // Actualizamos el texto con el formato que ya tenías
            scoreText.text = currentDisplayedScore.ToString("D6");

            yield return null; // Esperamos al próximo frame
        }

        // 3. Al final, nos aseguramos de que quede exactamente en el target
        currentDisplayedScore = targetScore;
        scoreText.text = currentDisplayedScore.ToString("D6");
        scoreCoroutine = null;
    }


    private void UpdateDuckIcon(int index, DuckUIStatus status)
    {
        if (hitBar != null)
        {
            hitBar.UpdateDuckStatus(index, status);
        }
    }

    private void UpdateAmmoUI(int currentAmmo)
    {
        if (ammoUI != null) ammoUI.UpdateAmmo(currentAmmo);
    }

    public void ResetHUD()
    {
        hitBar.ResetBar();
        ammoUI.ResetBullets();
    }
}