using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public EnemyFSM enemyFSM;   // ����״̬��
    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attackState;
    public EnemyState deadState;
    public Rigidbody rb; // �������
    public GameObject player;

    [Tooltip("Ѳ���ٶ�")] public float patrolSpeed;
    [Tooltip("׷���ٶ�")] public float chaseSpeed;
    [Tooltip("��ǰ�ٶ�")] public float currentSpeed;

    [Tooltip("������Χ�������")] public Vector3 attackPoint;
    [Tooltip("��Ұ��Χ�������")] public Vector3 visualPoint;
    [Tooltip("��Ұ��Χ")] public float visualRange;
    [Tooltip("������Χ")] public float attackRange;

    [Tooltip("��Ҳ�")] public LayerMask playerLayer = 1 << 6;
    [Tooltip("�ϰ����")] public LayerMask obstacleLayer = 1 << 7;

    // ����Ѳ�ߵ�
    public GameObject PointA;
    public GameObject PointB;
    public Vector3 patrolPointA;
    public Vector3 patrolPointB;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackPoint, attackRange);   //����������Χ

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + visualPoint, visualRange);   //������Ұ��Χ

        //Gizmos.color = Color.red; // ������ɫΪ��ɫ
        //Gizmos.DrawSphere(patrolPointA, 0.3f); // ���� A ��
        //Gizmos.DrawSphere(patrolPointB, 0.3f); // ���� B ��

        //// ���� A ��� B ��֮�����
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(patrolPointA, patrolPointB);
    }

    /// <summary>
    /// Awake�������ں�������ʼ������״̬��
    /// </summary>
    protected virtual void Awake()
    {
        enemyFSM = new EnemyFSM();   // ��������״̬��ʵ��
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();     // ��ȡ�������
        patrolPointA=PointA.transform.position;
        patrolPointB=PointB.transform.position;
        PointA.SetActive(false);
        PointB.SetActive(false);
    }

    /// <summary>
    /// OnEnable�������ں��������õ���ʱ��ʼ��״̬������ʼִ�е�һ��״̬
    /// </summary>
    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;

        /*��������base.OnEnable()֮ǰΪenemyFSM.startState��ֵ*/
        enemyFSM.InitializeState(enemyFSM.startState);  // ��ʼ������״̬������ʼִ�е�һ��״̬
    }

    /// <summary>
    /// OnDisable�������ں��������õ���ʱִ�е�ǰ״̬��OnExit����
    /// </summary>
    protected virtual void OnDisable()
    {
        enemyFSM.currentState.OnExit(); // ִ�е�ǰ״̬��OnExit����
    }

    protected virtual void Start()
    {

    }

    /// <summary>
    /// FixedUpdate�������ں�����ÿ���̶�ִ֡�е�ǰ״̬��״̬��PhysicsUpdate����
    /// </summary>
    protected virtual void FixedUpdate()
    {
        enemyFSM.currentState.PhysicsUpdate(); // ִ�е�ǰ״̬��״̬��PhysicsUpdate����
    }

    /// <summary>
    /// Update�������ں�����ÿִ֡�е�ǰ״̬��״̬��LogicUpdate����
    /// </summary>
    protected virtual void Update()
    {
        enemyFSM.currentState.LogicUpdate(); // ִ�е�ǰ״̬��״̬��LogicUpdate����
    }
    /// <summary>
    /// ������Χ��ⷽ�� (3D�汾)
    /// </summary>
    /// <returns>����ڹ�����Χ��Ϊtrue������Ϊfalse</returns>
    public bool IsPlayerInAttackRange()
    {
        // ��⹥����Χ���Ƿ������
        return Physics.OverlapSphere(transform.position + attackPoint, attackRange, playerLayer).Length > 0;
    }

    /// <summary>
    /// ��Ұ��Χ��ⷽ�� (3D�汾)
    /// </summary>
    /// <returns>�������Ұ��Χ��Ϊtrue������Ϊfalse</returns>
    public bool IsPlayerInVisualRange()
    {
        // �����Ұ��Χ���Ƿ������
        return Physics.OverlapSphere(transform.position + visualPoint, visualRange, playerLayer).Length > 0;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
