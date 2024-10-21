using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnchorPoint
{
    public GameObject anchorObject; // 关联的 GameObject
    public bool isUnlocked; // 解锁状态
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