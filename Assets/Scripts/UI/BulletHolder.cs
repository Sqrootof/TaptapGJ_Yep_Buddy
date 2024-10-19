using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BulletHolder : MonoBehaviour, IDragHandler, IEndDragHandler
{
    Bullet BulletData;
    Image BulletImage;

    void Start()
    {
        BulletImage = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(WeaponPanelMgr.Instance.BulletBackpack.GetComponent<RectTransform>(), eventData.position, Camera.main))
        {
            WeaponBackpack.Instance.StoreEquippedBullet(BulletData,WeaponPanelMgr.Instance.OnBulletDataChanged);
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(WeaponPanelMgr.Instance.EquppiedBulletContainer.GetComponent<RectTransform>(), eventData.position, Camera.main))
        {
            WeaponBackpack.Instance.EquipBulletFromBackpack(BulletData,WeaponPanelMgr.Instance.OnBulletDataChanged);
        }
        else {
            WeaponBackpack.Instance.DropBullet(BulletData);
            Destroy(gameObject);
        }
    }

    public void SetBulletIcon(Sprite Icon)
    {
        BulletImage.sprite = Icon;
    }
}