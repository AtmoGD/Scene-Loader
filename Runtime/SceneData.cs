using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace SceneLoading
{
    [CreateAssetMenu(fileName = "New scene data", menuName = "SceneData")]
    public class SceneData : ScriptableObject
    {
        public string sceneName;    
        public GameObject loadingScreen;
        public GameObject sceneAnimationStart;
        public float sceneStartTime;
        public GameObject sceneAnimationEnd;
        public float sceneEndTime;
    }
}

