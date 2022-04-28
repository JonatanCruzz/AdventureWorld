using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static string nextScene;

    [SerializeField]
    public Image pogressbar;

    IEnumerator LoadLevel()
    {
        AsyncOperation level = SceneManager.LoadSceneAsync(nextScene);
        while (!level.isDone)
        {
            pogressbar.fillAmount = Mathf.Clamp01(level.progress / 0.9f);
            yield return null;
        }
    }
    public void Start()
    {
        StartCoroutine(LoadLevel());
    }
}
