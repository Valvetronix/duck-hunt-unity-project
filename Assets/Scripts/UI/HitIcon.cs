// Desarrollado por Juan Ignacio Battelli
using UnityEngine;
using UnityEngine.UI;
using DuckHunt.Global;

public class HitIcon : MonoBehaviour
{
    [SerializeField] private Image iconImage;

    private bool isBlinking = false;
    private float blinkTimer = 0f;
    [SerializeField] private float blinkInterval = 0.2f;

    public void SetStatus(DuckUIStatus status)
    {
        isBlinking = false;
        iconImage.enabled = true;

        switch (status)
        {
            case DuckUIStatus.Pending:
                iconImage.color = Color.white;
                break;
            case DuckUIStatus.Hit:
                iconImage.color = Color.red;
                break;
            case DuckUIStatus.Missed:
                iconImage.color = Color.white;
                break;
            case DuckUIStatus.Current:
                iconImage.color = Color.white;
                isBlinking = true;
                break;

        }
    }

    private void Update()
    {
        if (!isBlinking) return;

        blinkTimer += Time.deltaTime;
        if (blinkTimer >= blinkInterval)
        {
            Color c = iconImage.color;
            c.a = (c.a == 1f) ? 0f : 1f;
            iconImage.color = c;

            blinkTimer = 0;
        }
    }
}
