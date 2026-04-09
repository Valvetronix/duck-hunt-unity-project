// Desarrollado por Juan Ignacio Battelli
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewRound", menuName = "DuckHunt/RoundConfig")]
public class RoundConfiguration : ScriptableObject
{
    public int roundID;
    [Tooltip("Orden de los patos que irán saliendo (Base, Blue, Red, etc.)")]
    public List<DuckBehaviour> duckSequence;

    public float speedMultiplier = 1f;
    public float escapeTime = 8f;
}
