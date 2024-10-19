using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BatEnemy : Enemy
{
    public Enemy enemy; // Enemy ʵ������
    public GameObject target;

    public Vector3 targetPoint;
    public Vector3 InitialPosition;
    public Vector3 targetPosition;

    public float bulletSpeed; // �ӵ��ٶ�
    public float attackTime;
    public float stopDuration = 2f; // ͣ��ʱ��
    public float stopTimer; // ͣ����ʱ��
    public float damage;

    public bool movingToA = true; // ��ǰ�Ƿ�A���ƶ�
    public bool isStopping = false; // �Ƿ�����ͣ��
    public bool bullet;
    public bool showGizmos = false; // ������Inspector�п����Ƿ���ʾGizmos

    // ����Ѳ�߷�Χ�Ĵ�С
    public Vector3 patrolAreaSize = new Vector3(5f, 5f, 0f); // X, Y, Z ��Ĵ�С

    private void OnDrawGizmos()
    {
        if (!showGizmos) return; // ���������ʾ��ֱ�ӷ���

        // ��ȡ���˵ĵ�ǰ����
        Vector3 position = InitialPosition;

        // ����Gizmos����ɫ
        Gizmos.color = Color.green;

        // ����һ���������ʾѲ�߷�Χ
        Gizmos.DrawCube(position, patrolAreaSize);
    }

    public void FinishAttack()
    {
        enemyFSM.ChangeState(patrolState);
    }
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BatEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new BatEnemyStateChase(this, enemyFSM, this);
        attackState = new BatEnemyStateAttack(this, enemyFSM, this);
        deadState = new BatEnemyStateDead(this, enemyFSM, this);
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

    // ����Ŀ��λ��ΪѲ�߷�Χ�ڵ����λ��
    public void SetRandomTargetPosition()
    {
        // ��ȡ���˵ĵ�ǰ����
        Vector3 position = InitialPosition;

        // �������λ�ã�����Ѳ�߷�Χ
        float randomX = Random.Range(position.x - patrolAreaSize.x / 2, position.x + patrolAreaSize.x / 2);
        float randomY = Random.Range(position.y - patrolAreaSize.y / 2, position.y + patrolAreaSize.y / 2);

        // ��Ŀ��λ�ø���Ϊ�µ����λ��
        target.transform.position = new Vector3(randomX, randomY, 0); // Z�ᱣ��Ϊ��ǰ����Z����
        targetPosition = target.transform.position; // ����targetPosition
    }

    public void SetPlayerTargetPosition()
    {
        target.transform.position = enemy.player.transform.position;
        targetPosition = target.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player Health = other.GetComponent<Player>();
            if (Health != null)
            {
                Health.health -= damage;
            }
        }
    }
}
