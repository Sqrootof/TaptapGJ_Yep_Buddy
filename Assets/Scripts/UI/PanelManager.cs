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
            PanelChoice.Num = 0; panelChoice.AltButton(PanelChoice.Num); panelChoice.AltUI(PanelChoice.Num);
            SettingOpen();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            PanelChoice.Num = 7; panelChoice.AltButton(PanelChoice.Num); panelChoice.AltUI(PanelChoice.Num);
            SettingOpen();
        }
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
