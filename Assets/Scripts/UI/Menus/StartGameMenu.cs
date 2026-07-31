using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameMenu : MenuPanel
{
    public int sceneToLoad = 1;

    public void StartGame()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
