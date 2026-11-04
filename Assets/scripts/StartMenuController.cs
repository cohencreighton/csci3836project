using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class StartMenuController : MonoBehaviour
{

    void Update() {
        if (Keyboard.current?.anyKey.wasPressedThisFrame == true) {
            StartGame();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void OnExitClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
            Application.Quit();
    }

}
