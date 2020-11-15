using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public bool setCursorInit = false;
    public List<Texture2D> cursors = new List<Texture2D>();

    void Start()
    {
        if (setCursorInit)
            Cursor.SetCursor(cursors[0], new Vector2(0, 0), CursorMode.ForceSoftware);
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

    public void SetCursor(int index)
    {
        Cursor.SetCursor(cursors[index], new Vector2(16, 16), CursorMode.ForceSoftware);
    }
}
