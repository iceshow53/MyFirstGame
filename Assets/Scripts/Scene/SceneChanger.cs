using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private AsyncOperation asyncOperation;
    public Slider slider;
	public Text text;


	IEnumerator Start()
	{
		asyncOperation = SceneManager.LoadSceneAsync("TestGame");
		asyncOperation.allowSceneActivation = false;
		slider.maxValue = 0.9f;


		while (!asyncOperation.isDone)
		{
			slider.value = asyncOperation.progress;

			yield return null;

		}
	}

	private void Update()
	{
		if (SaveController.Getinstance.Achive_load && SaveController.Getinstance.Stats_load)
		{
			text.gameObject.SetActive(true);
			if (Input.anyKeyDown)
				asyncOperation.allowSceneActivation = true;
		}
	}
}
