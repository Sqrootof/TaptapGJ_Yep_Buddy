using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : ProjectileHandler
{
    [Header("组件")]
    LineRenderer LineRenderer;
    [SerializeField] GameObject ExplosionPrefab; // 激光击中目标的爆炸特效

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ObstacleLayer; // 用于检测的障碍物图层
    [SerializeField] LayerMask EnemyLayer; // 用于检测敌人的图层
    [SerializeField] LayerMask EnemyShieldLayer;

    private void Awake()
    {
        base.Awake();    
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    protected override void ComponentInit()
    {
        base.ComponentInit();
        LineRenderer = transform.GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public LaserHandler(Projectile Projectile) : base(Projectile){
        
    }

    public override void BeShoot(Vector3 StartPos, Vector3 MousePos)
    {
        base.BeShoot(StartPos, MousePos);
        StartPos.z = 0;
        MousePos.z = 0;
        Vector3 Pointdir = (MousePos - StartPos).normalized;

        RaycastHit hit;
        int reflection = 0;

        LineRenderer.positionCount = 1;
        LineRenderer.SetPosition(0, StartPos);

        Vector3 currentPos = StartPos;
        Vector3 currentDir = Pointdir;

        while (reflection <= (ProjectileData as Laser).ReflectionTimes)
        {
            if (Physics.Raycast(currentPos, currentDir, out hit, (ProjectileData as Laser).LaserLength, ObstacleLayer | EnemyLayer | EnemyShieldLayer | GroundLayer))
            {
                LineRenderer.positionCount++;
                LineRenderer.SetPosition(reflection + 1, hit.point);

                // 检测是否击中敌人
                if ((EnemyLayer & (1 << hit.collider.gameObject.layer)) != 0)
                {
                    var enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemy)
                    {
                        enemy.currentHealth -= ProjectileData.Damage;
                        Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
                    }

                    reflection++;
                    currentPos = hit.point + currentDir.normalized * 0.01f;
                    continue;
                }
                else
                {
                    // 没有击中敌人，反射
                    Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
                    currentDir = Vector3.Reflect(currentDir.normalized, hit.normal); // 计算反射方向
                    currentPos = hit.point; // 更新反射起点
                    reflection++; // 增加反射次数
                }
            }
            else {
                LineRenderer.positionCount++;
                LineRenderer.SetPosition(reflection + 1, currentPos + currentDir.normalized * (ProjectileData as Laser).LaserLength);
                break;  
            }
        }
        OnProjectileDestroy += DestroyLaser;
        DestroyProjectile();
    }

    IEnumerator DestroyLaser()
    {
        yield return new WaitForEndOfFrame();

        List<int> cur = new List<int>(){0};
        int next = 1;
    
        while (next < LineRenderer.positionCount)
        {
            float distance = Vector3.Distance(LineRenderer.GetPosition(0), LineRenderer.GetPosition(next));
            while (distance > 0.01f)
            {
                distance = Vector3.Distance(LineRenderer.GetPosition(0), LineRenderer.GetPosition(next));
                Vector3 newPos = Vector3.MoveTowards(LineRenderer.GetPosition(0), LineRenderer.GetPosition(next), Time.deltaTime * (ProjectileData as Laser).DisappearSpeed);
                for (int i=0;i < cur.Count ; i++)
                {
                    LineRenderer.SetPosition(i,newPos);
                }
                yield return null;
            }
            for (int i = 0; i < cur.Count; i++)
            {
                LineRenderer.SetPosition(i, LineRenderer.GetPosition(next));
            }
            cur.Add(next++);
        }

        DestroyCoroutine = null;
    }

}
