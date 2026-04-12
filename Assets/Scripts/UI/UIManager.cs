// Desarrollado por Juan Ignacio Battelli
using UnityEngine;
using DuckHunt.Global;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [Header("Ducks and Ammo")]
    [SerializeField] private HitBarUI hitBar;
    [SerializeField] private AmmoUI ammoUI;

    [Header("Score and Round")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI roundText;

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

    private void UpdateRoundDisplay(int roundNumber)
    {
        // El formato "D2" rellena con ceros a la izquierda (01)
        roundText.text = roundNumber.ToString("D2");
    }

    private void UpdateScoreDisplay(int currentScore)
    {
        // El formato "D6" rellena con ceros a la izquierda (000500)
        scoreText.text = currentScore.ToString("D6");
    }

    private void OnDisable()
    {
        GameEvents.OnUIUpdateRequired -= hitBar.UpdateDuckStatus;
        GameEvents.OnAmmoChanged -= ammoUI.UpdateAmmo;
        GameEvents.OnScoreChanged -= UpdateScoreDisplay;
        GameEvents.OnRoundStarted -= UpdateRoundDisplay;
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