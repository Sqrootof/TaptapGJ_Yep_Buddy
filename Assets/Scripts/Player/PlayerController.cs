using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Pool;
using UnityEngine.UI;

public interface IDamageable
{
    public void ReciveDamage(float Damage);
}

public interface IKnockBackable
{
    public void BeKnockBack(Vector3 Position, float Force);
}


public class PlayerController : TIntance<PlayerController>,IDamageable,IKnockBackable
{
    [Header("�ƶ��������")]
    [SerializeField] float WalkSpeed;
    [SerializeField] float JumpSpeed;
    [SerializeField] float rotationSpeed = 100f; // ��ת�ٶ�
    [SerializeField] ParticleSystem WalkParticle;
    int RemainingJumpTime = 2;

    [Header("������")]
    [SerializeField] float DashSpeed;
    [SerializeField] float DashCoolDown = 5f;
    [SerializeField] float DashInterval = 0.8f;
    [SerializeField] ParticleSystem DashParticle;
    [SerializeField] LayerMask DashStop;
    bool isDashing = false;
    

    #region"���״̬���"
    [SerializeField]private bool InTheAir;
    int FaceDir = 1;//1Ϊ�ң�-1Ϊ��
    private float LastDashTime=-10;
    [SerializeField] Image HealthBar;
    #endregion

    [Header("ս�����")]
    [SerializeField] float HealthMax;
    [SerializeField] float BasicDefense;
    [SerializeField] float BasicKnockbackResistance;
    [SerializeField] float currentHealth;

