using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    /*开关面板并在开启面板时暂停*/
    public GameObject settingPanel;
    public bool isOpen;
    public PanelChoice panelChoice;
    [SerializeField] GameObject WeaponPanel_Pre;
                     GameObject WeaponPanel;
    [SerializeField] GameObject BackpackPanel_Pre;
                     GameObject BackpackPanel;

    private void Start()
    {
        PanelChoice.Num = 0;
        panelChoice = GameObject.Find("PanelManager").GetComponent<PanelChoice>();
    }

    private void Update()
    {
        //键盘监听Esc是简单开关
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelChoice.Num = 0;
            KeyCodeOpen();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            PanelChoice.Num = 1;
            KeyCode1();
        }
        /*if (Input.GetKeyDown(KeyCode.J))
        {
            PanelChoice.Num = 2;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PanelChoice.Num = 3;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PanelChoice.Num = 4;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PanelChoice.Num = 5;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            PanelChoice.Num = 6;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PanelChoice.Num = 7;
            KeyCode1();
        }*/
        if (Input.GetKeyDown(KeyCode.B))
        {
            PanelChoice.Num = 3;
            KeyCode1();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            PanelChoice.Num = 2;
            KeyCode1();
        }
    }

    private void KeyCodeOpen()
    {
        panelChoice.AltButton(PanelChoice.Num);
        panelChoice.AltUI(PanelChoice.Num);
        SettingOpen();
    }
    private void KeyCodeAlt()
    {
        panelChoice.AltButton(PanelChoice.Num);
        panelChoice.AltUI(PanelChoice.Num);
    }
    private void KeyCode1()
    {
        if (isOpen)
            KeyCodeAlt();
        else
            KeyCodeOpen();
    }

    public void SettingOpen()
    {
        if (settingPanel)
        {
            SoundsManager.PanelClip();
            if (!isOpen)
            {
                Time.timeScale = 0;
                settingPanel.SetActive(true);
                isOpen = true;
            }
            else
            {
                Time.timeScale = 1;
                settingPanel.SetActive(false);
                isOpen = false;
            }
        }
    }
}
