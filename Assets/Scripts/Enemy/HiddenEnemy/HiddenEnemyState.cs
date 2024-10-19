using UnityEngine;
using static Enemy;

/// <summary>
/// Ð¡¹ÖµÄ»ù´¡Ñ²Âß×´Ì¬£¬ËùÓÐÐ¡¹ÖµÄÑ²Âß×´Ì¬¼Ì³Ð´Ë×´Ì¬
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

        hiddenEnemy.stopTimer = 0f; // ³õÊ¼»¯Í£Áô¼ÆÊ±Æ÷
        hiddenEnemy.isStopping = true; // ³õÊ¼»¯Í£Áô×´Ì¬
    }

    public override void LogicUpdate()
    {
        attackTime -= Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {

        // ¼ì²â¹¥»÷·¶Î§
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
/// Ð¡¹ÖµÄ»ù´¡×·»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö×·»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
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
/// Ð¡¹ÖµÄ»ù´¡¹¥»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö¹¥»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
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
/// Ð¡¹ÖµÄ»ù´¡ËÀÍö×´Ì¬£¬ËùÓÐÐ¡¹ÖËÀÍö×´Ì¬¼Ì³Ð´Ë×´Ì¬
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
