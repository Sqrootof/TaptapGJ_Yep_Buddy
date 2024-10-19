using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponPanelMgr : TIntance<WeaponPanelMgr>
{
    [Header("控件")]
    [SerializeField] GameObject HoldBoard;
    [SerializeField] Image CurrentWeapon;
    [SerializeField] Text WeaponInfo;
    [SerializeField] public GameObject EquppiedBulletContainer;//已装备子弹的父物体
    [SerializeField] public GameObject BulletBackpack;//在背包子弹的父物体,有VerticalLayoutGroup组件
    [SerializeField] Scrollbar ScrollerBar_BulletBackpack;//背包子弹的滚动条
    [SerializeField] public GameObject WeaponContainer;//当前未使用物体的父物体,有VerticalLayoutGroup组件
    [SerializeField] Scrollbar ScrollerBar_WeaponContainer;//武器的滚动条

    [SerializeField] float RawSpeed = 10;

    [Header("UI预制体")]
    [SerializeField] GameObject BulletContainer;//子弹容器,放在CurrentBackpack下.有HorizontalLayoutGroup组件  
    [SerializeField] GameObject BulletHolder;
    [SerializeField] GameObject WeaponHolder;

    #region"UI事件"
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