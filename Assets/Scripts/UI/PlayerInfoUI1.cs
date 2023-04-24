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

	// ** �÷��̾� ���� �������� ����
	public void PRESSED()
	{
		texts[0].text = "�߰� ������ : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAddDmg()) + "x";
		texts[1].text = "�̵��ӵ� : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getSpeed());
		texts[2].text = "����� : " + PlayerInfo.Getinstance.getPenetrate();
		texts[3].text = "�ִ� ü�� : " + PlayerInfo.Getinstance.getMHP();
		texts[4].text = "���ݼӵ� : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAttackSpeed());
		texts[5].text = "������ : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getReloadSpeed());
	}

	public void dropWeapon(int i)
	{
		PlayerInfo.Getinstance.dropWeapon(i);
	}
}
