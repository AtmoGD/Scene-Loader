using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLoader;

public class LoadScreenOnPress : MonoBehaviour
{
    public SceneData sceneData = null;
    public void LoadScene()
    {
        LoadManager.Instance.LoadScene(sceneData);
    }
}