    [Header("��������")]
    [SerializeField] GameObject DeadUI;
    public GameObject quit;
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
                quit.GetComponent<SceneChoice>().SceneAltWithDelay();
            }
            else{
                currentHealth = value;
            }
        }
    }

    #region"���"
    [SerializeField] GameObject PlayerBody;
    Rigidbody Rigidbody;
    Animator Animator;
    #endregion

    [SerializeField] GameObject Head;

    void Awake()
    {
        base.Awake();
        ComponentInit();
        DataInit();
    }

    void ComponentInit()
    { 
        Rigidbody = GetComponent<Rigidbody>();
        Animator = GetComponent<Animator>();
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
        LookAtMouse();
        Move();
        Jump();
        Dash();
        HealthBar.fillAmount = HealthCurrent / HealthMax;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && collision.transform.position .y < transform.position.y) {
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
    void LookAtMouse()
    {
        Vector3 MousePos = Input.mousePosition;
        MousePos.z = Camera.main.transform.position.z;
        Vector3 WorldPos = Camera.main.ScreenToWorldPoint(MousePos);
        WorldPos.z = -Camera.main.transform.position.z;
        Head.transform.LookAt(WorldPos);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (RemainingJumpTime > 0) {
                RemainingJumpTime--;
                //���ٶ�
                Rigidbody.AddForce(Vector3.up * JumpSpeed,ForceMode.Impulse);
            }
        }
    }

    private void Move()
    {
        float moveaxis = Input.GetAxisRaw("Horizontal");
        Vector3 MoveVec = new Vector3(moveaxis * WalkSpeed, Rigidbody.velocity.y, 0);
        Rigidbody.velocity = MoveVec;

        if (Input.GetKey(KeyCode.D))
        {
            FaceDir = 1;
            Animator.SetBool("Walking", true);
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(-5, -45, 0),
                rotationSpeed * Time.deltaTime);
            if (!InTheAir){
                WalkParticle.Play();
            }
            else WalkParticle.Stop();
        }
        else if (Input.GetKey(KeyCode.A))
        {
            FaceDir = -1;
            Animator.SetBool("Walking", true);
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(5, 45, 0),
                rotationSpeed * Time.deltaTime);
            if (!InTheAir){
                WalkParticle.Play();
                WalkParticle.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else WalkParticle.Stop();
        }
        else
        {
            PlayerBody.transform.rotation = Quaternion.RotateTowards(
                PlayerBody.transform.rotation,
                Quaternion.Euler(0, 0, 0),
                rotationSpeed * Time.deltaTime);
            WalkParticle.Stop();
            Animator.SetBool("Walking",false);
        }
        transform.rotation = Quaternion.identity;
    }

    private void Dash()
    {
        if (Input.GetMouseButtonDown(1) && Time.time - LastDashTime >= DashCoolDown) {
            LastDashTime = Time.time;
            StartCoroutine(dash());
        }
    }

    IEnumerator dash()
    {
        DashParticle.Play();
        if(FaceDir > 0) DashParticle.transform.rotation = Quaternion.Euler(0, 0, 180);
        else DashParticle.transform.rotation = Quaternion.Euler(0, 0, 0);
        StartCoroutine(dash_phy());
        
        //var list = new List<GameObject>();
        //int count = 1;
        //while (count <= ShadowCount) {
        //    var newDashShadow = DashShadowPool.Get();
        //    newDashShadow.name = count.ToString();
        //    list.Add(newDashShadow);
        //    var mar = newDashShadow.GetComponent<Renderer>().material;
        //    var newMar = Instantiate(mar);

        //    newMar.SetFloat("_Mode", 3);
        //    newMar.SetFloat("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        //    newMar.SetFloat("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        //    newMar.SetFloat("_ZWrite", 0); // �ر����д��
        //    newMar.EnableKeyword("_ALPHAPREMULTIPLY_ON"); // ����͸����Ԥ��
        //    newMar.renderQueue = 3000; // ������Ⱦ���У�͸������ͨ����Ҫһ���ϸߵ�ֵ
        //    float newa = ((float)(count++))/(float)ShadowCount;
        //    newMar.color = new Color(newMar.color.r, newMar.color.g, newMar.color.b,newa);
        //    newDashShadow.GetComponent<Renderer>().material = newMar;

        //    yield return new WaitForSeconds(ShadowInterval);
        //}

        //int pos = 0;
        //while (pos < list.Count)
        //{
        //    for (int i = pos; i < list.Count; i++)
        //    {
        //        var mat = list[i].GetComponent<Renderer>().material;
        //        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, Mathf.Lerp(mat.color.a, 0, Time.deltaTime*15));
        //        Debug.Log(list[i].name + "  " + mat.color.a);
        //        if (mat.color.a <= 0.1f){
        //            DashShadowPool.Release(list[pos++]);
        //        }
        //    }
        //    yield return new WaitForSeconds(Time.deltaTime);
        //}
        //Debug.Log("dash finish");
        //list.Clear();
        yield return null;
    }

    IEnumerator dash_phy()
    {
        isDashing = true;
        Rigidbody.useGravity = false;
        bool ifhit = false;
        while (Time.time - LastDashTime < DashInterval && !ifhit) {
            Rigidbody.MovePosition(transform.position + FaceDir * DashSpeed * Time.fixedDeltaTime * Vector3.right);
            ifhit = Physics.Raycast(transform.position,FaceDir * Vector3.right,0.2f,DashStop);
            yield return null;
        }
        Rigidbody.useGravity = true;
        isDashing = false;
        DashParticle.Stop();
        yield return null;
    }

    /// <summary>
    /// �����˺�
    /// </summary>
    /// <param name="Damage">�˺���ֵ</param>
    public void ReciveDamage(float Damage)
    {
        HealthCurrent -= Damage;
    }

    /// <summary>
    /// �ܵ�����
    /// </summary>
    /// <param name="Position">��ɻ��˵��˵�λ��</param>
    /// <param name="Force">������</param>
    public void BeKnockBack(Vector3 Position,float Force)
    {
        Vector3 KonckBackVec = transform.position - Position;
        KonckBackVec.z = 0;
        KonckBackVec.Normalize();
        Rigidbody.AddForce(KonckBackVec*Force,ForceMode.Impulse);
    }
}