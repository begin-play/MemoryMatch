using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadMainScene : MonoBehaviour
{
    [SerializeField] private float waitTime = 3f;

    [SerializeField] private Text loadingText;

    [SerializeField] private Slider loadingBar;

    private bool allowNextSceneTransition;

    private float loadingProgress;

    private IEnumerator Start()
    {
        allowNextSceneTransition = false;
        StartCoroutine(LoadNextScene());
        yield return new WaitForSeconds(waitTime);
        AnimationComplete();
    }

    private void AnimationComplete()
    {
      
        allowNextSceneTransition = true;
    }

    IEnumerator LoadNextScene()
    {
        //make sure scene 1 exists and is added to build settings
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1);

        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            loadingProgress = Mathf.InverseLerp(0.0f, .9f, asyncOperation.progress);


            loadingBar.value = loadingProgress;
            loadingText.text = $"Loading... {(int)loadingProgress * 100}%";
            yield return null;
            if (allowNextSceneTransition)
            {
                asyncOperation.allowSceneActivation = true;
            }
        }
    }
}