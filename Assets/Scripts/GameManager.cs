using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float START_TIMER = 2f;
    private const float DELAY_BETWEEN_DUCKS = 1.5f;

    [Header("Dependencies")]
    // Le paso el pool de patitos por el inspector
    [SerializeField] private DuckPool duckPool;

    // El pato que va a pedir
    [SerializeField] private DuckBehaviour duckToSpawnPrefab;

    // Puntos de Spawn
    [SerializeField] private Transform[] spawnPoints;

    // El visualizador del score
    [SerializeField] private ScorePopup scorePopupPrefab;
    [SerializeField] private Vector2 scorePopupOffset = new Vector2(0f, 0f);

    [Header("Game State")]
    public int currentScore;
    public int roundNumber;

    public int ducksPerRound = 10;
    private int ducksSpawnedThisRound = 0;


    void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private void OnEnable()
    {
        // Nos suscribimos cuando el GameManager se activa
        DuckBehaviour.OnDuckKilled += AddScore;
        DuckBehaviour.OnDuckFinished += HandleDuckFinished;
    }

    private void OnDisable()
    {
        // Nos desuscribimos cuando se desactiva/destruye
        DuckBehaviour.OnDuckKilled -= AddScore;
        DuckBehaviour.OnDuckFinished -= HandleDuckFinished;
    }

    private IEnumerator RoundRoutine()
    {
        ducksSpawnedThisRound = 0;

        yield return new WaitForSeconds(START_TIMER);

        SpawnNextDuck();
    }

    private void SpawnNextDuck()
    {
        if (ducksSpawnedThisRound < ducksPerRound)
        {
            ducksSpawnedThisRound++;

            // Lógica que ya tenías
            DuckBehaviour duck = duckPool.GetDuck(duckToSpawnPrefab);
            if (duck != null && spawnPoints.Length > 0)
            {
                int randomIndex = Random.Range(0, spawnPoints.Length);
                duck.Spawn(spawnPoints[randomIndex].position);
            }
        }
        else
        {
            // Ya salieron todos los patos de esta ronda
            Debug.Log("¡RONDA " + roundNumber + " TERMINADA!");
            roundNumber++;
            StartCoroutine(RoundRoutine()); // Iniciamos la siguiente ronda
        }
    }

    private void HandleDuckFinished(DuckBehaviour duck)
    {
        StartCoroutine(WaitAndSpawnNextDuck());
    }

    private IEnumerator WaitAndSpawnNextDuck()
    {
        yield return new WaitForSeconds(DELAY_BETWEEN_DUCKS);
        SpawnNextDuck();
    }

    public void AddScore(int points, Vector2 pos)
    {
        currentScore += points;
        Debug.Log("Score: " + currentScore);

        // Instancio el popup con el score donde murio el patito
        if (scorePopupPrefab != null)
        {
            Vector2 finalSpawnPosition = pos + scorePopupOffset;

            // Quaternion.identity significa "sin rotación"
            ScorePopup popup = Instantiate(scorePopupPrefab, finalSpawnPosition, Quaternion.identity);
            popup.Setup(points);
        }
    }
}
