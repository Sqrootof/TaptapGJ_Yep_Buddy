using UnityEngine;
using static Enemy;

/// <summary>
/// С�ֵĻ���Ѳ��״̬������С�ֵ�Ѳ��״̬�̳д�״̬
/// </summary>
public class EasyEnemyStatePatrol : EnemyState
{
    private Vector3 targetPoint;
    private float attackTime;
    private float stopDuration = 2.0f; // ͣ��ʱ��
    private float stopTimer; // ͣ����ʱ��
    private bool movingToA = true; // ��ǰ�Ƿ�A���ƶ�
    private bool isStopping = false; // �Ƿ�����ͣ��

    public EasyEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {
        // ����Ѳ�ߵ�
        targetPoint = enemy.patrolPointA; // ��ʼĿ���ΪA��
    }

    public override void OnEnter()
    {
        attackTime = 1.5f;
        stopTimer = 0f; // ��ʼ��ͣ����ʱ��
        isStopping = false; // ��ʼ��ͣ��״̬
    }

    public override void LogicUpdate()
    {
        // �������ͣ��
        if (isStopping)
        {
            stopTimer += Time.deltaTime; // ����ͣ����ʱ��
            if (stopTimer >= stopDuration)
            {
                // ͣ��ʱ��������л�Ŀ���
                movingToA = !movingToA;
                targetPoint = movingToA ? enemy.patrolPointA : enemy.patrolPointB;
                isStopping = false; // ����ͣ��״̬
            }
            return; // ͣ��ʱ��ִ�������߼�
        }

        // ����Ƿ񵽴�Ŀ���
        if (Vector3.Distance(enemy.transform.position, targetPoint) < 0.1f)
        {
            // ��ʼͣ��
            isStopping = true;
            stopTimer = 0f; // ���ü�ʱ��
        }
    }

    public override void PhysicsUpdate()
    {
        // ��ײ���
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit, 1f))
        {
            if (hit.collider.CompareTag("Obstacles"))
            {
                // �����ϰ��ֹͣ�ƶ�
                movingToA = !movingToA;
                return;
            }
        }

        // ��⹥����Χ
        if (enemy.IsPlayerInAttackRange())
        {
            if (attackTime <= 0f)
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }

        // ���û��ͣ�������ƶ�С��
        if (!isStopping)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, targetPoint, enemy.patrolSpeed * Time.deltaTime);
        }
    }

    public override void OnExit()
    {
        // �����˳�ʱ��һЩ������
    }
}


/// <summary>
/// С�ֵĻ���׷��״̬������С��׷��״̬�̳д�״̬
/// </summary>
public class EasyEnemyStateChase : EnemyState
{

    public EasyEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// С�ֵĻ�������״̬������С�ֹ���״̬�̳д�״̬
/// </summary>
public class EasyEnemyStateAttack : EnemyState
{
    EasyEnemy easyEnemy;
    public EasyEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {
        this.easyEnemy = easyEnemy;
    }

    public override void OnEnter()
    {

    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        easyEnemy.TryAttack();
        enemyFSM.ChangeState(enemy.patrolState);
    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// С�ֵĻ�������״̬������С������״̬�̳д�״̬
/// </summary>
public class EasyEnemyStateDead : EnemyState
{
    public EasyEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.DestroyEnemy();
    }

    public override void LogicUpdate()
    {
        Debug.Log(2);
    }

    public override void PhysicsUpdate()
    {
        enemy.rb.velocity = Vector3.zero;
    }

    public override void OnExit()
    {

    }
}
