using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelChoice : MonoBehaviour
{
    public GameObject[] ui;
    public GameObject[] buttons;

    public static int Num;

    private void Start()
    {
        Num = 0;
    }

    private void OnEnable()
    {
        AltButton(Num);
        AltUI(Num);
    }

    public void AltButton(int a)
    {
        if (buttons.Length > 0)
        {
            foreach (var t in buttons)
            {
                t.GetComponent<Image>().color = Color.white;
            }
            buttons[a].GetComponent<Image>().color = Color.yellow;
        }
    }
    
    public void AltUI(int a)
    {
        if (ui.Length > 0)
        {
            foreach (var t in ui)
            {
                t.SetActive(false);
            }
            ui[a].SetActive(true);
        }
    }
}
