using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanelMgr : TIntance<BackpackPanelMgr>
{
    [Header("UI组件")]
    [SerializeField] Text Coin;
    [SerializeField] Image Info_PropIcon;
    [SerializeField] Text Info_PropType;
    [SerializeField] Text Info_PropAmount;
    [SerializeField] Text Info_Description;
    [SerializeField] VerticalLayoutGroup VLG_Backpack;
    [SerializeField] Scrollbar ScrollBar_Backpack;
    [SerializeField] RectTransform SelectedMask;

    [Header("UI预制体")]
    [SerializeField] GameObject PropContainer;//道具容器,有HorizontalLayoutGroup组件
    [SerializeField] GameObject PropHolder;

    [Header("UI范围")]
    [SerializeField]
    Rect Rect_Backpack;

    bool PropBeSelected = false;
    RectTransform Prop_Selected;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBackpack();
    }

    // Update is called once per frame
    void Update()
    {
        if(PropBeSelected && Prop_Selected){
            SelectedMask.position = Prop_Selected.position;
        }
        RowBackpac();
    }

    void RowBackpac()
    {
        VerticalLayoutGroup VLG = VLG_Backpack.GetComponent<VerticalLayoutGroup>();
        RectTransform RT = PropHolder.GetComponent<RectTransform>();
        float RowRange = (VLG_Backpack.transform.childCount - 3) * (VLG.spacing + RT.rect.height);
        if (RowRange <= 0)
        {
            ScrollBar_Backpack.gameObject.SetActive(false);
            return;
        }
        else ScrollBar_Backpack.gameObject.SetActive(true);
        float TargetY = Mathf.Lerp(0, RowRange, ScrollBar_Backpack.value);
        RectTransform RectToMove = VLG_Backpack.GetComponent<RectTransform>();
        Vector2 TargetPos = new Vector2(0, TargetY);
        RectToMove.anchoredPosition = TargetPos;
    }

    public void WhenPropBeSelected(Prop propData,RectTransform Proprect)
    {
        //处理遮罩
        PropBeSelected = true;
        Prop_Selected = Proprect;
        SelectedMask.gameObject.SetActive(true);

        //更新信息
        Info_PropIcon.sprite = propData.Icon;
        switch (propData.Type) {
            case PropType.Material:
                Info_PropType.text = "材料";
                break;
            case PropType.Collection:
                Info_PropType.text = "收藏品";
                break;
        }
        Info_PropAmount.text = propData.Count_Current.ToString() + "/" + propData.Count_Max.ToString();
        Info_Description.text = propData.Description;
    }

    public void UpdateBackpack()
    {
        GameObject Child;
        //删除所有的子弹
        for (int i = VLG_Backpack.gameObject.transform.childCount - 1; i >= 0; i--)
        {
            Child = VLG_Backpack.gameObject.transform.GetChild(i).gameObject;
            Destroy(Child);
        }   

        var list = Backpack.Instance.GetHeldProp();
        int count = 0;
        GameObject CurrentPropContiner = Instantiate(PropContainer);
        CurrentPropContiner.transform.SetParent(VLG_Backpack.transform);
        foreach (var prop in list) {
            if (count == 5){
                count = 0;
                CurrentPropContiner = Instantiate(PropContainer);
                CurrentPropContiner.transform.SetParent(VLG_Backpack.transform);
            }

            Debug.Log(prop.name);
            GameObject CurrentPropHolder = Instantiate(PropHolder);
            CurrentPropHolder.transform.SetParent(CurrentPropContiner.transform);
            CurrentPropHolder.GetComponent<Image>().sprite = prop.Icon;
            CurrentPropHolder.GetComponent<Button>().onClick.AddListener(() => { WhenPropBeSelected(prop,CurrentPropHolder.GetComponent<RectTransform>()); }); 
            count++;
        }
    }

    public void CloseUI()
    {
        PropBeSelected = false;
        Prop_Selected = null;
        gameObject.SetActive(false);
    }

}
