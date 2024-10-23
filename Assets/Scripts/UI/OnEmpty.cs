using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEmpty : MonoBehaviour
{
    public static bool ClickButton;

    public void setTrue()
    {
        ClickButton = true;
    }

    public void setFalse()
    {
        ClickButton = false;
    }
}
