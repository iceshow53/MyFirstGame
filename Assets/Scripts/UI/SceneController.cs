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

    // ** �� �ٲٰų� ������ ������ �� ���� ����
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

    // ** �ϵ��� ��ư **
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
