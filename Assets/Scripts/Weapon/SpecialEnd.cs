using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEnd : MonoBehaviour
{
    public GameObject bullet; // �ӵ�Ԥ����
    public float bulletSpeed;  // �ӵ��ٶ�

    // Start is called before the first frame update
    void Start()
    {
        // �����ӵ�
        FireBullets();
        Destroy(gameObject); // �����Լ�
    }

    // ����36���ӵ���ÿ���������10��
    void FireBullets()
    {
        for (int i = 0; i < 18; i++)
        {
            // ���㵱ǰ�Ƕ�
            float angle = i * 20f; // 15�ȼ��

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
}
