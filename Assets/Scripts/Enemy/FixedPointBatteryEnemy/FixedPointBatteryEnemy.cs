using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FixedPointBatteryEnemy : Enemy
{
    public GameObject bullet;
    public float bulletSpeed;
    public float attackTime;
    float timer;
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnEnable()
    {
        currentHealth = maxHealth;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }

    protected override void Start()
    {
        timer = 0f;
        base.Start();
    }

    protected override void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
            return;
        }
        timer += Time.deltaTime;
        if (timer > attackTime)
        {
            FireBullets();
            timer= 0f;
        }
    }

    protected override void FixedUpdate()
    {

    }

    public void FireBullets()
    {
        for (int i = 0; i < 20; i++)
        {
            // 计算当前角度
            float angleOffset = i * 18f; // 每个子弹的发射角度偏移
            // 计算方向向量（只在XY平面上）
            Vector3 direction = new Vector3(Mathf.Cos(angleOffset*Mathf.Deg2Rad), Mathf.Sin(angleOffset * Mathf.Deg2Rad), 0);

            // 创建子弹
            GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            Rigidbody bulletRigidbody = newBullet.GetComponent<Rigidbody>();
            if (bulletRigidbody != null)
            {
                bulletRigidbody.velocity = direction * bulletSpeed;
            }
        }
    }
}
