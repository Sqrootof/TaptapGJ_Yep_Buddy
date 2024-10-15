using UnityEngine;
using static Enemy;
/// <summary>
/// Ð¡¹ÖµÄ»ù´¡Ñ²Âß×´Ì¬£¬ËùÓÐÐ¡¹ÖµÄÑ²Âß×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class EasyEnemyStatePatrol : EnemyState
{
    EasyEnemy easyEnemy;
    float attackTime;
    public EasyEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {
        this.easyEnemy = easyEnemy;
    }

    public override void OnEnter()
    {
        attackTime = 1.5f;
        Debug.Log(2);
    }

    public override void LogicUpdate()
    {
        attackTime -= Time.deltaTime;
    }

    public override void PhysicsUpdate()
    {
        if (enemy.IsPlayerInAttackRange())
        {
            if (attackTime <=0f)
            {
                easyEnemy.TryAttack();
                attackTime = 1.5f;
            }
        }
    }

    public override void OnExit()
    {
        Debug.Log(3);
    }
}


/// <summary>
/// Ð¡¹ÖµÄ»ù´¡×·»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö×·»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
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
/// Ð¡¹ÖµÄ»ù´¡¹¥»÷×´Ì¬£¬ËùÓÐÐ¡¹Ö¹¥»÷×´Ì¬¼Ì³Ð´Ë×´Ì¬
/// </summary>
public class EasyEnemyStateAttack : EnemyState
{
    public EasyEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
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
/// Ð¡¹ÖµÄ»ù´¡ËÀÍö×´Ì¬£¬ËùÓÐÐ¡¹ÖËÀÍö×´Ì¬¼Ì³Ð´Ë×´Ì¬
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
