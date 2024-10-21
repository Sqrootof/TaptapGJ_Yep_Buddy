using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPointButton : MonoBehaviour
{
    public GameObject player;
    public GameObject MapUI;
    public GameObject Black;

    public void Unlocked()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        AnchorManager anchorManager = player.GetComponent<AnchorManager>();
        AnchorPoint anchorPoint = Whole.FindAnchorPointByGameObject(anchorManager.currentAnchor);
        if (anchorPoint != null)
        {
            anchorPoint.isUnlocked = true;
        }
        else
        {

            Debug.Log("NULL");
        }
    }

    public void OpenMapUI()
    {
        MapUI.SetActive(true);
        //时间暂停
        Time.timeScale = 0;
    }

    public void ToAnchorPoint(int p)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = Whole.anchorPoints[p].anchorObject.transform.position;
        BlackScreen blackScreen = Black.GetComponent<BlackScreen>();
        blackScreen.gameObject.SetActive(true);
        blackScreen.fadeInOut = true;
        Time.timeScale = 1; // 恢复时间
        //CloseMapUI延迟1s执行
        StartCoroutine(CloseMapUICoroutine(0.5f));

    }

    private IEnumerator CloseMapUICoroutine(float delay)
    {
        yield return new WaitForSeconds(delay); // 等待指定的延迟时间
        CloseMapUI(); // 关闭地图 UI
    }

    public void CloseMapUI()
    {
        MapUI.SetActive(false);
    }
}
