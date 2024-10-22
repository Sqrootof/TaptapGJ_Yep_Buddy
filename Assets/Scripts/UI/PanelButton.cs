using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelButton : MonoBehaviour
{
    public PanelChoice panelChoice;
    public int a;

    public void Alt()
    {
        PanelChoice.Num = a;
        panelChoice.AltButton(a);
        panelChoice.AltUI(a);
    }
}
