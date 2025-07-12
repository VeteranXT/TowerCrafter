using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum Scene
{
    Main_Menu,
    Map,
    Battle
}
public static class SceneSwicher
{
    private static List<AsyncOperation> operations = new List<AsyncOperation>();    
    public static void LoadScene(string sceneName)
    {
        operations.Add(SceneManager.LoadSceneAsync(sceneName));
    }

    public static void LoadScene(int sceneName)
    {
        operations.Add(SceneManager.LoadSceneAsync(sceneName));
    }

    public static void LoadScene(Scene sceneName)
    {
        operations.Add(SceneManager.LoadSceneAsync((int)sceneName));
    }

    public static void SwichCurrentScene(Scene sceneName)
    {
        operations.Add(SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex));
        operations.Add(SceneManager.LoadSceneAsync((int)sceneName));
    }
}
