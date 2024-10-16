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
        // ���¼�ʱ��
        timer += Time.deltaTime;

        // ����ʱ���Ƿ񳬹��趨ʱ��
        if (timer >= destroyTime)
        {
            // �ݻٵ�ǰ����
            Destroy(gameObject);
        }
    }

    // ��ײ��ⷽ��
    private void OnTriggerEnter(Collider other)
    {
        // �����ײ�������Ƿ������
        if (other.CompareTag("Player"))
        {
            // ��ȡ��ҽű����
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.GetHit(damage);
            }

            Destroy(gameObject);
        }
    }
}
