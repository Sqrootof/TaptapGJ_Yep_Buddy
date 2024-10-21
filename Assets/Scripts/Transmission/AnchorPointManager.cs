using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnchorManager : MonoBehaviour
{
    public List<AnchorPoint> anchorPoints; // ����ê��
    public GameObject unlockButton; // ������ť
    public GameObject interactButton; // �Ӵ���ť
    private GameObject currentAnchor; // ��ǰ�Ӵ���ê��

    private void Update()
    {
        // �����ê��ĽӴ�
        CheckAnchorProximity();
    }

    private void CheckAnchorProximity()
    {
        bool flag=false;
        foreach (var anchor in anchorPoints)
        {
            float distance = Vector3.Distance(transform.position, anchor.anchorObject.transform.position);
            if (distance < 3f) // �趨�Ӵ�����
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
