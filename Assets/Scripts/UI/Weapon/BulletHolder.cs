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

        //�����ק���˱�����
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
        //�����ק������װ�����ӵ���
        else if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.EquippedBulletRect, eventData.position))
        {
            if (FromBackpack){
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
            WeaponBackpack.Instance.DropBullet(BulletData);
            Destroy(gameObject);
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
        text.text = BulletData.BulletName + "\n" + "�����ȴ��" + BulletData.ShootInterval + "\n";
        if (BulletData as Extend)
        {
            text.text += "�����ͷ�����" + (BulletData as Extend).StepExtension + "\n";
        }
        else if (BulletData as Gain)
        {
            InfoBox.GetComponent<RectTransform>().sizeDelta += new Vector2(0,0);
            switch ((BulletData as Gain).GainType)
            {
                case GainType.General:
                    text.text += "�������ͣ�����\n";
                    break;

                case GainType.Laser:
                    text.text += "�������ͣ�������\n";
                    break;

                case GainType.Bomb:
                    text.text += "�������ͣ�����ը\n";
                    break;
            }
            text.text += "���棺" + (BulletData as Gain).Description + "\n";
        }
        else if (BulletData as Projectile) {
            InfoBox.GetComponent<RectTransform>().sizeDelta += new Vector2(40, 100);
            switch ((BulletData as Projectile).ProjectileType) {
                case ProjectileType.Missile:
                    text.text += "���ͣ�����\n";
                    break;

                case ProjectileType.Laser:
                    text.text += "���ͣ�����\n";
                    break;

                case ProjectileType.Bomb:
                    text.text += "���ͣ���ը\n";
                    break;
            } 
            text.text += "�˺���" + (BulletData as Projectile).Damage + "\n";
            text.text += "���ٶȣ�" + (BulletData as Projectile).InitialVelocity + "\n";
            text.text += "���ˣ�" + ((BulletData as Projectile).SelfDamage? "��\n": "��\n");
            text.text += "����ʱ�䣺" + (BulletData as Projectile).LifeTime + "\n";
            text.text += "ƫ�ƽǣ�" + (BulletData as Projectile).OffsetAngle + "\n";
            if((BulletData as Projectile).ExternalFunction) 
                text.text += " " + (BulletData as Projectile).ExternalFunction.Description + "\n";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Destroy(InfoBox);
    }
}