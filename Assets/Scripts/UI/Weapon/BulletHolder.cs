using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletHolder : MonoBehaviour, IDragHandler, IEndDragHandler , IBeginDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Bullet BulletData;
    Image BulletImage;
    bool FromBackpack = false;
    Vector3 OriginPos;
    Transform OriginParent;
    [SerializeField] GameObject InfoBox_Pre;
    GameObject InfoBox;
    [SerializeField] GameObject Drop;

    void Awake()
    {
        BulletImage = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (InfoBox) Destroy(InfoBox);
        OriginPos = transform.position;
        OriginParent = transform.parent;
        transform.parent = transform.parent.parent.parent.parent;
        if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.BulletBackpackRect, eventData.position))
        {
            FromBackpack = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (InfoBox) Destroy(InfoBox);
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (InfoBox) Destroy(InfoBox);

        //如果拖拽到了背包里
        if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.BulletBackpackRect, eventData.position))
        {
            if (!FromBackpack && WeaponBackpack.Instance.GetEquippedBullets().Count > 1){
                WeaponBackpack.Instance.StoreEquippedBullet(BulletData, WeaponPanelMgr.Instance.OnBulletDataChanged);
                Destroy(gameObject);
            }
            else
            {
                transform.position = OriginPos;
                transform.parent = OriginParent;
            }
        }
        //如果拖拽到了已装备的子弹上
        else if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.EquippedBulletRect, eventData.position))
        {
            if (FromBackpack && WeaponBackpack.Instance.GetEquippedBullets().Count < 7){
                WeaponBackpack.Instance.EquipBulletFromBackpack(BulletData, WeaponPanelMgr.Instance.OnBulletDataChanged);
                Destroy(gameObject);
            }
            else
            {
                transform.position = OriginPos;
                transform.parent = OriginParent;
            }
        }
        else
        {
            Debug.Log("Drop");
            DropBullet();
            if (FromBackpack)
                WeaponBackpack.Instance.GetBulletInBackpack().Remove(BulletData);
            else
                WeaponBackpack.Instance.GetEquippedBullets().Remove(BulletData);
            Destroy(gameObject);

            //transform.position = OriginPos;
            //transform.parent = OriginParent;
        }
    }

    public void SetBulletIcon(Sprite Icon)
    {
        BulletImage.sprite = Icon;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoBox = Instantiate(InfoBox_Pre);
        InfoBox.transform.SetParent(transform.parent.parent.parent.parent);
        InfoBox.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position + new Vector3(120,0,0);
        Text text = InfoBox.GetComponentInChildren<Text>();
        text.text = BulletData.BulletName + "\n" + "射击冷却：" + BulletData.ShootInterval + "\n";
        if (BulletData as Extend)
        {
            text.text += "额外释放数：" + (BulletData as Extend).StepExtension + "\n";
        }
        else if (BulletData as Gain)
        {
            InfoBox.GetComponent<RectTransform>().sizeDelta += new Vector2(0,0);
            switch ((BulletData as Gain).GainType)
            {
                case GainType.General:
                    text.text += "增益类型：泛用\n";
                    break;

                case GainType.Laser:
                    text.text += "增益类型：仅激光\n";
                    break;

                case GainType.Bomb:
                    text.text += "增益类型：仅爆炸\n";
                    break;
            }
            text.text += "增益：" + (BulletData as Gain).Description + "\n";
        }
        else if (BulletData as Projectile) {
            InfoBox.GetComponent<RectTransform>().sizeDelta += new Vector2(40, 100);
            switch ((BulletData as Projectile).ProjectileType) {
                case ProjectileType.Missile:
                    text.text += "类型：导弹\n";
                    break;

                case ProjectileType.Laser:
                    text.text += "类型：激光\n";
                    break;

                case ProjectileType.Bomb:
                    text.text += "类型：爆炸\n";
                    break;
            } 
            text.text += "伤害：" + (BulletData as Projectile).Damage + "\n";
            text.text += "初速度：" + (BulletData as Projectile).InitialVelocity + "\n";
            text.text += "己伤？" + ((BulletData as Projectile).SelfDamage? "是\n": "否\n");
            text.text += "持续时间：" + (BulletData as Projectile).LifeTime + "\n";
            text.text += "偏移角：" + (BulletData as Projectile).OffsetAngle + "\n";
            if((BulletData as Projectile).ExternalFunction) 
                text.text += " " + (BulletData as Projectile).ExternalFunction.Description + "\n";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(InfoBox);
    }

    private void OnDisable()
    {
        if (InfoBox) Destroy(InfoBox);
    }

    void DropBullet()
    {
        //Debug.Log(BulletData.BulletName);
        Vector3 pos = PlayerController.Instance.gameObject.transform.position + new Vector3(1,1,0);
        GameObject DropBullet = Instantiate(Drop,pos,Quaternion.identity);
        DropBullet.GetComponent<BulletDrop>().BulletInfo = BulletData;
        Destroy(gameObject);
    }
}