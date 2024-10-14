using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonPYP : MonoBehaviour
{
    public string sceneToLoad;

    public void SwitchScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
