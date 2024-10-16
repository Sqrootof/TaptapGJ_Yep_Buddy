using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void ReciveDamage(float Damage);
}

public interface IKnockBackable
{
    public void BeKnockBack(Vector3 Position, float Force);
}


public class PlayerController : MonoBehaviour,IDamageable,IKnockBackable
{
    [Header("移动属性相关")]
    [SerializeField] float WalkSpeed;
    [SerializeField] float JumpSpeed;
    [SerializeField] float DashForce;
    [SerializeField] float rotationSpeed = 100f; // 旋转速度
    float DashCoolDown = 5f;
    int RemainingJumpTime = 2;

    #region"玩家状态相关"
    private bool InTheAir;
    int FaceDir = 1;//1为右，-1为左
    private float LastDashTime=-10;
    #endregion

    [Header("战斗相关")]
    [SerializeField] float HealthMax;
    [SerializeField] float BasicDefense;
    [SerializeField] float BasicKnockbackResistance;
    [SerializeField] float currentHealth;
    float HealthCurrent {
        get{
            return currentHealth;
        }
        set {
            if (value > HealthMax) {
                currentHealth = HealthMax;
            }
            else if(value <= 0){
                currentHealth = 0;
                StartCoroutine(PlayerDie());
            }
            else{
                currentHealth = value;
            }
        }
    }

    #region"组件"
    [SerializeField] GameObject PlayerBody;
    Rigidbody Rigidbody;
    #endregion

    void Awake()
    {
        ComponentInit();
        DataInit();
    }

    void ComponentInit()
    { 
        Rigidbody = GetComponent<Rigidbody>();
    }

    void DataInit()
    { 
        currentHealth = HealthMax;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        if (Input.GetKeyDown(KeyCode.J)) {
            BeKnockBack(Vector3.zero,12);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) {
            InTheAir = false;
            RemainingJumpTime = 2;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground")) { 
            InTheAir = true;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (RemainingJumpTime > 0) {
                RemainingJumpTime--;
                Rigidbody.AddForce(Vector3.up * JumpSpeed,ForceMode.Impulse);
            }
        }
    }

    private void Move()
    {
        float moveaxis = Input.GetAxisRaw("Horizontal");
        Vector3 MoveVec = new Vector3(moveaxis * WalkSpeed, Rigidbody.velocity.y, 0);
        Rigidbody.velocity = MoveVec;

        if (Input.GetKey(KeyCode.D)){
            FaceDir = 1;
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(0, -45, 0),
                rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.A)) {
            FaceDir = -1;
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(0, 45, 0),
                rotationSpeed * Time.deltaTime);
        }
        else{
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(0, 0, 0),
                rotationSpeed * Time.deltaTime);
        }
    }

    private void Dash()
    {
        if (Input.GetMouseButtonDown(1) && Time.time - LastDashTime >= DashCoolDown) {
            
            Rigidbody.AddForce(DashForce * FaceDir * Vector3.right,ForceMode.Impulse);
        }
    }

    /// <summary>
    /// 接受伤害
    /// </summary>
    /// <param name="Damage">伤害白值</param>
    public void ReciveDamage(float Damage)
    {
        HealthCurrent -= Damage;
    }

    /// <summary>
    /// 受到击退
    /// </summary>
    /// <param name="Position">造成击退的人的位置</param>
    /// <param name="Force">击退力</param>
    public void BeKnockBack(Vector3 Position,float Force)
    {
        Vector3 KonckBackVec = transform.position - Position;
        KonckBackVec.z = 0;
        KonckBackVec.Normalize();
        Rigidbody.AddForce(KonckBackVec,ForceMode.Impulse);
    }

    IEnumerator PlayerDie()
    { 
        yield return null;

        
    }
}