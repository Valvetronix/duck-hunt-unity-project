// Desarrollado por Juan Ignacio Battelli
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DuckPool : MonoBehaviour
{
    [Header("Duck Prefabs")]
    [SerializeField] private DuckBehaviour[] duckPrefabs;

    [Header("Pool Settings")]
    [SerializeField] private int initialPoolSizePerColor = 5;

    private Dictionary<DuckBehaviour, Queue<DuckBehaviour>> poolDictionary;

    void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        poolDictionary = new Dictionary<DuckBehaviour, Queue<DuckBehaviour>>();

        foreach (DuckBehaviour duckPrefab in duckPrefabs)
        {
            Queue<DuckBehaviour> duckQueue = new Queue<DuckBehaviour>();

            for (int i = 0; i < initialPoolSizePerColor; i++)
            {
                // INstancio el pato y lo hago hijo del pool
                DuckBehaviour newDuck = Instantiate(duckPrefab, this.transform);

                // Lo iniciamos apagado, todavía no se va a usar
                newDuck.gameObject.SetActive(false);

                // Lo coloco al final de la QUEUE
                duckQueue.Enqueue(newDuck);
            }

            // Guardo la queue en el diccionario usando como key su prefab
            poolDictionary.Add(duckPrefab, duckQueue);
        }
    }

    public DuckBehaviour GetDuck(DuckBehaviour requestedPrefab)
    {
        // Si no existe el prefab solicitado
        if (!poolDictionary.ContainsKey(requestedPrefab))
        {
            Debug.LogError("El prefab " + requestedPrefab.name + " no existe en el Pool.");
            return null; // Salimos de la función sin romper el juego
        }

        Queue<DuckBehaviour> duckQueue = poolDictionary[requestedPrefab];

        // Si la queue esta vacía (porque estan todos en uso, no debería ocurrir pero bueno)
        if (duckQueue.Count == 0)
        {
            Debug.LogWarning("Nos quedamos sin patos de " + requestedPrefab.name + ". Expandiendo el pool...");

            // Instanciamos uno nuevo, lo apagamos y lo metemos a la fila de emergencia
            DuckBehaviour extraDuck = Instantiate(requestedPrefab, this.transform);
            extraDuck.gameObject.SetActive(false);
            duckQueue.Enqueue(extraDuck);
        }

        DuckBehaviour duckToSpawn = duckQueue.Dequeue();

        // Le asigno a la variable originalPrefab cual es su prefab de origen (base, blue, red)
        duckToSpawn.originalPrefab = requestedPrefab;

        duckToSpawn.gameObject.SetActive(true);

        return duckToSpawn;
    }

    private void ReturnDuckToPool(DuckBehaviour duckToReturn)
    {
        // Apagamos al pato
        duckToReturn.gameObject.SetActive(false);

        // lo volvemos a guardar en su queue
        if (poolDictionary.ContainsKey(duckToReturn.originalPrefab))
        {
            poolDictionary[duckToReturn.originalPrefab].Enqueue(duckToReturn);
        }
        else
        {
            Debug.LogError("Error: Patito no registrado en el pool.");
        }
    }

    private void OnEnable()
    {
        DuckBehaviour.OnDuckFinished += ReturnDuckToPool;
    }

    private void OnDisable()
    {
        DuckBehaviour.OnDuckFinished -= ReturnDuckToPool;
    }
}
