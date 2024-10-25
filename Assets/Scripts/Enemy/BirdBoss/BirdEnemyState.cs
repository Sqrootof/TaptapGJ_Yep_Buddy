using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Enemy;

/// <summary>
/// 巡逻
/// </summary>
public class BirdEnemyStatePatrol : EnemyState
{
    private BirdEnemy birdEnemy;
    bool isStop;
    float time;

    public BirdEnemyStatePatrol(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        isStop = true;
        time = 1f;
    }

    public override void LogicUpdate()
    {
        if (Vector3.Distance(birdEnemy.transform.position, birdEnemy.targetPosition) < 0.1f)
        {
            isStop = true;
        }

        if (isStop==true)
        {
            time += Time.deltaTime;
        }
        if(time>2f)
        {
            birdEnemy.SetRandomTargetPosition();
            isStop = false;
            time = 0f;
        }
    }

    public override void PhysicsUpdate()
    {
        if (isStop==false)
        {
            birdEnemy.MoveToTarget(enemy.patrolSpeed);
        }
    }

    public override void OnExit()
    {

    }
}

/// <summary>
/// 向前激光
/// </summary>
public class BirdEnemyStateChase : EnemyState
{
    private BirdEnemy birdEnemy;
    int num;
    public BirdEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        birdEnemy.SetTargetPosition();
        num = 0;
        birdEnemy.StartLaser();
        birdEnemy.transform.rotation = Quaternion.Euler(0, 0, 50);
        birdEnemy.Square1.SetActive(true);
    }

    public override void LogicUpdate()
    {
        birdEnemy.LaserAttack();
        if(num==0)
        {
            if (Vector3.Distance(birdEnemy.transform.position,birdEnemy.targetPosition)<0.1f)
            {
                num++;
                birdEnemy.target.transform.position=2*birdEnemy.InitialPosition-birdEnemy.targetPosition;
                birdEnemy.targetPosition = birdEnemy.target.transform.position;
            }
        }
        else if(num==1)
        {
            if (Vector3.Distance(birdEnemy.transform.position, birdEnemy.targetPosition) < 0.1f)
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        birdEnemy.MoveToTarget(enemy.patrolSpeed);
    }

    public override void OnExit()
    {
        birdEnemy.FinishLaser();
        birdEnemy.transform.rotation = Quaternion.Euler(0, 0, 0);
        birdEnemy.Square1.SetActive(false);
    }
}

/// <summary>
/// 小怪的基础攻击状态，所有小怪攻击状态继承此状态
/// </summary>
public class BirdEnemyStateAttack : EnemyState
{
    BirdEnemy birdEnemy;
    int num;
    public BirdEnemyStateAttack(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        num = 0;
        birdEnemy.SetPlayerTargetPosition();
    }

