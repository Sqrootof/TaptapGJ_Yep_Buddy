using UnityEngine;

public class QuitGame : MonoBehaviour
{
    /*用于退出游戏*/
    
    public void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
