// Desarrollado por Juan Ignacio Battelli
using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private const float START_TIMER = 2f;
    private const float DELAY_BETWEEN_DUCKS = 1.5f;

    // Eventos
    public static event Action OnNewDuck;

    [Header("Dependencies")]
    // Le paso el pool de patitos por el inspector
    [SerializeField] private DuckPool duckPool;
    [SerializeField] private RoundConfiguration[] allRounds;

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

    private int currentRoundIndex = 0;
    private int duckInRoundIndex = 0;

    void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private void OnEnable()
    {
        // suscribimos cuando el GameManager se activa
        DuckBehaviour.OnDuckKilled += AddScore;
        DuckBehaviour.OnDuckFall += HandleDuckFall;
        DuckBehaviour.OnDuckEscaped += HandleDuckEscaped;
    }

    private void OnDisable()
    {
        // desuscribimos cuando se desactiva/destruye
        DuckBehaviour.OnDuckKilled -= AddScore;
        DuckBehaviour.OnDuckFall -= HandleDuckFall;
        DuckBehaviour.OnDuckEscaped -= HandleDuckEscaped;
    }


    private IEnumerator RoundRoutine()
    {
        // Inicio / Reseteo el indice de patos a 0
        duckInRoundIndex = 0;
        yield return new WaitForSeconds(START_TIMER);
        Debug.Log("Inicio Ronda: " + (currentRoundIndex + 1));
        SpawnNextDuck();
    }

    private void SpawnNextDuck()
    {
        RoundConfiguration config = allRounds[currentRoundIndex];

        if (duckInRoundIndex < config.duckSequence.Count)
        {
            // el prefab que toca segun la lista que le pase con el ScriptableObject
            DuckBehaviour prefabToRequest = config.duckSequence[duckInRoundIndex];
            duckInRoundIndex++;

            DuckBehaviour duck = duckPool.GetDuck(prefabToRequest);

            if (duck != null)
            {
                // Aplicamos settings de la ronda si queremos (opcional)
                duck.speed *= config.speedMultiplier;
                duck.timeToEscape = config.escapeTime;
                int randomIndex = UnityEngine.Random.Range(0, spawnPoints.Length);
                duck.Spawn(spawnPoints[randomIndex].position);
                OnNewDuck?.Invoke();
            }
        }
        else
        {
            // Fin de la secuencia de ronda
            Debug.Log("Fin Ronda: " + (currentRoundIndex + 1));
            currentRoundIndex++;
            if (currentRoundIndex < allRounds.Length)
            {
                StartCoroutine(RoundRoutine());
            }
            else
            {
                Debug.Log("FIN DEL JUEGO!");
            }
        }
    }

    private void HandleDuckEscaped(DuckBehaviour behaviour)
    {
        StartCoroutine(WaitAndSpawnNextDuck());
    }

    private void HandleDuckFall(DuckBehaviour duck)
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
