using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PropType
{ 
    Material,
    Collection
}

[CreateAssetMenu(fileName ="Prop",menuName ="Data/Prop",order = 0)]
public class Prop : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    public PropType Type;
    public int Count_Max;
    public int count_current;
    public int Count_Current
    {
        get {
            return count_current;
        }
        set {
            if(value > Count_Max) {
                count_current = Count_Max;
            }
            else if(value < 0) {
                count_current = 0;
            }
            else {
                count_current = value;
            }
        }
    }
}
