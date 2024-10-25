using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorManager : MonoBehaviour
{
    public List<AnchorPoint> anchorPoints; // 所有锚点
    public GameObject unlockButton; // 解锁按钮
    public GameObject interactButton; // 接触按钮
    public GameObject currentAnchor; // 当前接触的锚点

    private void Start()
    {
        //anchorPoints = Whole.anchorPoints;
        for (int i = 0; i < anchorPoints.Count && i < 6; i++) // 确保不超过6个
        {
            anchorPoints[i].isUnlocked = SaveManager.NowSD.Anchor[i];
        }
        Whole.anchorPoints=anchorPoints;
    }

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
            if (distance < 2f) // 设定接触距离
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
        interactButton.SetActive(false);
    }

    private void ShowInteractButton()
    {
        unlockButton.SetActive(false);
        interactButton.SetActive(true);
    }

    private void HideAllButtons()
    {
        unlockButton.SetActive(false);
        interactButton.SetActive(false);
    }
}
