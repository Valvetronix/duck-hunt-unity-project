// Desarrollado por Juan Ignacio Battelli
using UnityEngine;
using TMPro;

public class ScorePopup : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private TextMeshPro textMesh;

    [Header("Animation Settings")]
    [SerializeField] private float moveYSpeed = 0.5f;
    [SerializeField] private float disappearTime = 1f;

    private float disappearTimer;
    private Color textColor;

    public void Setup(int scoreAmount)
    {
        textMesh.SetText(scoreAmount.ToString());
        textColor = textMesh.color;
        disappearTimer = disappearTime;
    }

    private void Update()
    {
        // Movimiento constante hacia arriba para darle un poquito de juice
        // Usa un Vector3 
        transform.position += new Vector3(0, moveYSpeed * Time.deltaTime, 0);

        // (Fade Out)
        disappearTimer -= Time.deltaTime;

        if (disappearTimer < 0)
        {
            // Lo vuelvo transparente
            float fadeAmount = 3f;
            textColor.a -= fadeAmount * Time.deltaTime;
            textMesh.color = textColor;

            // si el alfa es 0, osea es invisible, lo elimino
            if (textColor.a <= 0)
            {
                // TODO: Podría hacer un pool para esto también.
                Destroy(gameObject);
            }
        }
    }
}
