using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletBatEnemy : Enemy
{
    public Enemy enemy; // Enemy 实例引用
    public GameObject bulletPrefab; // 子弹的预制体
    public GameObject target;

    public Vector3 targetPoint;
    public Vector3 InitialPosition;
    public Vector3 targetPosition;

    public float bulletSpeed; // 子弹速度
    public float attackTime;
    public float stopDuration = 2f; // 停留时间
    public float stopTimer; // 停留计时器

    public bool movingToA = true; // 当前是否朝A点移动
    public bool isStopping = false; // 是否正在停留
    public bool bullet;
    public bool showGizmos = false; // 允许在Inspector中控制是否显示Gizmos

    // 设置巡逻范围的大小
    public Vector3 patrolAreaSize = new Vector3(5f, 5f, 0f); // X, Y, Z 轴的大小

    private void OnDrawGizmos()
    {
        if (!showGizmos) return; // 如果不想显示，直接返回

        // 获取敌人的当前坐标
        Vector3 position = InitialPosition;

        // 设置Gizmos的颜色
        Gizmos.color = Color.green;

        // 绘制一个立方体表示巡逻范围
        Gizmos.DrawCube(position, patrolAreaSize);
    }

    // 调用这个方法来尝试攻击
    public void TryAttack()
    {
        Vector3 playerPosition = enemy.player.transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
        bulletInstance.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
    }

    public void FinishAttack()
    {
        enemyFSM.ChangeState(patrolState);
    }
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BulletBatEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new BulletBatEnemyStateChase(this, enemyFSM, this);
        attackState = new BulletBatEnemyStateAttack(this, enemyFSM, this);
        deadState = new BulletBatEnemyStateDead(this, enemyFSM, this);
        enemyFSM.startState = patrolState;
    }

    protected override void OnEnable()
    {
        enemyFSM.startState = patrolState;

        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        enemy = FindObjectOfType<Enemy>();
        base.Start();
    }

    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            enemyFSM.ChangeState(deadState);
            return;
        }
        targetPosition = target.transform.position;
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public void MoveToTarget(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }

    // 更新目标位置为巡逻范围内的随机位置
    public void SetRandomTargetPosition()
    {
        // 获取敌人的当前坐标
        Vector3 position = InitialPosition;

        // 计算随机位置，基于巡逻范围
        float randomX = Random.Range(position.x - patrolAreaSize.x / 2, position.x + patrolAreaSize.x / 2);
        float randomY = Random.Range(position.y - patrolAreaSize.y / 2, position.y + patrolAreaSize.y / 2);

        // 将目标位置更新为新的随机位置
        target.transform.position = new Vector3(randomX, randomY, 0); // Z轴保持为当前敌人Z坐标
        targetPosition = target.transform.position; // 更新targetPosition
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Obstacles"))
    //    {
    //        // 碰到障碍物，停止移动并切换目标点
    //        //movingToA = !movingToA;
    //        //targetPoint = movingToA ? enemy.patrolPointA : enemy.patrolPointB;
    //        isStopping = true; // 开始停留
    //        stopTimer = 0f; // 重置计时器
    //    }
    //}
}
