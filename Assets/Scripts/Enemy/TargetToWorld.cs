using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetToWorld : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // ����Ϸ��ʼʱ�������������丸����
        DetachFromParent();
    }

    void DetachFromParent()
    {
        // ��������ĸ���������Ϊ null
        transform.SetParent(null);
    }

    // Update is called once per frame
    void Update()
    {
        // �������Ҫ�ڸ���ʱ��������������������������Ӵ���
    }
}