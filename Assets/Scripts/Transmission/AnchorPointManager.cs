using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorManager : MonoBehaviour
{
    public List<AnchorPoint> anchorPoints; // 所有锚点
    public GameObject unlockButton; // 解锁按钮
    public GameObject interactButton; // 接触按钮
    private GameObject currentAnchor; // 当前接触的锚点

    private void Update()
    {
        // 检测与锚点的接触
        CheckAnchorProximity();
    }

    private void CheckAnchorProximity()
    {
        bool flag=false;
        foreach (var anchor in anchorPoints)
        {
            float distance = Vector3.Distance(transform.position, anchor.anchorObject.transform.position);
            if (distance < 3f) // 设定接触距离
            {
                flag = true;
                currentAnchor = anchor.anchorObject;

                if (!anchor.isUnlocked)
                {
                    ShowUnlockButton();
                }
                else
                {
                    ShowInteractButton();
                }
                return;
            }
        }
        if (!flag)
        {
            HideAllButtons();
        }

    }

    private void ShowUnlockButton()
    {
        unlockButton.SetActive(true);
    }

    private void ShowInteractButton()
    {
        interactButton.SetActive(true);
    }

    private void HideAllButtons()
    {
        unlockButton.SetActive(false);
        interactButton.SetActive(false);
    }
}
