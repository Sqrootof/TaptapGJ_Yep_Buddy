using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class EasyEnemy : Enemy
{
    public Enemy enemy; // Enemy 实例引用
    public GameObject bulletPrefab; // 子弹的预制体
    public float bulletSpeed; // 子弹速度
    public bool bullet;

    public Vector3 targetPoint;
    public float attackTime;
    public float stopDuration=2f; // 停留时间

    public float stopTimer; // 停留计时器
    public bool movingToA = true; // 当前是否朝A点移动
    public bool isStopping = false; // 是否正在停留
                                    // 定义巡逻点
    public GameObject PointA;
    public GameObject PointB;
    public Vector3 patrolPointA;
    public Vector3 patrolPointB;

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
        patrolState = new EasyEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new EasyEnemyStateChase(this, enemyFSM, this);
        attackState = new EasyEnemyStateAttack(this, enemyFSM, this);
        deadState = new EasyEnemyStateDead(this, enemyFSM, this);
        enemyFSM.startState = patrolState;
        patrolPointA = PointA.transform.position;
        patrolPointB = PointB.transform.position;
        PointA.SetActive(false);
        PointB.SetActive(false);
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
        targetPoint = patrolPointA; // 初始目标点为A点
        base.Start();
    }

    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            enemyFSM.ChangeState(deadState);
            return;
        }

        // 如果正在停留
        if (isStopping)
        {
            stopTimer += Time.deltaTime; // 增加停留计时器
            if (stopTimer >= stopDuration)
            {
                // 停留时间结束，切换目标点
                movingToA = !movingToA;
                targetPoint = movingToA ? patrolPointA : patrolPointB;
                isStopping = false; // 重置停留状态
            }
            return; // 停留时不执行其他逻辑
        }

        // 检测是否到达目标点
        if (Mathf.Abs(enemy.transform.position.x - targetPoint.x) < 0.1f)
        {
            // 开始停留
            isStopping = true;
            stopTimer = 0f; // 重置计时器
        }

        base.Update();
    }

    protected override void FixedUpdate()
    {
        // 如果没有停留，则移动小怪
        if (!isStopping)
        {
            targetPoint.y=transform.position.y;
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);
        }
        base.FixedUpdate();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            // 碰到障碍物，停止移动并切换目标点
            //movingToA = !movingToA;
            //targetPoint = movingToA ? enemy.patrolPointA : enemy.patrolPointB;
            isStopping = true; // 开始停留
            stopTimer = 0f; // 重置计时器
        }
    }
}
