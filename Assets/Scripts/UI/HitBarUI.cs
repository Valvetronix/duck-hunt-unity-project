// Desarrollado por Juan Ignacio Battelli
using DuckHunt.Global;
using System.Collections.Generic;
using UnityEngine;

public class HitBarUI : MonoBehaviour
{
    [SerializeField] private List<HitIcon> icons = new List<HitIcon>();

    public void UpdateDuckStatus(int index, DuckUIStatus status)
    {
        if (index >= 0 && index < icons.Count)
        {
            icons[index].SetStatus(status);
        }
    }

    public void ResetBar()
    {
        foreach (var icon in icons)
        {
            icon.SetStatus(DuckUIStatus.Pending);
        }
    }
}
