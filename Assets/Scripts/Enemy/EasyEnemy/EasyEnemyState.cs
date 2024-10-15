using UnityEngine;
using static Enemy;

/// <summary>
/// 小怪的基础巡逻状态，所有小怪的巡逻状态继承此状态
/// </summary>
public class EasyEnemyStatePatrol : EnemyState
{
    //private Vector3 targetPoint;
    private float attackTime;
    EasyEnemy easyEnemy;
    //private float stopDuration = 2.0f; // 停留时间
    //private float stopTimer; // 停留计时器
    //private bool movingToA = true; // 当前是否朝A点移动
    //private bool isStopping = false; // 是否正在停留

    public EasyEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, EasyEnemy easyEnemy) : base(enemy, enemyFSM)
    {
        this.easyEnemy = easyEnemy;
    }

    public override void OnEnter()
    {
        attackTime = 1.5f; 
        easyEnemy.stopTimer = 0f; // 初始化停留计时器
        easyEnemy.isStopping = true; // 初始化停留状态
    }

    public override void LogicUpdate()
    {
        attackTime -= Time.deltaTime;
        //// 如果正在停留
        //if (isStopping)
        //{
        //    stopTimer += Time.deltaTime; // 增加停留计时器
        //    if (stopTimer >= stopDuration)
        //    {
        //        // 停留时间结束，切换目标点
        //        movingToA = !movingToA;
        //        targetPoint = movingToA ? enemy.patrolPointA : enemy.patrolPointB;
        //        isStopping = false; // 重置停留状态
        //    }
        //    return; // 停留时不执行其他逻辑
        //}

        //// 检测是否到达目标点
        //if (Vector3.Distance(enemy.transform.position, targetPoint) < 0.1f)
        //{
        //    // 开始停留
        //    isStopping = true;
        //    stopTimer = 0f; // 重置计时器
        //}
    }

    public override void PhysicsUpdate()
    {

        // 检测攻击范围
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
        // 可在退出时做一些清理工作
    }
}


/// <summary>
/// 小怪的基础追击状态，所有小怪追击状态继承此状态
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
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
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
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
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
