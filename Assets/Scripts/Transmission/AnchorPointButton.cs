using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorPointButton : MonoBehaviour
{
    public GameObject player;
    public GameObject MapUI;
    public GameObject Black;

    private PanelManager panelManager;

    public static string Where = "未选择";
    public Text text;
    
    private void Start()
    {
        panelManager = GetComponent<PanelManager>();
    }

    private void Update()
    {
        if (text.text != Where)
        {
            if (Whole.anchorPoints.Find(a => a.AnchorPointName == Where).isUnlocked)
            {
                text.text = "前往：" + Where;
            }
            else
            {
                text.text = "暂未解锁";
            }
        }
    }

    public void Unlocked()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AnchorManager anchorManager = player.GetComponent<AnchorManager>();
        AnchorPoint anchorPoint = Whole.FindAnchorPointByGameObject(anchorManager.currentAnchor);
        if (anchorPoint != null)
        {
            anchorPoint.isUnlocked = true;
        }
    }

    public void OpenMapUI()
    {
        MapUI.SetActive(true);
        //时间暂停
        Time.timeScale = 0;
    }

    public void ToAnchorPoint()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AnchorPoint anchorPoint=Whole.anchorPoints.Find(a => a.AnchorPointName == Where);
        if (anchorPoint != null && anchorPoint.isUnlocked)
        {
            player.transform.position=anchorPoint.anchorObject.transform.position;
            BlackScreen blackScreen = Black.GetComponent<BlackScreen>();
            blackScreen.gameObject.SetActive(true);
            blackScreen.fadeInOut = true;
            Time.timeScale = 1; // 恢复时间
            //CloseMapUI延迟1s执行
            StartCoroutine(CloseMapUICoroutine(0.32f));
        }
    }

    private IEnumerator CloseMapUICoroutine(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间
        panelManager.SettingOpen();
        //CloseMapUI(); // 关闭地图 UI
    }

    /*public void CloseMapUI()
    {
        MapUI.SetActive(false);
    }*/
}
