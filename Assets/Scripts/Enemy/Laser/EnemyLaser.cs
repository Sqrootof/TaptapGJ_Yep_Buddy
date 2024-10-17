using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLaser : MonoBehaviour
{
    //public GameObject target;
    //public Vector3 targetPosition;
    public GameObject Line;
    public LineRenderer lineRenderer; // 用于显示激光
    public LayerMask obstacleLayer; // 用于检测的障碍物图层
    public LayerMask playerLayer; // 用于检测敌人的图层
    public float damageAmount; // 每次激光造成的伤害
    public GameObject explosionPrefab; // 爆炸效果预制体
    public int maxReflections; //最大反射次数
    public GameObject laserStartParticlePrefab; // 粒子系统预制体

    public Vector3 end;

    //private void Start()
    //{
    //    targetPosition=target.transform.position;
    //}

    public void UpdateLaser(Vector3 targetPosition)
    {
        // 激光的起始点是武器的位置
        Vector3 startPosition = transform.position;
        Vector3 laserDirection = targetPosition - startPosition;

        // 使用射线检测
        RaycastHit hit;
        float laserMaxDistance = 100f;
        int reflections = 0; // 当前反射次数

        // 初始化激光线段位置
        lineRenderer.positionCount = 1; // 动态调整点的数量
        lineRenderer.SetPosition(0, startPosition); // 激光的起点

        Transform lineChild = Line.transform.GetChild(0); // 获取第一个子物体
        lineChild.position = startPosition;
        if (laserDirection != Vector3.zero) // 确保方向不为零
        {
            lineChild.rotation = Quaternion.LookRotation(laserDirection); // 根据激光方向设置旋转
        }


        Vector3 currentPosition = startPosition;
        Vector3 currentDirection = laserDirection;

        while (reflections <= maxReflections)
        {
            // 射线检测
            if (Physics.Raycast(currentPosition, currentDirection, out hit, laserMaxDistance, obstacleLayer | playerLayer))
            {
                // 更新激光的终点或反射点
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(reflections + 1, hit.point); // 添加新的激光点

                // 检测是否击中敌人
                if ((playerLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    // 击中敌人，但继续穿透
                    Player Health = hit.collider.GetComponent<Player>();
                    if (Health != null)
                    {
                        Health.health -= damageAmount * (maxReflections - reflections+1); // 对敌人造成伤害
                        Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                    }

                    // 减少一次反射机会，但继续穿透敌人
                    reflections++;
                    currentPosition = hit.point + currentDirection.normalized * 0.01f; // 继续从敌人后面一点的位置
                                                                                       // 这里继续用相同的 `currentDirection`，表示穿透后继续向前
                    continue; // 继续下一次反射或穿透检测
                }
                else
                {
                    // 没有击中敌人，反射
                    Instantiate(explosionPrefab, hit.point, Quaternion.identity);
                    currentDirection = Vector3.Reflect(currentDirection.normalized, hit.normal); // 计算反射方向
                    currentPosition = hit.point; // 更新反射起点
                    reflections++; // 增加反射次数
                }
            }
            else
            {
                // 如果没有碰到障碍物，激光达到最大射程
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(reflections + 1, currentPosition + currentDirection.normalized * laserMaxDistance); // 激光的终点
                break; // 退出循环，激光已经达到最大射程
            }
        }

        end = currentPosition; // 更新end为最后一个反射的终点
    }
}
