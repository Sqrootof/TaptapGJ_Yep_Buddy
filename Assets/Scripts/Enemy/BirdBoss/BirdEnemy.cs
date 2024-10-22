using Unity.Burst.CompilerServices;
using UnityEngine;

public class BirdEnemy : Enemy
{
    public Enemy enemy; // Enemy 实例引用
    public EnemyLaser enemyLaser;

    public GameObject target;
    public GameObject targetLaser;
    public GameObject Square1;
    public GameObject Square2;
    public GameObject SpecialEnd;
    public GameObject bullet;

    public EnemyState laserAttackState;
    public EnemyState underLaserAttackState;
    public EnemyState barrageAttackState;

    public Vector3 targetPosition;
    public Vector3 targetInitialPosition;
    public Vector3 InitialPosition;

    public float rotationSpeed = 5f; // 旋转速度
    public float bulletSpeed;
    public float timer = 0f;
    public float damage;
    public float force = 2f;

    public bool showGizmos = false; // 允许在Inspector中控制是否显示Gizmos
    // 设置巡逻范围的大小
    public Vector3 patrolAreaSize = new Vector3(5f, 5f, 0f); // X, Y, Z 轴的大小

    public void FinishAttack()
    {
        enemyFSM.ChangeState(patrolState);
    }

    protected override void Awake()
    {
        base.Awake();
        enemy=GetComponent<Enemy>();
        targetInitialPosition = target.transform.position;
        enemyLaser = GetComponent<EnemyLaser>();
        patrolState = new BirdEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new BirdEnemyStateChase(this, enemyFSM, this);
        attackState = new BirdEnemyStateAttack(this, enemyFSM, this);
        deadState = new BirdEnemyStateDead(this, enemyFSM, this);
        laserAttackState = new BirdEnemyLaserAttackState(this, enemyFSM, this);
        underLaserAttackState=new BirdEnemyUnderLaserAttackState(this, enemyFSM, this);
        barrageAttackState=new BirdEnemyBarrageAttackState(this, enemyFSM, this);

        InitialPosition =transform.position;
        FinishLaser();
    }

    public void LaserAttack()
    {
        enemyLaser.UpdateLaser(targetLaser.transform.position);
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
        base.Start();
        timer = 0f;
    }

    protected override void Update()
    {
        if (currentHealth<=0f)
        {
            enemyFSM.ChangeState(deadState); return;
        }
        targetPosition = target.transform.position;
        if (enemyFSM.currentState == patrolState) // Replace `patrolState` with your patrol state variable or reference
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                Bullets();
                timer = 0f;
                if (currentHealth>maxHealth/2)
                {
                    int p=Random.Range(0,4);
                    if (p==0)
                    {
                        enemyFSM.ChangeState(attackState); return;
                    }
                    else if (p==1)
                    {
                        enemyFSM.ChangeState(underLaserAttackState);
                        return;
                    }
                    else if(p==2)
                    {
                        enemyFSM.ChangeState(chaseState); return;
                    }
                    else
                    {
                        enemyFSM.ChangeState(barrageAttackState); return;
                    }
                }
                else
                {
                    int p = Random.Range(0, 5);
                    if (p == 0)
                    {
                        enemyFSM.ChangeState(attackState); return;
                    }
                    else if (p == 1)
                    {
                        enemyFSM.ChangeState(underLaserAttackState);
                        return;
                    }
                    else if(p == 2 || p==4)
                    {
                        enemyFSM.ChangeState(barrageAttackState); return;
                    }
                    else if (p == 3)
                    {
                        enemyFSM.ChangeState(chaseState); return;
                    }
                }
            }
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public void StartLaser()
    {
        enemyLaser.Line.SetActive(true);
    }

    public void FinishLaser()
    {
        enemyLaser.Line.SetActive(false);
    }
    // 在场景视图中绘制巡逻范围
    private void OnDrawGizmos()
    {
        if (!showGizmos) return; // 如果不想显示，直接返回

        // 获取敌人的当前坐标
        Vector3 position = InitialPosition;

        // 设置Gizmos的颜色
        Gizmos.color = Color.green;

        // 绘制一个立方体表示巡逻范围
        Gizmos.DrawCube(position, patrolAreaSize);
    }

    // 更新目标位置为巡逻范围内的随机位置
    public void SetRandomTargetPosition()
    {
        // 获取敌人的当前坐标
        Vector3 position = InitialPosition;

        // 计算随机位置，基于巡逻范围
        float randomX = Random.Range(position.x - patrolAreaSize.x / 2, position.x + patrolAreaSize.x / 2);
        float randomY = Random.Range(position.y - patrolAreaSize.y / 2, position.y + patrolAreaSize.y / 2);

        // 将目标位置更新为新的随机位置
        target.transform.position = new Vector3(randomX, randomY, 0); // Z轴保持为当前敌人Z坐标
        targetPosition = target.transform.position; // 更新targetPosition
    }

    public void SetTargetPosition()
    {
        // 获取敌人的当前坐标
        Vector3 position = InitialPosition;

        // 计算随机位置，基于巡逻范围
        int pp = Random.Range(0, 2);
        float randomX;
        if (pp == 0)
        {
            randomX = position.x - patrolAreaSize.x / 2;
        }
        else
        {
            randomX = position.x + patrolAreaSize.x / 2;
        }

        float randomY = Random.Range(position.y - patrolAreaSize.y / 2, position.y + patrolAreaSize.y / 2);

        // 将目标位置更新为新的随机位置
        target.transform.position = new Vector3(randomX, randomY, 0); // Z轴保持为当前敌人Z坐标
        targetPosition = target.transform.position; // 更新targetPosition
    }

    public void SetPlayerTargetPosition()
    {
        target.transform.position = enemy.player.transform.position;
        targetPosition = target.transform.position;
    }

    public void MoveToTarget(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }


    public void InstantiateSpecialEnd()
    {
        Instantiate(SpecialEnd, enemyLaser.end, Quaternion.identity);
    }

    public void FireBullets()
    {
        // 获取当前敌人的旋转角度
        float currentRotationZ = gameObject.transform.eulerAngles.z;

        for (int i = 0; i < 18; i++)
        {

            // 计算当前角度
            float angleOffset = i * 20f; // 每个子弹的发射角度偏移
            float angle = currentRotationZ + angleOffset; // 基于当前旋转加上偏移

            // 计算方向向量（只在XY平面上）
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // 创建子弹
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
        }
    }

    public void Bullets()
    {
        // 计算朝向玩家的方向
        Vector3 direction = (enemy.player.transform.position - transform.position).normalized;

        // 实例化子弹
        GameObject bullets = Instantiate(bullet, transform.position, Quaternion.identity);

        // 获取子弹的 Rigidbody 组件
        Rigidbody bulletRigidbody = bullets.GetComponent<Rigidbody>();

        // 确保子弹存在 Rigidbody 组件
        if (bulletRigidbody != null)
        {
            bulletRigidbody.velocity = direction * bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController Health = other.GetComponent<PlayerController>();
            if (Health != null)
            {
                Health.ReciveDamage(damage);
                Health.BeKnockBack(transform.position, force);
            }
        }
    }

}
