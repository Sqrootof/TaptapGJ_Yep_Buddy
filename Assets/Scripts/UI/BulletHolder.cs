using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletHolder : MonoBehaviour, IDragHandler, IEndDragHandler , IBeginDragHandler
{
    public Bullet BulletData;
    Image BulletImage;
    bool FromBackpack = false;
    Vector3 OriginPos;
    Transform OriginParent;

    void Awake()
    {
        BulletImage = transform.GetChild(0).GetComponent<Image>();
    }

    void Start()
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
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
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position);

        //如果拖拽到了背包里
        if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.BulletBackpackRect, eventData.position))
        {
            Debug.Log("拖拽到了背包里");
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
        else if (WeaponPanelMgr.Instance.PointerInRect(WeaponPanelMgr.Instance.EquippedBulletRect, eventData.position) && FromBackpack)
        {
            Debug.Log("拖拽到了已装备的子弹上");
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
}