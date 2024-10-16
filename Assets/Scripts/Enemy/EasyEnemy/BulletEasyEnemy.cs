using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEasyEnemy : MonoBehaviour
{
    private float timer = 0f;
    public float destroyTime = 5f;
    public float damage;
    // Update is called once per frame
    void Update()
    {
        // 更新计时器
        timer += Time.deltaTime;

        // 检查计时器是否超过设定时间
        if (timer >= destroyTime)
        {
            // 摧毁当前物体
            Destroy(gameObject);
        }
    }

    // 碰撞检测方法
    private void OnTriggerEnter(Collider other)
    {
        // 检查碰撞的物体是否是玩家
        if (other.CompareTag("Player"))
        {
            // 获取玩家脚本组件
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.GetHit(damage);
            }

            Destroy(gameObject);
        }
    }
}
