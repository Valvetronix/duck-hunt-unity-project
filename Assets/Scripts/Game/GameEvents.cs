// Desarrollado por Juan Ignacio Battelli
using DuckHunt.Global;
using System;

public static class GameEvents
{
    public static Action<int, DuckUIStatus> OnUIUpdateRequired;     // Pasa el index del pato y su estado
    public static Action<int> OnAmmoChanged;                        // Pasa la cantidad de balas actual
    public static Action OnAmmoEmpty;                               // Estado munición vacía
    public static Action<int> OnScoreChanged;                       // Pasa el puntaje total
    public static Action<int> OnRoundStarted;                       // Pasa el número de ronda
}