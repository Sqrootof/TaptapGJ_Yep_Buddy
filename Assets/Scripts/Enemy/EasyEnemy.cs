using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class EasyEnemy : Enemy
{
    public Enemy enemy; // Enemy ʵ������
    public GameObject bulletPrefab; // �ӵ���Ԥ����
    public float bulletSpeed; // �ӵ��ٶ�
    public bool bullet;
    // ����������������Թ���
    public void TryAttack()
    {
        Vector3 playerPosition = enemy.player.transform.position;
        GameObject bulletInstance = Instantiate(bulletPrefab, gameObject.transform.position, Quaternion.identity);
        Vector2 bulletDirection = (playerPosition - gameObject.transform.position).normalized;
        bulletInstance.GetComponent<Rigidbody>().velocity = bulletDirection * bulletSpeed;
    }

    public void FinishAttack()
    {
        enemyFSM.ChangeState(patrolState);
    }
    protected override void Awake()
    {
        base.Awake();
        patrolState = new EasyEnemyStatePatrol(this, enemyFSM, this);
        chaseState = new EasyEnemyStateChase(this, enemyFSM, this);
        attackState = new EasyEnemyStateAttack(this, enemyFSM, this);
        deadState = new EasyEnemyStateDead(this, enemyFSM, this);
        enemyFSM.startState = patrolState;
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
        enemy = FindObjectOfType<Enemy>();
        base.Start();
    }

    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            enemyFSM.ChangeState(deadState);
            return;
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
