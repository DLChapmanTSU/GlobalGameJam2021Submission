using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneFade : MonoBehaviour
{
    private int sceneIndex = 0;

    public void SetIndex(int x)
    {
        sceneIndex = x;
    }

    public void SceneTransition()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
