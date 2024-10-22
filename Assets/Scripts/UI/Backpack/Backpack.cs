using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : TIntance<Backpack>
{
    readonly string PropDataPath = "Data/PropData";
    [SerializeField] List<Prop> AllProp = new();
    [SerializeField] List<Prop> HeldProps = new();
    [SerializeField] List<Prop> UnHeldProps = new(); 

    private void Awake()
    {
        base.Awake();
        LoadAllPropData();
    }

    void LoadAllPropData()
    {
        AllProp.Clear();
        var Props= Resources.LoadAll(PropDataPath,typeof(Prop));
        foreach (var prop in Props) {
            AllProp.Add(prop as Prop);
        }
    }

    public List<Prop> GetHeldProp()
    {
        HeldProps.Clear();
        foreach (var prop in AllProp) { 
            if(prop.Count_Current > 0) HeldProps.Add(prop);
        }
        return HeldProps;
    }

    public List<Prop> GetUnHeldProp()
    {
        UnHeldProps.Clear();
        foreach (var prop in AllProp){
            if (prop.Count_Current == 0) UnHeldProps.Add(prop);
        }
        return UnHeldProps;
    }

    public void ObtainProp(string PropName,int count,Action WhenGetProp)
    {
        var Props = Resources.LoadAll(PropDataPath, typeof(Prop));
        Prop target = Array.Find(Props, x => (x as Prop).Name == PropName) as Prop;
        if (target) {
            target.Count_Current += count;
            WhenGetProp?.Invoke();
        }
        else{
            Debug.LogError("未找到该物品");
        }
    }

    public bool UseProp(string PropName,int count,Action WhenUseProp)
    {
        var Props = Resources.LoadAll(PropDataPath, typeof(Prop));
        Prop target = Array.Find(Props, x => (x as Prop).Name == PropName) as Prop;
        if (target){
            if (target.Count_Current >= count){
                target.Count_Current += count;
                WhenUseProp?.Invoke();
                return true;
            }
            else
                return false;
        }
        else{
            Debug.LogError("未找到该物品");
            return false;
        }
    }
}
