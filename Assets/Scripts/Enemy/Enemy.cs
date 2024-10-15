using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float maxHealth;
    public float currentHealth;
    public EnemyFSM enemyFSM;   // 敌人状态机
    public EnemyState patrolState;
    public EnemyState chaseState;
    public EnemyState attackState;
    public EnemyState deadState;
    public Rigidbody rb; // 刚体组件
    public GameObject player;

    [Tooltip("巡逻速度")] public float patrolSpeed;
    [Tooltip("追击速度")] public float chaseSpeed;
    [Tooltip("当前速度")] public float currentSpeed;

    [Tooltip("攻击范围检测中心")] public Vector3 attackPoint;
    [Tooltip("视野范围检测中心")] public Vector3 visualPoint;
    [Tooltip("视野范围")] public float visualRange;
    [Tooltip("攻击范围")] public float attackRange;

    [Tooltip("玩家层")] public LayerMask playerLayer = 1 << 6;
    [Tooltip("障碍物层")] public LayerMask obstacleLayer = 1 << 7;

    // 定义巡逻点
    public GameObject PointA;
    public GameObject PointB;
    public Vector3 patrolPointA;
    public Vector3 patrolPointB;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + attackPoint, attackRange);   //画出攻击范围

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + visualPoint, visualRange);   //画出视野范围

        //Gizmos.color = Color.red; // 设置颜色为红色
        //Gizmos.DrawSphere(patrolPointA, 0.3f); // 绘制 A 点
        //Gizmos.DrawSphere(patrolPointB, 0.3f); // 绘制 B 点

        //// 绘制 A 点和 B 点之间的线
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(patrolPointA, patrolPointB);
    }

    /// <summary>
    /// Awake生命周期函数，初始化敌人状态机
    /// </summary>
    protected virtual void Awake()
    {
        enemyFSM = new EnemyFSM();   // 创建敌人状态机实例
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();     // 获取刚体组件
        patrolPointA=PointA.transform.position;
        patrolPointB=PointB.transform.position;
        PointA.SetActive(false);
        PointB.SetActive(false);
    }

    /// <summary>
    /// OnEnable生命周期函数，启用敌人时初始化状态机并开始执行第一个状态
    /// </summary>
    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;

        /*子类中在base.OnEnable()之前为enemyFSM.startState赋值*/
        enemyFSM.InitializeState(enemyFSM.startState);  // 初始化敌人状态机并开始执行第一个状态
    }

    /// <summary>
    /// OnDisable生命周期函数，禁用敌人时执行当前状态的OnExit函数
    /// </summary>
    protected virtual void OnDisable()
    {
        enemyFSM.currentState.OnExit(); // 执行当前状态的OnExit函数
    }

    protected virtual void Start()
    {

    }

    /// <summary>
    /// FixedUpdate生命周期函数，每个固定帧执行当前状态机状态的PhysicsUpdate函数
    /// </summary>
    protected virtual void FixedUpdate()
    {
        enemyFSM.currentState.PhysicsUpdate(); // 执行当前状态机状态的PhysicsUpdate函数
    }

    /// <summary>
    /// Update生命周期函数，每帧执行当前状态机状态的LogicUpdate函数
    /// </summary>
    protected virtual void Update()
    {
        enemyFSM.currentState.LogicUpdate(); // 执行当前状态机状态的LogicUpdate函数
    }
    /// <summary>
    /// 攻击范围检测方法 (3D版本)
    /// </summary>
    /// <returns>玩家在攻击范围内为true，否则为false</returns>
    public bool IsPlayerInAttackRange()
    {
        // 检测攻击范围内是否有玩家
        return Physics.OverlapSphere(transform.position + attackPoint, attackRange, playerLayer).Length > 0;
    }

    /// <summary>
    /// 视野范围检测方法 (3D版本)
    /// </summary>
    /// <returns>玩家在视野范围内为true，否则为false</returns>
    public bool IsPlayerInVisualRange()
    {
        // 检测视野范围内是否有玩家
        return Physics.OverlapSphere(transform.position + visualPoint, visualRange, playerLayer).Length > 0;
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
