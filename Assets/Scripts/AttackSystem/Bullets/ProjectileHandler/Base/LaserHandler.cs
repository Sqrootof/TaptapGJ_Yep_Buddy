using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHandler : ProjectileHandler
{
    [Header("���")]
    LineRenderer LineRenderer;
    [SerializeField] GameObject ExplosionPrefab; // �������Ŀ��ı�ը��Ч

    [SerializeField] LayerMask GroundLayer;
    [SerializeField] LayerMask ObstacleLayer; // ���ڼ����ϰ���ͼ��
    [SerializeField] LayerMask EnemyLayer; // ���ڼ����˵�ͼ��
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

                // ����Ƿ���е���
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
                    // û�л��е��ˣ�����
                    Instantiate(ExplosionPrefab, hit.point, Quaternion.identity);
                    currentDir = Vector3.Reflect(currentDir.normalized, hit.normal); // ���㷴�䷽��
                    currentPos = hit.point; // ���·������
                    reflection++; // ���ӷ������
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
