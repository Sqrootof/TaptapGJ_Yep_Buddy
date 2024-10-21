using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingTask : MonoBehaviour
{
    public GameObject anchorPoint;
    public GameObject player;
    private LineRenderer lineRenderer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //激光,连接玩家和锚点
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Whole.theQuest!= null && Whole.theQuest.trackingCoordinates[Whole.theQuest.Process] != new Vector3(0, 0, 0))
        {
            anchorPoint.SetActive(true);
            anchorPoint.transform.position = Whole.theQuest.trackingCoordinates
                [Whole.theQuest.Process];
            lineRenderer.enabled=true;

        }
        else
        {
            anchorPoint.SetActive(false);
            lineRenderer.enabled=false;
        }
    }
}
