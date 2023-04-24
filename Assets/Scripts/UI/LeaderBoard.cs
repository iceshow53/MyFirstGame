using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject Board;
    public GameObject Audio;
    public List<Text> texts = new List<Text>();

    // ** �÷��̾ ���ݱ��� �ߴ� ��� ǥ��

    public void Active()
    {
        Board.SetActive(!Board.activeSelf);
        if(Board.activeSelf)
        {
            OnActive();
        }
    }

    public void ResetButton()
    {
        SaveController.Getinstance.ResetData();
        OnActive();
    }

    private void OnActive()
    {
        texts[0].text = "ų : " + SaveController.Getinstance.KILL;
        texts[1].text = "���� : " + SaveController.Getinstance.DIE;
        texts[2].text = "���� : " + SaveController.Getinstance.CLEAR;
        texts[3].text = "�ִ� ų : " + SaveController.Getinstance.MOSTKILL;
    }

    public void Active2()
    {
        Audio.SetActive(!Audio.activeSelf);
    }
}
