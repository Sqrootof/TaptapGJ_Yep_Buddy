using UnityEngine;
using static Enemy;

/// <summary>
/// Ð¡¹ÖµÄ»ù´¡Ñ²Âß×´Ì¬£¬ËùÓÐÐ¡¹ÖµÄÑ²Âß×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BulletBatEnemyStatePatrol : EnemyState
{
    BulletBatEnemy bulletBatEnemy;
    bool isStop;
    float time;
    float attackTime;

    public BulletBatEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, BulletBatEnemy bulletBatEnemy) : base(enemy, enemyFSM)
    {
        this.bulletBatEnemy = bulletBatEnemy;
    }

    public override void OnEnter()
    {
        isStop = true;
        time = 1f;
        attackTime = 1.5f;
    }

    public override void LogicUpdate()
    {
        attackTime-=Time.deltaTime;
        if (Vector3.Distance(bulletBatEnemy.transform.position, bulletBatEnemy.targetPosition) < 0.1f)
        {
            isStop = true;
        }

        if (isStop == true)
        {
            time += Time.deltaTime;
        }
        if (time > 2f)
        {
            bulletBatEnemy.SetRandomTargetPosition();
            isStop = false;
            time = 0f;
        }
        if(enemy.IsPlayerInAttackRange())
        {
            if(attackTime<=0f)
            {
                enemyFSM.ChangeState(enemy.attackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (isStop == false)
        {
            bulletBatEnemy.MoveToTarget(enemy.patrolSpeed);
        }
    }

    public override void OnExit()
    {

    }
}


/// <summary>
/// Ð¡¹ÖµÄ»ù´¡×·»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö×·»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BulletBatEnemyStateChase : EnemyState
{

    public BulletBatEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, BulletBatEnemy bulletBatEnemy) : base(enemy, enemyFSM)
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
public class BulletBatEnemyStateAttack : EnemyState
{
    BulletBatEnemy bulletBatEnemy;
    public BulletBatEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, BulletBatEnemy bulletBatEnemy) : base(enemy, enemyFSM)
    {
        this.bulletBatEnemy = bulletBatEnemy;
    }

    public override void OnEnter()
    {
        bulletBatEnemy.TryAttack();
        enemyFSM.ChangeState(enemy.patrolState);
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
/// Ð¡¹ÖµÄ»ù´¡ËÀÍö×´Ì¬£¬ËùÓÐÐ¡¹ÖËÀÍö×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class BulletBatEnemyStateDead : EnemyState
{
    public BulletBatEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, BulletBatEnemy bulletBatEnemy) : base(enemy, enemyFSM)
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
