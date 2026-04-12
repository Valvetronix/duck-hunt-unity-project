// Desarrollado por Juan Ignacio Battelli
namespace DuckHunt.Global
{
    public enum DuckUIStatus
    {
        Pending,   // Blanco (no procesado)
        Current,   // Titilando (el pato que está en pantalla ahora)
        Hit,       // Rojo (cazado)
        Missed     // Blanco pero escapó
    }
}
