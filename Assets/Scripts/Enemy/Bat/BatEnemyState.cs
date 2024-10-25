using UnityEngine;
using static Enemy;

/// <summary>
/// Ð¡¹ÖµÄ»ù´¡Ñ²Âß×´Ì¬£¬ËùÓÐÐ¡¹ÖµÄÑ²Âß×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BatEnemyStatePatrol : EnemyState
{
    BatEnemy batEnemy;
    bool isStop;
    float time;
    float attackTime;

    public BatEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, BatEnemy batEnemy) : base(enemy, enemyFSM)
    {
        this.batEnemy = batEnemy;
    }

    public override void OnEnter()
    {
        isStop = true;
        time = 1f;
        attackTime = 2f;
    }

    public override void LogicUpdate()
    {
        attackTime -= Time.deltaTime;
        if (Vector3.Distance(batEnemy.transform.position, batEnemy.targetPosition) < 0.1f)
        {
            isStop = true;
        }

        if (isStop == true)
        {
            time += Time.deltaTime;
        }
        if (time > 2f)
        {
            batEnemy.SetRandomTargetPosition();
            isStop = false;
            time = 0f;
        }
        if (enemy.IsPlayerInAttackRange())
        {
            if (attackTime <= 0f)
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (isStop == false)
        {
            batEnemy.MoveToTarget(enemy.patrolSpeed);
        }
    }

    public override void OnExit()
    {

    }
}


/// <summary>
/// Ð¡¹ÖµÄ»ù´¡×·»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö×·»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BatEnemyStateChase : EnemyState
{

    public BatEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, BatEnemy batEnemy) : base(enemy, enemyFSM)
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
/// Ð¡¹ÖµÄ»ù´¡¹¥»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö¹¥»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BatEnemyStateAttack : EnemyState
{
    BatEnemy batEnemy;
    int num;
    public BatEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, BatEnemy batEnemy) : base(enemy, enemyFSM)
    {
        this.batEnemy = batEnemy;
    }

    public override void OnEnter()
    {
        num = 0;
        batEnemy.SetPlayerTargetPosition();
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(enemy.transform.position, batEnemy.targetPosition) < 0.1f)
        {
            if (num == 0)
            {
                num++;
                //birdEnemy.SetRandomTargetPosition();
                batEnemy.target.transform.position = batEnemy.InitialPosition;
            }
            else
            {
                enemyFSM.ChangeState(batEnemy.patrolState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        batEnemy.MoveToTarget(enemy.chaseSpeed);
    }

    public override void OnExit()
    {
        enemy.rb.velocity = batEnemy.rb.velocity * 0;
    }
}


/// <summary>
/// Ð¡¹ÖµÄ»ù´¡ËÀÍö×´Ì¬£¬ËùÓÐÐ¡¹ÖËÀÍö×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BatEnemyStateDead : EnemyState
{
    public BatEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, BatEnemy batEnemy) : base(enemy, enemyFSM)
    {

    }

    public override void OnEnter()
    {
        enemy.Drop();
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
