using Unity.Burst.CompilerServices;
using UnityEngine;

public class BirdEnemy : Enemy
{
    public Enemy enemy; // Enemy ʵ������
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

    public float rotationSpeed = 5f; // ��ת�ٶ�
    public float bulletSpeed;
    public float timer = 0f;
    public float damage;
    public float force = 2f;

    public bool showGizmos = false; // ������Inspector�п����Ƿ���ʾGizmos
    // ����Ѳ�߷�Χ�Ĵ�С
    public Vector3 patrolAreaSize = new Vector3(5f, 5f, 0f); // X, Y, Z ��Ĵ�С

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
    // �ڳ�����ͼ�л���Ѳ�߷�Χ
    private void OnDrawGizmos()
    {
        if (!showGizmos) return; // ���������ʾ��ֱ�ӷ���

        // ��ȡ���˵ĵ�ǰ����
        Vector3 position = InitialPosition;

        // ����Gizmos����ɫ
        Gizmos.color = Color.green;

        // ����һ���������ʾѲ�߷�Χ
        Gizmos.DrawCube(position, patrolAreaSize);
    }

    // ����Ŀ��λ��ΪѲ�߷�Χ�ڵ����λ��
    public void SetRandomTargetPosition()
    {
        // ��ȡ���˵ĵ�ǰ����
        Vector3 position = InitialPosition;

        // �������λ�ã�����Ѳ�߷�Χ
        float randomX = Random.Range(position.x - patrolAreaSize.x / 2, position.x + patrolAreaSize.x / 2);
        float randomY = Random.Range(position.y - patrolAreaSize.y / 2, position.y + patrolAreaSize.y / 2);

        // ��Ŀ��λ�ø���Ϊ�µ����λ��
        target.transform.position = new Vector3(randomX, randomY, 0); // Z�ᱣ��Ϊ��ǰ����Z����
        targetPosition = target.transform.position; // ����targetPosition
    }

    public void SetTargetPosition()
    {
        // ��ȡ���˵ĵ�ǰ����
        Vector3 position = InitialPosition;

        // �������λ�ã�����Ѳ�߷�Χ
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

        // ��Ŀ��λ�ø���Ϊ�µ����λ��
        target.transform.position = new Vector3(randomX, randomY, 0); // Z�ᱣ��Ϊ��ǰ����Z����
        targetPosition = target.transform.position; // ����targetPosition
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
        // ��ȡ��ǰ���˵���ת�Ƕ�
        float currentRotationZ = gameObject.transform.eulerAngles.z;

        for (int i = 0; i < 18; i++)
        {

            // ���㵱ǰ�Ƕ�
            float angleOffset = i * 20f; // ÿ���ӵ��ķ���Ƕ�ƫ��
            float angle = currentRotationZ + angleOffset; // ���ڵ�ǰ��ת����ƫ��

            // ���㷽��������ֻ��XYƽ���ϣ�
            Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0);

            // �����ӵ�
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
        // ���㳯����ҵķ���
        Vector3 direction = (enemy.player.transform.position - transform.position).normalized;

        // ʵ�����ӵ�
        GameObject bullets = Instantiate(bullet, transform.position, Quaternion.identity);

        // ��ȡ�ӵ��� Rigidbody ���
        Rigidbody bulletRigidbody = bullets.GetComponent<Rigidbody>();

        // ȷ���ӵ����� Rigidbody ���
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
