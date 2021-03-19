using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoading
{
    public class SceneLoader : MonoBehaviour
    {
        #region Actions
        public Action OnLoadStart;
        public Action<float> OnLoadProgressChanged;
        public Action OnLoadEnd;
        #endregion

        #region Singleton
        private static SceneLoader _instance { get; set; }
        public static SceneLoader Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = new GameObject("SceneLoader").AddComponent<SceneLoader>();
                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }
        #endregion

        #region Properties
        public bool sceneIsLoading { get; private set; }
        public AsyncOperation loadingOperation { get; private set; }
        #endregion

        #region Default
        public bool playAnimationOnStart = true;
        public GameObject defaultAnimationIn = null;
        public float defaultAnimationTimeIn = 1;
        public GameObject defaultAnimationOut = null;
        public GameObject defaultLoadingScreen = null;
        public float defaultAnimationTimeOut = 1;
        #endregion

        void Awake()
        {
            if (_instance != null)
            {
                Destroy(this.gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            if (playAnimationOnStart && defaultAnimationOut)
                StartCoroutine(DoTransition(defaultAnimationOut, defaultAnimationTimeIn));
        }

        public void LoadScene(SceneData sceneData)
        {
            if (sceneIsLoading) return;

            string sceneName = sceneData.sceneName;
            GameObject animStart = sceneData.sceneAnimationStart;
            float animStartTime = sceneData.sceneStartTime;
            GameObject animEnd = sceneData.sceneAnimationEnd;
            float animEndTime = sceneData.sceneEndTime;
            GameObject loadingScreen = sceneData.loadingScreen;
            StartCoroutine(LoadingScene(sceneName, animStart, animStartTime, animEnd, animEndTime, loadingScreen));
        }

        public void LoadScene(string sceneName)
        {
            if (sceneIsLoading) return;

            StartCoroutine(LoadingScene(sceneName, defaultAnimationIn, defaultAnimationTimeIn, defaultAnimationOut, defaultAnimationTimeOut, defaultLoadingScreen));
            
        }

        IEnumerator LoadingScene(string sceneName, GameObject animStart, float animStartTime, GameObject animEnd, float animEndTime, GameObject loadingScreenPrefab)
        {
            yield return StartCoroutine(DoTransition(animStart, animStartTime));
            OnLoadStart?.Invoke();

            GameObject loadingScreen = Instantiate(loadingScreenPrefab, this.transform);

            int currentScene = SceneManager.GetActiveScene().buildIndex;
            loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            sceneIsLoading = true;

            while (loadingOperation.progress != 1)
            {
                yield return null;
                float progressClamped = Mathf.Clamp01(loadingOperation.progress / 0.9f);
                OnLoadProgressChanged?.Invoke(progressClamped);
            }

            sceneIsLoading = false;
            loadingOperation = null;

            OnLoadEnd?.Invoke();
            yield return StartCoroutine(DoTransition(animEnd, animEndTime));
            SceneManager.UnloadSceneAsync(currentScene);
            Destroy(loadingScreen);
        }

        IEnumerator DoTransition(GameObject transitionObject, float time)
        {
            GameObject loadingObject = Instantiate(transitionObject, this.transform);
            yield return new WaitForSeconds(time);
            Destroy(loadingObject);
        }
    }

}
