using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoard : MonoBehaviour
{
    public GameObject Board;
    public GameObject Audio;
    public List<Text> texts = new List<Text>();

    // ** 플레이어가 지금까지 했던 기록 표시

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
        texts[0].text = "킬 : " + SaveController.Getinstance.KILL;
        texts[1].text = "죽음 : " + SaveController.Getinstance.DIE;
        texts[2].text = "생존 : " + SaveController.Getinstance.CLEAR;
        texts[3].text = "최다 킬 : " + SaveController.Getinstance.MOSTKILL;
    }

    public void Active2()
    {
        Audio.SetActive(!Audio.activeSelf);
    }
}
