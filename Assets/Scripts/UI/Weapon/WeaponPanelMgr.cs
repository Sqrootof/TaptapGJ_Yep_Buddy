using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Serializable]
public struct Rect
{
    public Vector2 Origin;
    public float Height;
    public float Width;
}

public class WeaponPanelMgr : TIntance<WeaponPanelMgr>
{
    [Header("控件")]
    [SerializeField] GameObject HoldBoard;
    [SerializeField] Image CurrentWeapon;
    [SerializeField] Text WeaponInfo;
    [SerializeField] public GameObject EquippiedBulletContainer;//已装备子弹的父物体
    [SerializeField] Scrollbar ScorllBar_EquippedBulletContainer;//已装备子弹的滚动条
    [SerializeField] public GameObject BulletBackpack;//在背包子弹的父物体,有VerticalLayoutGroup组件
    [SerializeField] Scrollbar ScrollerBar_BulletBackpack;//背包子弹的滚动条
    [SerializeField] public GameObject WeaponContainer;//当前未使用物体的父物体,有VerticalLayoutGroup组件
    [SerializeField] Scrollbar ScrollerBar_WeaponContainer;//武器的滚动条
    [SerializeField] public UIRect EquippedBulletMask;
    [SerializeField] public UIRect BackpackMask;

    [SerializeField] float RawSpeed = 10;

    [Header("UI预制体")]
    [SerializeField] GameObject BulletContainer;//子弹容器,放在CurrentBackpack下.有HorizontalLayoutGroup组件  
    [SerializeField] GameObject BulletHolder;
    [SerializeField] GameObject WeaponHolder;

    [Header("UI范围")]
    public Rect CurrentWeaponRect;
    public Rect EquippedBulletRect;
    public Rect BulletBackpackRect;
    public Rect WeaponContainerRect;

    #region"UI事件"
    public Action OnBulletDataChanged;
    public Action OnWeaponDataChanged;
    #endregion

    private void Awake()
    {
        base.Awake();
        OnBulletDataChanged += UpdateBulletUI;
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
        RowQuippedBullet();
    }

    void RowStoredBullet()
    {
        VerticalLayoutGroup VLG = BulletBackpack.GetComponent<VerticalLayoutGroup>();
        RectTransform RT = BulletContainer.GetComponent<RectTransform>();
        float RowRange = (BulletBackpack.transform.childCount-3) * (VLG.spacing + RT.rect.height);
        if (RowRange < 0){
            ScrollerBar_BulletBackpack.gameObject.SetActive(false);
            return;
        }
        else ScrollerBar_BulletBackpack.gameObject.SetActive(true);
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
        if (RowRange < 0){
            ScrollerBar_WeaponContainer.gameObject.SetActive(false);
            return;
        }
        else ScrollerBar_WeaponContainer.gameObject.SetActive(true);
        float TargetY = Mathf.Lerp(0, RowRange, ScrollerBar_WeaponContainer.value);
        RectTransform RectToMove = WeaponContainer.GetComponent<RectTransform>();
        Vector2 TargetPos = new Vector2(0, TargetY);
        RectToMove.anchoredPosition = TargetPos;
    }

    void RowQuippedBullet()
    {
        HorizontalLayoutGroup VLG = EquippiedBulletContainer.GetComponent<HorizontalLayoutGroup>();
        RectTransform RT = BulletHolder.GetComponent<RectTransform>();
        float RowRange = (EquippiedBulletContainer.transform.childCount - 9) * (VLG.spacing + RT.rect.width);
        if (RowRange < 0) { 
            ScorllBar_EquippedBulletContainer.gameObject.SetActive(false);
            return;
        }
        else ScorllBar_EquippedBulletContainer.gameObject.SetActive(true);
        float TargetX = Mathf.Lerp(0, RowRange, ScorllBar_EquippedBulletContainer.value);
        RectTransform RectToMove = EquippiedBulletContainer.GetComponent<RectTransform>();
        Vector2 TargetPos = new Vector2(-TargetX,0);
        RectToMove.anchoredPosition = TargetPos;
    }

    void UpdateBulletUI()
    {
        UpdateCurrentBullet();
        UpdateStroedBullet();
    }

    void UpdateCurrentBullet()
    {
        var List = WeaponBackpack.Instance.GetEquippedBullets();

        GameObject Child;
        //删除所有的子弹
        for (int i= EquippiedBulletContainer.transform.childCount-1; i >= 0 ;i-- )
        {
            Child = EquippiedBulletContainer.transform.GetChild(i).gameObject;
            Destroy(Child);
        }

        //添加新的子弹
        foreach (var bullet in List) {
            GameObject newBullet = Instantiate(BulletHolder);
            newBullet.transform.parent = EquippiedBulletContainer.transform;
            BulletHolder BH = newBullet.GetComponent<BulletHolder>();
            BH.BulletData = bullet;
            BH.SetBulletIcon(bullet.Icon);
        }
    }

    void UpdateStroedBullet()
    {
        var List = WeaponBackpack.Instance.GetBulletInBackpack();

        GameObject Child;
        //删除所有的子弹
        for (int i = BulletBackpack.transform.childCount - 1; i >= 0; i--)
        {
            Child = BulletBackpack.transform.GetChild(i).gameObject;
            Destroy(Child);
        }

        //添加新的子弹
        int count = 1;
        GameObject CurrentContainer = Instantiate(BulletContainer);
        CurrentContainer.transform.parent = BulletBackpack.transform;
        foreach (var bullet in List) {
            if (count > 6){
                count = 1;
                CurrentContainer = Instantiate(BulletContainer);
                CurrentContainer.transform.parent = BulletBackpack.transform;
            }
            GameObject newBullet = Instantiate(BulletHolder);
            newBullet.transform.parent = CurrentContainer.transform;
            BulletHolder BH = newBullet.GetComponent<BulletHolder>();
            BH.BulletData = bullet;
            BH.SetBulletIcon(bullet.Icon);
            count++;
        }
    }

    public bool PointerInRect(Rect UIRect ,Vector2 PointerPos)
    {
        Vector2 RelativePos = PointerPos - UIRect.Origin;
        return ((RelativePos.x <= UIRect.Width && RelativePos.x >= 0) && (RelativePos.y <= UIRect.Height && RelativePos.y >= 0));
    }
}