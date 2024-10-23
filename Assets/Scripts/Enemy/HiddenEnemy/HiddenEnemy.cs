using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class HiddenEnemy : Enemy
{
    public Enemy enemy; // Enemy ʵ������
    public EnemyLaser enemyLaser;

    public float bulletSpeed; // �ӵ��ٶ�
    public float attackTime;
    public float stopDuration = 2f; // ͣ��ʱ��
    public float stopTimer; // ͣ����ʱ��

    public bool movingToA = true; // ��ǰ�Ƿ�A���ƶ�
    public bool isStopping = false; // �Ƿ�����ͣ��
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
        targetPoint = patrolPointA; // ��ʼĿ���ΪA��
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
            // �������ͣ��
            if (isStopping)
            {
                stopTimer += Time.deltaTime; // ����ͣ����ʱ��
                if (stopTimer >= stopDuration)
                {
                    // ͣ��ʱ��������л�Ŀ���
                    movingToA = !movingToA;
                    targetPoint = movingToA ? patrolPointA : patrolPointB;
                    isStopping = false; // ����ͣ��״̬
                }
                return; // ͣ��ʱ��ִ�������߼�
            }

            // ����Ƿ񵽴�Ŀ���
            if (Mathf.Abs(enemy.transform.position.x - targetPoint.x) < 0.1f)
            {
                // ��ʼͣ��
                isStopping = true;
                stopTimer = 0f; // ���ü�ʱ��
            }
        }

        base.Update();
    }

    public void SetTargetPosition()
    {
        // ����Ŀ�귽��
        Vector3 direction = enemy.player.transform.position - enemy.transform.position;
        Quaternion targetRotation;
        if (direction.x < 0) // ����
        {
            targetRotation = Quaternion.Euler(0, 40, 0); // ����ת��
        }
        else// ����
        {
            targetRotation = Quaternion.Euler(0, 0, 0); // ����ת��
        }
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 15f); // 5f Ϊת���ٶ�
        // �������һ��
        direction.Normalize();
        // ����Ŀ��λ�ã����˵ĵ�ǰλ�� + ���� * 2
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
            // ���û��ͣ�������ƶ�С��
            if (!isStopping)
            {
                targetPoint.y = transform.position.y;
                enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);

                Vector3 direction = targetPoint - enemy.transform.position; // ����Ŀ�귽��
                Quaternion targetRotation;

                if (direction.x < 0) // ����
                {
                    targetRotation = Quaternion.Euler(0, 40, 0); // ����ת��
                }
                else if (direction.x >= 0) // ����
                {
                    targetRotation = Quaternion.Euler(0, 0, 0); // ����ת��
                }
                else
                {
                    return; // ���û���ƶ���ֱ�ӷ���
                }

                // ʹ�� Slerp ʵ��ƽ��ת��
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, Time.deltaTime * 15f); // 5f Ϊת���ٶ�
            }
        }
        base.FixedUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacles"))
        {
            // �����ϰ��ֹͣ�ƶ����л�Ŀ���
            isStopping = true; // ��ʼͣ��
            stopTimer = 0f; // ���ü�ʱ��
        }
    }
}
