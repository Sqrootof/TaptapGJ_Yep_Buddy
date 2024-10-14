using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPYPTest : MonoBehaviour
{
    public float moveSpeed = 5f; // 玩家移动速度
    public float jumpForce = 5f; // 跳跃力度
    private Rigidbody rb; // 3D 刚体组件
    private bool isGrounded; // 判断玩家是否在地面

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 获取刚体组件
    }

    // Update is called once per frame
    void Update()
    {
        Move(); // 调用移动方法
        Jump(); // 调用跳跃方法
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal"); // 获取水平输入
        Vector3 move = new Vector3(moveInput, 0, 0); // 创建移动向量
        rb.velocity = new Vector3(move.x * moveSpeed, rb.velocity.y, rb.velocity.z); // 更新刚体速度
    }

    private void Jump()
    {
        // 检测空格键是否按下，并且玩家在地面上
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // 给玩家一个向上的力
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测与地面的碰撞
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // 玩家在地面上
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // 离开地面时设置为不在地面
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // 玩家不在地面上
        }
    }
}
