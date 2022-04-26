using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
	public Button StartButton;
	public Blackscreen bs;
	public Animator anim;

	void Start()
	{
		Button btn = StartButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	public void StartG()
    {
		LoadingScreen.nextScene = "SampleScene";
		SceneManager.LoadScene("Loading");
	}

	void TaskOnClick()
	{
		bs.Toggle();
		anim.Play("bs_fade_2");
	}
}
