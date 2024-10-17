using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float timer=1f; // ��ת���ڣ��룩
    private float minRotation = -15f; // ��С��ת�Ƕ�
    private float maxRotation = 75f;   // �����ת�Ƕ�
    private float currentRotation;      // ��ǰ��ת�Ƕ�
    private bool rotatingToMax = true; // ��ǰ�Ƿ�����ת�����ֵ

    // Update is called once per frame
    void Update()
    {
        // ���㵱ǰ����ת�ٶ�
        float rotationSpeed = (maxRotation - minRotation) / timer * Time.deltaTime;

        // ���ݵ�ǰ״̬������ת�Ƕ�
        if (rotatingToMax)
        {
            currentRotation += rotationSpeed;
            if (currentRotation >= maxRotation) // �ﵽ�����ת�Ƕ�
            {
                currentRotation = maxRotation; // ȷ�����������ֵ
                rotatingToMax = false; // �л�����ת����Сֵ
            }
        }
        else
        {
            currentRotation -= rotationSpeed;
            if (currentRotation <= minRotation) // �ﵽ��С��ת�Ƕ�
            {
                currentRotation = minRotation; // ȷ����������Сֵ
                rotatingToMax = true; // �л�����ת�����ֵ
            }
        }

        // Ӧ����ת�������Z��
        transform.rotation = Quaternion.Euler(0, 0, currentRotation);
    }
}
