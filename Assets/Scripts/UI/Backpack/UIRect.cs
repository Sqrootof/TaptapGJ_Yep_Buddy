using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIRect : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        PointerIn = false;
    }

    public bool PointerIn;
}
