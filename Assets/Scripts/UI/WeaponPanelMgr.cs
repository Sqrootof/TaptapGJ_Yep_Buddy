using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPanelMgr : TIntance<WeaponPanelMgr>
{
    [Header("�ؼ�")]
    [SerializeField] GameObject HoldBoard;
    [SerializeField] Image CurrentWeapon;
    [SerializeField] Text WeaponInfo;
    [SerializeField] public GameObject EquppiedBulletContainer;//��װ���ӵ��ĸ�����
    [SerializeField] public GameObject BulletBackpack;//�ڱ����ӵ��ĸ�����,��VerticalLayoutGroup���
    [SerializeField] Scrollbar ScrollerBar_BulletBackpack;//�����ӵ��Ĺ�����
    [SerializeField] public GameObject WeaponContainer;//��ǰδʹ������ĸ�����,��VerticalLayoutGroup���
    [SerializeField] Scrollbar ScrollerBar_WeaponContainer;//�����Ĺ�����

    [SerializeField] float RawSpeed = 10;

    [Header("UIԤ����")]
    [SerializeField] GameObject BulletContainer;//�ӵ�����,����CurrentBackpack��.��HorizontalLayoutGroup���  
    [SerializeField] GameObject BulletHolder;
    [SerializeField] GameObject WeaponHolder;

    #region"UI�¼�"
    public Action OnBulletDataChanged;
    public Action OnWeaponDataChanged;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        OnBulletDataChanged += UpdateBulletUI;
    }

    private void OnEnable()
    {
        OnBulletDataChanged?.Invoke();
        OnWeaponDataChanged?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        RowStoredBullet();
        RowStoredWeapon();
    }

    void RowStoredBullet()
    {
        VerticalLayoutGroup VLG = BulletBackpack.GetComponent<VerticalLayoutGroup>();
        RectTransform RT = BulletContainer.GetComponent<RectTransform>();
        float RowRange = (BulletBackpack.transform.childCount-3) * (VLG.spacing + RT.rect.height);
        if (RowRange < 0) RowRange = 0;
        float TargetY = Mathf.Lerp(0,RowRange,ScrollerBar_BulletBackpack.value);
        RectTransform RectToMove = BulletBackpack.GetComponent<RectTransform>();
        Vector2 TargetPos = new Vector2(0, TargetY);
        RectToMove.anchoredPosition = TargetPos;
    }

    void RowStoredWeapon()
    {
        VerticalLayoutGroup VLG = WeaponContainer.GetComponent<VerticalLayoutGroup>();
        RectTransform RT = WeaponHolder.GetComponent<RectTransform>();
        float RowRange = (WeaponContainer.transform.childCount-3) * (VLG.spacing + RT.rect.height);
        if (RowRange < 0) RowRange = 0;
        float TargetY = Mathf.Lerp(0, RowRange, ScrollerBar_WeaponContainer.value);
        RectTransform RectToMove = WeaponContainer.GetComponent<RectTransform>();
        Vector2 TargetPos = new Vector2(0, TargetY);
        RectToMove.anchoredPosition = TargetPos;
    }

    void UpdateBulletUI()
    {
        UpdateCurrentBullet();
        UpdateStroedBullet();
    }

    void UpdateCurrentBullet()
    {

    }

    void UpdateStroedBullet()
    {

    }
}