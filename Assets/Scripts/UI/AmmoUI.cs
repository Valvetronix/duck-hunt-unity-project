// Desarrollado por Juan Ignacio Battelli
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{

    [SerializeField] private List<Image> bulletIcons;

    private int MAX_AMMO = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void UpdateAmmo(int currentAmmo)
    {
        for (int i = 0; i < bulletIcons.Count; i++)
        {
            Color c = bulletIcons[i].color;
            // Si el índice es menor a las balas que tenemos, es visible (1), si no, invisible (0)
            c.a = (i < currentAmmo) ? 1f : 0f;
            bulletIcons[i].color = c;
        }
    }

    public void ResetBullets()
    {
        UpdateAmmo(MAX_AMMO);
    }
}
