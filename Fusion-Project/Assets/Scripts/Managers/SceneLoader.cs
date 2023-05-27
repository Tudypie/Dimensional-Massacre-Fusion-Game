using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{   
    public bool currentSceneIsLoadingScene;
    public static int sceneToLoad;

    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }
    
    private void Start()
    {   
        if(currentSceneIsLoadingScene)
            StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        yield return null;

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
    }

    public void LoadScene(int index)
    {
        sceneToLoad = index;
        SceneManager.LoadScene("Loading");
    }   

    public void RestartScene()
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Loading");
    }

    public void RestartScene(float delay)
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        Invoke("RestartScene", delay);
    }


}