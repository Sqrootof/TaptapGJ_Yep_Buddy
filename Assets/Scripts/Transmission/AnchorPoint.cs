using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnchorPoint
{
    public GameObject anchorObject; // ������ GameObject
    public bool isUnlocked; // ����״̬
}

public class AnchorPointFill:MonoBehaviour
{
    public List<AnchorPoint> anchorPoints;

    private void Start()
    {
        if (Whole.isAnchorPoints)
        {
            anchorPoints=Whole.anchorPoints;
        }
    }
}