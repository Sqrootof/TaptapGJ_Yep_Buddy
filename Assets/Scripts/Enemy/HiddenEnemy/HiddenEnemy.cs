using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class HiddenEnemy : Enemy
{
    public Enemy enemy; // Enemy 实例引用
    public EnemyLaser enemyLaser;

    public float bulletSpeed; // 子弹速度
    public float attackTime;
    public float stopDuration = 2f; // 停留时间
    public float stopTimer; // 停留计时器

    public bool movingToA = true; // 当前是否朝A点移动
    public bool isStopping = false; // 是否正在停留
    public bool bullet;

    public GameObject PointA;
    public GameObject PointB;
    public GameObject body;
    public GameObject hitCollider;

    public Vector3 patrolPointA;
    public Vector3 patrolPointB;
    public Vector3 targetPosition;
    public Vector3 targetPoint;


    protected override void Awake()
    {
        base.Awake();
        patrolState = new HiddenEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new HiddenEnemyStateChase(this, enemyFSM, this);
        attackState = new HiddenEnemyStateAttack(this, enemyFSM, this);
        deadState = new HiddenEnemyStateDead(this, enemyFSM, this);
        enemyFSM.startState = patrolState;
        patrolPointA = PointA.transform.position;
        patrolPointB = PointB.transform.position;
        PointA.SetActive(false);
        PointB.SetActive(false);
        enemyLaser=GetComponent<EnemyLaser>();
        body.SetActive(false);
        FinishLaser();
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
        if (enemyFSM.currentState == patrolState)
        {
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
        }

        base.Update();
    }

    public void SetTargetPosition()
    {
        // 计算目标方向
        Vector3 direction = enemy.player.transform.position - enemy.transform.position;
        Quaternion targetRotation;
        if (direction.x < 0) // 向左
        {
            targetRotation = Quaternion.Euler(0, 40, 0); // 向左转向
        }
        else// 向右
        {
            targetRotation = Quaternion.Euler(0, 0, 0); // 向右转向
        }
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 15f); // 5f 为转向速度
        // 将方向归一化
        direction.Normalize();
        // 计算目标位置，敌人的当前位置 + 方向 * 2
        if (direction.x>0)
        {
            targetPosition = enemy.transform.position + new Vector3(2, 0, 0);
            hitCollider.transform.position = enemy.transform.position + new Vector3(1, 0, 0);
        }
        else
        {
            targetPosition = enemy.transform.position + new Vector3(-2, 0, 0);
            hitCollider.transform.position = enemy.transform.position + new Vector3(-1, 0, 0);
        }
        hitCollider.SetActive(true);
    }

    public void StartLaser()
    {
        enemyLaser.Line.SetActive(true);
    }

    public void FinishLaser()
    {
        enemyLaser.Line.SetActive(false);
    }

    public void Attack()
    {
        enemyLaser.UpdateLaser(targetPosition);
    }
    protected override void FixedUpdate()
    {
        if (enemyFSM.currentState == patrolState)
        {
            // 如果没有停留，则移动小怪
            if (!isStopping)
            {
                targetPoint.y = transform.position.y;
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);

                Vector3 direction = targetPoint - enemy.transform.position; // 计算目标方向
                Quaternion targetRotation;

                if (direction.x < 0) // 向左
                {
                    targetRotation = Quaternion.Euler(0, 40, 0); // 向左转向
                }
                else if (direction.x >= 0) // 向右
                {
                    targetRotation = Quaternion.Euler(0, 0, 0); // 向右转向
                }
                else
                {
                    return; // 如果没有移动，直接返回
                }

                // 使用 Slerp 实现平滑转向
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 15f); // 5f 为转向速度
            }
        }
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            // 碰到障碍物，停止移动并切换目标点
            isStopping = true; // 开始停留
            stopTimer = 0f; // 重置计时器
        }
    }
}
