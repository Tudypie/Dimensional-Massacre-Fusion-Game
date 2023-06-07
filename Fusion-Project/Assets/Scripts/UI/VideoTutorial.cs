using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] tutorialVideos;
    [SerializeField] private GameObject[] tutorialTexts;

    public int index = 0;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {   
            index++;
            if(index > 0)
            {
                tutorialVideos[index - 1].SetActive(false);
                tutorialTexts[index - 1].SetActive(false);
            }
            tutorialVideos[index].SetActive(true);
            tutorialTexts[index].SetActive(true);
        }

        if(index == tutorialVideos.Length)
            SceneLoader.Instance.LoadScene(2);
    }
}
