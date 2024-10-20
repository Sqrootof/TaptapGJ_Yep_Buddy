﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whole : MonoBehaviour
{
    public static List<Achievement> achievements = new List<Achievement>(); //成就列表
    public static int firstLogin=0; //是否第一次登陆
    public static List<Task> questList = new List<Task>(); // 任务列表
    public static Task theQuest;  //正在追踪的任务
    public static List<PictorialBook> pictorialBooks= new List<PictorialBook>();
    public static GameObject[] anchorPoint;
    public static List<AnchorPoint> anchorPoints = new List<AnchorPoint>();
    public static bool isAnchorPoints=false;

    public static AnchorPoint FindAnchorPointByGameObject(GameObject targetObject)
    {
        foreach (var anchorPoint in anchorPoints)
        {
            if (anchorPoint.anchorObject == targetObject)
            {
                return anchorPoint; // 找到对应的 AnchorPoint
            }
        }
        return null; // 未找到，返回 null
    }
}
