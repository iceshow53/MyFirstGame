using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerUpgrade : MonoBehaviour
{
	private List<int> playerStat = new List<int>();

	public List<GameObject> stats = new List<GameObject>();

	private void Awake()
	{
		playerStat = SaveController.Getinstance.getStatData();
	}

	void Start()
	{
		for (int j = 0; j < 9; ++j)
		{
			for (int i = 3; i >= playerStat[j]; --i)
			{
				stats[j].transform.GetChild(i + 2).GetChild(0).gameObject.SetActive(false);
			}
		}
	}

    private void Update()
    {
		stats[0].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[0] * 100;
		stats[1].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[1] * 10 + "%";
		stats[2].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[2] * 10;
		stats[3].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[3] * 10 + "%";
		stats[4].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[4] * 10 + "%";
		stats[5].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[5] * 10 + "%";
		stats[6].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[6] * 10 + "%";
		stats[7].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[7] * 1;
		stats[8].transform.GetChild(0).GetComponent<Text>().text = "+ " + playerStat[8] * 1;
		stats[9].transform.GetComponent<Text>().text = "특성 포인트 : " + playerStat[9];
	}

    public void UpgradeStat(int i)
    {
		// ** 업그레이드를 끝마쳤거나 업그레이드 할 특성포인트가 없으면 리턴
		if (playerStat[i] == 4)
			return;
		if (playerStat[9] == 0)
			return;

		playerStat[9] -= 1;
		playerStat[i] += 1;
		if (stats[i].transform.GetChild(2).GetChild(0).gameObject.activeSelf)
		{
			if (stats[i].transform.GetChild(3).GetChild(0).gameObject.activeSelf)
			{
				if (stats[i].transform.GetChild(4).GetChild(0).gameObject.activeSelf)
				{
					if (stats[i].transform.GetChild(5).GetChild(0).gameObject.activeSelf)
					{
						return;
					}
					else
						stats[i].transform.GetChild(5).GetChild(0).gameObject.SetActive(true);
				}
				else
					stats[i].transform.GetChild(4).GetChild(0).gameObject.SetActive(true);
			}
			else
				stats[i].transform.GetChild(3).GetChild(0).gameObject.SetActive(true);
		}
		else
			stats[i].transform.GetChild(2).GetChild(0).gameObject.SetActive(true);

		SaveUpgrade();
	}

    public void exit()
    {
		SaveUpgrade();
    }

    private void SaveUpgrade()
    {
		SaveController.Getinstance.UpdateStatData(playerStat);
		SaveController.Getinstance.onSaveStat();
    }

	public void reload()
	{
		playerStat = SaveController.Getinstance.getStatData();
	}
}
