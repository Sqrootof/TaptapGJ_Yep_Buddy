using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnd : MonoBehaviour
{
    public GameObject bullet; // 子弹预制体
    public float bulletSpeed;  // 子弹速度

    // Start is called before the first frame update
    void Start()
    {
        // 发射子弹
        FireBullets();
        Destroy(gameObject); // 销毁自己
    }

    // 发射36个子弹，每个方向相隔10度
    void FireBullets()
    {
        for (int i = 0; i < 18; i++)
        {
            // 计算当前角度
            float angle = i * 20f; // 15度间隔

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
}
