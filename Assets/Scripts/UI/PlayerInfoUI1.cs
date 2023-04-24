using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI1 : MonoBehaviour
{
	private List<GameObject> items = new List<GameObject>();
	private List<Text> texts = new List<Text>();

	private void Awake()
	{
	}

	void Start()
	{
		GameObject obj = GameObject.Find("BackBoard");
		for (int i = 0; i < obj.transform.childCount; ++i)
		{
			GameObject item = obj.transform.GetChild(i).gameObject;
			items.Add(item);
			texts.Add(item.transform.GetChild(0).GetComponent<Text>());
		}
	}

	// ** 플레이어 정보 열때마다 갱신
	public void PRESSED()
	{
		texts[0].text = "추가 데미지 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAddDmg()) + "x";
		texts[1].text = "이동속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getSpeed());
		texts[2].text = "관통력 : " + PlayerInfo.Getinstance.getPenetrate();
		texts[3].text = "최대 체력 : " + PlayerInfo.Getinstance.getMHP();
		texts[4].text = "공격속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAttackSpeed());
		texts[5].text = "재장전 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getReloadSpeed());
	}

	public void dropWeapon(int i)
	{
		PlayerInfo.Getinstance.dropWeapon(i);
	}
}
