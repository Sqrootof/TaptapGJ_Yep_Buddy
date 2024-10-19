using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public class EasyEnemy : Enemy
{
    public Enemy enemy; // Enemy ʵ������
    public GameObject bulletPrefab; // �ӵ���Ԥ����
    public float bulletSpeed; // �ӵ��ٶ�
    public bool bullet;

    public Vector3 targetPoint;
    public float attackTime;
    public float stopDuration=2f; // ͣ��ʱ��

    public float stopTimer; // ͣ����ʱ��
    public bool movingToA = true; // ��ǰ�Ƿ�A���ƶ�
    public bool isStopping = false; // �Ƿ�����ͣ��
                                    // ����Ѳ�ߵ�
    public GameObject PointA;
    public GameObject PointB;
    public Vector3 patrolPointA;
    public Vector3 patrolPointB;

    // ����������������Թ���
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

        base.Update();
    }

    protected override void FixedUpdate()
    {
        // ���û��ͣ�������ƶ�С��
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
            // �����ϰ��ֹͣ�ƶ����л�Ŀ���
            //movingToA = !movingToA;
            //targetPoint = movingToA ? enemy.patrolPointA : enemy.patrolPointB;
            isStopping = true; // ��ʼͣ��
            stopTimer = 0f; // ���ü�ʱ��
        }
    }
}
