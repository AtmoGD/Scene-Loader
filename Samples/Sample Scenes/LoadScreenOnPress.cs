using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneLoading;

public class LoadScreenOnPress : MonoBehaviour
{
    public SceneData sceneData = null;
    public void LoadScene()
    {
        SceneLoader.Instance.LoadScene(sceneData);
    }
}
