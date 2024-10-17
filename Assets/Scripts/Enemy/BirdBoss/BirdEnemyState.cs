using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using static Enemy;

/// <summary>
/// С�ֵĻ���Ѳ��״̬������С�ֵ�Ѳ��״̬�̳д�״̬
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
/// С�ֵĻ���׷��״̬������С��׷��״̬�̳д�״̬
/// </summary>
public class BirdEnemyStateChase : EnemyState
{

    public BirdEnemyStateChase(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
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

//���⹥��
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
        elapsedTime += Time.deltaTime; // ���¾�����ʱ��
        birdEnemy.LaserAttack();
    }

    public override void PhysicsUpdate()
    {

        if (elapsedTime < 2f) // ��5���ڽ�����ת
        {
            float t = elapsedTime / 2f; // ����ʱ�������0��1֮�䣩

            // ���㵱ǰ����ת�Ƕȣ�0��360��
            float currentRotationZ = Mathf.Lerp(0f, 160f, t);
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ); // Ӧ����ת
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

//���¼��⹥��
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
            // Ŀ����ת�Ƕ�
            Quaternion targetRotation = Quaternion.Euler(0, 0, 68);

            // ��ǰ��ת�Ƕ�
            Quaternion currentRotation = birdEnemy.transform.rotation;

            // ʹ�� Slerp ����ƽ����ת
            birdEnemy.transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * rotationSpeed);

            // ��ѡ������Ƿ��ѽӽ�Ŀ��Ƕȣ����ӽ�������޸� num ��ֵ
            if (Quaternion.Angle(currentRotation, targetRotation) < 0.1f) // 1�ȵ��ݲ�
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
    float rotationSpeed = 20f; // ��ת�ٶȣ���/�룩
    float rotationDuration = 0.5f; // ��תʱ�䣨�룩
    public float bulletInterval = 0.75f; // �ӵ�������ʱ��
    private float bulletTimer = 0f; // �ӵ���ʱ��

    private float rotationTimer; // ���ڹ�����ת�͵ȴ��ļ�ʱ��
    private bool rotatingRight; // ��ת������
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
        rotatingRight = true; // ���Ҳ࿪ʼ��ת
        num = 0;
        q = Random.Range(0, 2);
    }

    public override void LogicUpdate()
    {
        if(q==0)
        {
            timer += Time.deltaTime;

            // ����Ƿ񳬹���������ʱ�䣬�����򷵻�Ѳ��״̬
            if (timer > 10f)
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }

            // ������ת��Ϊ
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
                    rotationTimer = 0f; // ������ת��ʱ��
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
            rotationSpeed = 30f; // ��ת�ٶȣ���/�룩
            // ����Ƿ񳬹���������ʱ�䣬�����򷵻�Ѳ��״̬
            if (timer > 10f)
            {
                enemyFSM.ChangeState(enemy.patrolState);
            }

            // ���������ת��Ϊ
            float angle = rotatingRight ? rotationSpeed * Time.deltaTime : -rotationSpeed * Time.deltaTime;
            birdEnemy.transform.Rotate(0, 0, angle);

            // ���Ʒ����ӵ����߼�
            if (bulletTimer >= bulletInterval) // �����趨�ļ�������ӵ�
            {
                birdEnemy.FireBullets();
                bulletTimer = 0f; // ���ü�ʱ��
            }
        }

    }

    public override void PhysicsUpdate()
    {
        // ��������Դ���������صĸ���
    }

    public override void OnExit()
    {
        enemy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}


/// <summary>
/// С�ֵĻ�������״̬������С������״̬�̳д�״̬
/// </summary>
public class BirdEnemyStateDead : EnemyState
{
    public BirdEnemyStateDead(Enemy enemy, EnemyFSM enemyFSM, BirdEnemy birdEnemy) : base(enemy, enemyFSM)
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
