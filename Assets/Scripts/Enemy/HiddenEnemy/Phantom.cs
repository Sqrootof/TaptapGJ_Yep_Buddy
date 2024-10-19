using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phantom : MonoBehaviour
{
    private MeshRenderer objectRenderer; // ��Ϊ MeshRenderer
    private Color startColor;
    public float flashSpeed;
    private float timeCounter = 0f;

    void Start()
    {
        objectRenderer = GetComponent<MeshRenderer>(); // ��ȡ MeshRenderer ���
        startColor = objectRenderer.material.color; // �洢ԭʼ��ɫ
    }

    void Update()
    {
        timeCounter += Time.deltaTime * flashSpeed; // ����ʱ�����
        float alpha = Mathf.PingPong(timeCounter, 1f); // ����һ���� 0 �� 1 ֮���ֵ
        // ��������ɫ���޸� alpha ֵ
        objectRenderer.material.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
    }
}