    public override void LogicUpdate()
    {
        if(Vector3.Distance(enemy.transform.position,birdEnemy.targetPosition)<0.1f)
        {
            if (num == 0)
            {
                num++;
                //birdEnemy.SetRandomTargetPosition();
                birdEnemy.target.transform.position = birdEnemy.InitialPosition;
            }
            else
            {
                enemyFSM.ChangeState(birdEnemy.laserAttackState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        birdEnemy.MoveToTarget(enemy.chaseSpeed);
    }

    public override void OnExit()
    {
        enemy.rb.velocity = birdEnemy.rb.velocity * 0;
    }
}

//激光攻击
public class BirdEnemyLaserAttackState : EnemyState
{
    BirdEnemy birdEnemy;
    float elapsedTime;
    public BirdEnemyLaserAttackState(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        elapsedTime = 0;
        birdEnemy.StartLaser();
        birdEnemy.Square1.SetActive(true);
        birdEnemy.Square2.SetActive(true);
    }

    public override void LogicUpdate()
    {
        elapsedTime += Time.deltaTime; // 更新经过的时间
        birdEnemy.LaserAttack();
    }

    public override void PhysicsUpdate()
    {

        if (elapsedTime < 2f) // 在5秒内进行旋转
        {
            float t = elapsedTime / 2f; // 计算时间比例（0到1之间）

            // 计算当前的旋转角度（0到360）
            float currentRotationZ = Mathf.Lerp(0f, 160f, t);
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ); // 应用旋转
        }
        else
        {
            enemyFSM.ChangeState(enemy.patrolState);
        }
    }

    public override void OnExit()
    {
        //birdEnemy.target.transform.position = birdEnemy.targetInitialPosition;
        birdEnemy.FinishLaser();
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        birdEnemy.timer = 0f;
        birdEnemy.Square1.SetActive(false);
        birdEnemy.Square2.SetActive(false);
    }
}

//向下激光攻击
public class BirdEnemyUnderLaserAttackState : EnemyState
{
    BirdEnemy birdEnemy;
    int num;
    float rotationSpeed = 15f;
    float time;
    public BirdEnemyUnderLaserAttackState(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        birdEnemy.target.transform.position = birdEnemy.InitialPosition;
        num = 0;
        time = 0f;
    }

    public override void LogicUpdate()
    {
        if (num==0 && Vector3.Distance(birdEnemy.transform.position,birdEnemy.targetPosition)<0.1f)
        {
            num++;
        }
        else if (num==2)
        {
            birdEnemy.StartLaser();
            birdEnemy.LaserAttack();
            birdEnemy.InstantiateSpecialEnd();
            num++;
        }
        if (num==3)
        {
            time += Time.deltaTime;
            if (time>=1f)
            {
                birdEnemy.FinishLaser();
                enemyFSM.ChangeState(enemy.patrolState);
            }
        }
    }

    public override void PhysicsUpdate()
    {
        if (num==0)
        {
            birdEnemy.MoveToTarget(enemy.patrolSpeed);
        }
        else if (num==1)
        {
            // 目标旋转角度
            Quaternion targetRotation = Quaternion.Euler(0, 0, 68);

            // 当前旋转角度
            Quaternion currentRotation = birdEnemy.transform.rotation;

            // 使用 Slerp 进行平滑旋转
            birdEnemy.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            // 可选：检查是否已接近目标角度，若接近则可能修改 num 的值
            if (Quaternion.Angle(currentRotation, targetRotation) < 0.1f) // 1度的容差
            {
                num++;
            }
        }
    }

    public override void OnExit()
    {
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}


public class BirdEnemyBarrageAttackState : EnemyState
{
    BirdEnemy birdEnemy;
    float timer;
    float rotationSpeed = 20f; // 旋转速度（度/秒）
    float rotationDuration = 0.5f; // 旋转时间（秒）
    public float bulletInterval = 0.75f; // 子弹发射间隔时间
    private float bulletTimer = 0f; // 子弹计时器

    private float rotationTimer; // 用于管理旋转和等待的计时器
    private bool rotatingRight; // 旋转方向标记
    int num;
    int q;
    public BirdEnemyBarrageAttackState(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
    {
        this.birdEnemy = birdEnemy;
    }

    public override void OnEnter()
    {
        timer = 0f;
        rotationTimer = 0f;
        rotatingRight = true; // 从右侧开始旋转
        num = 0;
        q = Random.Range(0, 2);
    }

    public override void LogicUpdate()
    {
        if(q==0)
        {
            timer += Time.deltaTime;

            // 检查是否超过攻击持续时间，若是则返回巡逻状态
            if (timer > 10f)
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }

            // 处理旋转行为
            rotationTimer += Time.deltaTime;

            if (rotationTimer < rotationDuration)
            {
                float angle = rotatingRight ? rotationSpeed * Time.deltaTime : -rotationSpeed * Time.deltaTime;
                birdEnemy.transform.Rotate(0, 0, angle);
                num = 0;
            }
            else
            {
                if (rotationTimer >= rotationDuration + 0.5f)
                {
                    rotatingRight = !rotatingRight;
                    rotationTimer = 0f; // 重置旋转计时器
                    if (num==0)
                    {
                        birdEnemy.FireBullets();
                        num++;
                    }
                }
            }
        }
        else
        {
            timer += Time.deltaTime;
            bulletTimer += Time.deltaTime;
            rotationSpeed = 30f; // 旋转速度（度/秒）
            // 检查是否超过攻击持续时间，若是则返回巡逻状态
            if (timer > 10f)
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }

            // 处理持续旋转行为
            float angle = rotatingRight ? rotationSpeed * Time.deltaTime : -rotationSpeed * Time.deltaTime;
            birdEnemy.transform.Rotate(0, 0, angle);

            // 控制发射子弹的逻辑
            if (bulletTimer >= bulletInterval) // 根据设定的间隔发射子弹
            {
                birdEnemy.FireBullets();
                bulletTimer = 0f; // 重置计时器
            }
        }

    }

    public override void PhysicsUpdate()
    {
        // 在这里可以处理物理相关的更新
    }

    public override void OnExit()
    {
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}


/// <summary>
/// 小怪的基础死亡状态，所有小怪死亡状态继承此状态
/// </summary>
public class BirdEnemyStateDead : EnemyState
{
    public BirdEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
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
