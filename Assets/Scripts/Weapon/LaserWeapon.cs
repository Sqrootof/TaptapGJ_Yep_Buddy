using UnityEngine;

public class LaserWeapon : MonoBehaviour
{
    public Camera Camera;
    public GameObject Line;
    public LineRenderer lineRenderer; // 用于显示激光
    public LayerMask obstacleLayer; // 用于检测的障碍物图层
    public LayerMask enemyLayer; // 用于检测敌人的图层
    public float damageAmount; // 每次激光造成的伤害
    public GameObject explosionPrefab; // 爆炸效果预制体
    public int maxReflections; //最大反射次数
    public GameObject laserStartParticlePrefab; // 粒子系统预制体
    float distanceFromCamera;
    private void Start()
    {
        // 确保 LineRenderer 组件已添加
        if (lineRenderer == null)
        {
            lineRenderer=Line.GetComponent<LineRenderer>();
        }
        Line.SetActive(false);
        distanceFromCamera = -Camera.transform.position.z;
    }

    private void Update()
    {
        // 检测鼠标左键是否按下
        if (Input.GetMouseButton(0)) // 0 表示鼠标左键
        {
            // 获取鼠标位置并更新激光
            Vector3 mouseWorldPosition = GetMousePosition();
            UpdateLaser(mouseWorldPosition);
            //lineRenderer.enabled = true; // 显示激光
            Line.SetActive(true);
        }
        else
        {
            //lineRenderer.enabled = false; // 隐藏激光
            Line.SetActive(false);
        }
    }

    public Vector3 GetMousePosition()
    {
        // 获取鼠标屏幕位置
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = distanceFromCamera;
        Vector3 mouseWorldPosition = Camera.ScreenToWorldPoint(mouseScreenPosition);
        return mouseWorldPosition;
    }

    //激光穿透敌人版本
    private void UpdateLaser(Vector3 targetPosition)
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
            if (Physics.Raycast(currentPosition, currentDirection, out hit, laserMaxDistance, obstacleLayer | enemyLayer))
            {
                // 更新激光的终点或反射点
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(reflections + 1, hit.point); // 添加新的激光点

                // 检测是否击中敌人
                if ((enemyLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    // 击中敌人，但继续穿透
                    Enemy enemyHealth = hit.collider.GetComponent<Enemy>();
                    if (enemyHealth != null)
                    {
                        enemyHealth.currentHealth -= damageAmount * (maxReflections - reflections+1); // 对敌人造成伤害
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
    }
}
