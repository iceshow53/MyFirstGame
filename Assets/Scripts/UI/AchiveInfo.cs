using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchiveInfo : MonoBehaviour
{
	private List<int> AchiveInfos = new List<int>();
	public List<GameObject> gameObjects = new List<GameObject>();
	public List<Text> DescriptionText = new List<Text>();
	public List<Text> RewardText = new List<Text>();
	public List<Text> IsClearText = new List<Text>();
	public List<string> Title = new List<string>();

	private void Awake()
	{
		titleSetting();
		setTexts();
		AchiveInfos = SaveController.Getinstance.AchiveList;
	}

	void Start()
	{
		StartCoroutine(loading());
	}

	IEnumerator loading()
	{
		while(!SaveController.Getinstance.Achive_load)
		{
			yield return null;
		}
		UpdateAchive();
	}

	private void titleSetting()
	{
		Title.Add("삽질");
		Title.Add("폭발물 관리자");
		Title.Add("수리공");
		Title.Add("보스 킬러");
		Title.Add("좀비 학살자");
		Title.Add("공중 정원");
		Title.Add("특이한 꽃(?)");
		Title.Add("정복자");
		Title.Add("패배자");
	}

	private void setTexts()
	{
		for (int i = 0; i < 17; i += 2)
			gameObjects.Add(transform.Find("GameObject").GetChild(i).gameObject);
		for(int i = 0; i<gameObjects.Count;++i)
		{
			DescriptionText.Add(gameObjects[i].transform.Find("Description").GetComponent<Text>());
			RewardText.Add(gameObjects[i].transform.Find("Reward").GetComponent<Text>());
			IsClearText.Add(gameObjects[i].transform.Find("IsClear").Find("Text (Legacy)").GetComponent<Text>());
		}
	}

	void UpdateAchive()
	{
		for(int i = 0; i < AchiveInfos.Count - 1; ++i)
		{
			if (AchiveInfos[i] == 1)
			{
				IsClearText[i].text = "달성";
				gameObjects[i].GetComponent<Text>().text = Title[i];
			}
			else
			{
				IsClearText[i].text = "미달성";
				gameObjects[i].GetComponent<Text>().text = "???";
			}
		}
	}
}
