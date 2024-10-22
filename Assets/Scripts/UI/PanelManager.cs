using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    /*开关面板并在开启面板时暂停*/
    public GameObject settingPanel;
    public bool isOpen;

    private void Start()
    {
        PanelChoice.Num = 0;
    }

    private void Update()
    {
        //键盘监听Esc是简单开关
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelChoice.Num = 0;
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
