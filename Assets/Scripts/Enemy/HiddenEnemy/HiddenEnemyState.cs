using UnityEngine;
using static Enemy;

/// <summary>
/// С�ֵĻ���Ѳ��״̬������С�ֵ�Ѳ��״̬�̳д�״̬
/// </summary>
public class HiddenEnemyStatePatrol : EnemyState
{
    private float attackTime;
    HiddenEnemy hiddenEnemy;

    public HiddenEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, HiddenEnemy hiddenEnemy) : base(enemy, enemyFSM)
    {
        this.hiddenEnemy = hiddenEnemy;
    }

    public override void OnEnter()
    {
        attackTime = 1.5f;

        hiddenEnemy.stopTimer = 0f; // ��ʼ��ͣ����ʱ��
        hiddenEnemy.isStopping = true; // ��ʼ��ͣ��״̬
    }

    public override void LogicUpdate()
    {
        attackTime -= Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {

        // ��⹥����Χ
        if (enemy.IsPlayerInAttackRange())
        {
            if (attackTime <= 0f)
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
    }

    public override void OnExit()
    {

    }
}


/// <summary>
/// С�ֵĻ���׷��״̬������С��׷��״̬�̳д�״̬
/// </summary>
public class HiddenEnemyStateChase : EnemyState
{

    public HiddenEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, HiddenEnemy hiddenEnemy) : base(enemy, enemyFSM)
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
public class HiddenEnemyStateAttack : EnemyState
{
    HiddenEnemy hiddenEnemy;
    float time;
    int num;
    public HiddenEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, HiddenEnemy hiddenEnemy) : base(enemy, enemyFSM)
    {
        this.hiddenEnemy = hiddenEnemy;
    }

    public override void OnEnter()
    {
        hiddenEnemy.SetTargetPosition();
        hiddenEnemy.body.SetActive(true);
        hiddenEnemy.StartLaser();
        hiddenEnemy.Attack();
        time = 0f;
        num = 0;
    }

    public override void LogicUpdate()
    {
        time += Time.deltaTime;
        if (time > 0.5f)
        {
            if (num == 0)
            {
                hiddenEnemy.FinishLaser();
                hiddenEnemy.transform.position = hiddenEnemy.targetPosition;
                num++;
                time = 0f;
            }
            else
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }

        }
    }

    public override void PhysicsUpdate()
    {

    }

    public override void OnExit()
    {
        hiddenEnemy.body.SetActive(false);
        hiddenEnemy.hitCollider.SetActive(false);
    }
}

/// <summary>
/// С�ֵĻ�������״̬������С������״̬�̳д�״̬
/// </summary>
public class HiddenEnemyStateDead : EnemyState
{
    public HiddenEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, HiddenEnemy hiddenEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.DestroyEnemy();
    }

    public override void LogicUpdate()
    {

    }

    public override void PhysicsUpdate()
    {
        enemy.rb.velocity = Vector3.zero;
    }

    public override void OnExit()
    {

    }
}
