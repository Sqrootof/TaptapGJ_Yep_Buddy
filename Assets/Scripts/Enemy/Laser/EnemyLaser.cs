using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    //public GameObject target;
    //public Vector3 targetPosition;
    public GameObject Line;
    public LineRenderer lineRenderer; // ������ʾ����
    public LayerMask obstacleLayer; // ���ڼ����ϰ���ͼ��
    public LayerMask playerLayer; // ���ڼ����˵�ͼ��
    public float damageAmount; // ÿ�μ�����ɵ��˺�
    public GameObject explosionPrefab; // ��ըЧ��Ԥ����
    public int maxReflections; //��������
    public GameObject laserStartParticlePrefab; // ����ϵͳԤ����

    public Vector3 end;

    //private void Start()
    //{
    //    targetPosition=target.transform.position;
    //}

    public void UpdateLaser(Vector3 targetPosition)
    {
        // �������ʼ����������λ��
        Vector3 startPosition = transform.position;
        Vector3 laserDirection = targetPosition - startPosition;

        // ʹ�����߼��
        RaycastHit hit;
        float laserMaxDistance = 100f;
        int reflections = 0; // ��ǰ�������

        // ��ʼ�������߶�λ��
        lineRenderer.positionCount = 1; // ��̬�����������
        lineRenderer.SetPosition(0, startPosition); // ��������

        Transform lineChild = Line.transform.GetChild(0); // ��ȡ��һ��������
        lineChild.position = startPosition;
        if (laserDirection != Vector3.zero) // ȷ������Ϊ��
        {
            lineChild.rotation = Quaternion.LookRotation(laserDirection); // ���ݼ��ⷽ��������ת
        }


        Vector3 currentPosition = startPosition;
        Vector3 currentDirection = laserDirection;

        while (reflections <= maxReflections)
        {
            // ���߼��
            if (Physics.Raycast(currentPosition, currentDirection, out hit, laserMaxDistance, obstacleLayer | playerLayer))
            {
                // ���¼�����յ�����
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(reflections + 1, hit.point); // ����µļ����

                // ����Ƿ���е���
                if ((playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    // ���е��ˣ���������͸
                    Player Health = hit.collider.GetComponent<Player>();
                    if (Health != null)
                    {
                        Health.health -= damageAmount * (maxReflections - reflections+1); // �Ե�������˺�
                        Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                    }

                    // ����һ�η�����ᣬ��������͸����
                    reflections++;
                    currentPosition = hit.point + currentDirection.normalized * 0.01f; // �����ӵ��˺���һ���λ��
                                                                                       // �����������ͬ�� `currentDirection`����ʾ��͸�������ǰ
                    continue; // ������һ�η����͸���
                }
                else
                {
                    // û�л��е��ˣ�����
                    Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                    currentDirection = Vector3.Reflect(currentDirection.normalized, hit.normal); // ���㷴�䷽��
                    currentPosition = hit.point; // ���·������
                    reflections++; // ���ӷ������
                }
            }
            else
            {
                // ���û�������ϰ������ﵽ������
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(reflections + 1, currentPosition + currentDirection.normalized * laserMaxDistance); // ������յ�
                break; // �˳�ѭ���������Ѿ��ﵽ������
            }
        }

        end = currentPosition; // ����endΪ���һ��������յ�
    }
}
