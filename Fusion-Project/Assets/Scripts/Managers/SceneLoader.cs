using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{   
    public bool isLoaderScene;
    public bool webGLBuild;
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
        if(isLoaderScene)
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
        if(PlayerStats.Instance != null)
            PlayerStats.Instance.SaveStats();
        if(webGLBuild)
        {
            SceneManager.LoadScene(index);
            return;
        }
     
        sceneToLoad = index;
        SceneManager.LoadScene("Loading");
    }      

    public void RestartScene()
    {
        if(webGLBuild)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }
        
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene("Loading");
    }

    public void RestartScene(float delay)
    {
        sceneToLoad = SceneManager.GetActiveScene().buildIndex;
        Invoke("RestartScene", delay);
    }


}
