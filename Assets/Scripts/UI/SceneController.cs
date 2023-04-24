using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;


public class SceneController : MonoBehaviour
{
    public Button button;
    public Sprite On;
    public Sprite Off;

    // ** 씬 바꾸거나 게임을 종료할 때 정보 저장
    public void SceneChange(string s)
    {
        if(SaveController.Getinstance.Achive_load && SaveController.Getinstance.Stats_load)
            SaveController.Getinstance.onSave();
        SceneManager.LoadScene(s);
    }

    public void GameExit()
    {
        SaveController.Getinstance.onSave();
        Application.Quit();
    }

    // ** 하드모드 버튼 **
    public void HardMode()
    {
        SaveController.Getinstance.hardMode();
        if (SaveController.Getinstance.hardmod)
            button.image.sprite = On;
        else
            button.image.sprite = Off;
    }

    public void textFunction()
	{
        SaveController.Getinstance.StatUpdate();
	}
}
