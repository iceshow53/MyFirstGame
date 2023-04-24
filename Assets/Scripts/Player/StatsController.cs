using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsController : MonoBehaviour
{
	private List<GameObject> items = new List<GameObject>();
	private List<Text> texts = new List<Text>();

	public List<Sprite> sprite = new List<Sprite>();

	private List<GameObject> buttons = new List<GameObject>();

	public Text PriceText;

	public Text skillpoint;

	private GameObject Sub;
	public GameObject Sub1;
	public SubWeaponContoller contoller;

	private int price, increasePrice;

	private void Awake()
	{
		Sub = GameObject.Find("SubWeapon");
		Sub1 = GameObject.Find("SubWeaponSystem");
		buttons.Add(GameObject.Find("StatShop"));
		buttons.Add(GameObject.Find("SubWeaponShop"));
	}

	void Start()
	{
		price = 100;
		increasePrice = 50;
		GameObject obj = GameObject.Find("BackBoard");
		for (int i = 0; i < obj.transform.childCount; ++i)
		{
			GameObject item = obj.transform.GetChild(i).gameObject;
			items.Add(item);
			texts.Add(item.transform.GetChild(0).GetComponent<Text>());
		}

		contoller = Sub.GetComponent<SubWeaponContoller>();

		texts[0].text = "데미지 : " + PlayerInfo.Getinstance.getDamage();
		texts[1].text = "이동속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getSpeed());
		texts[2].text = "관통력 : " + PlayerInfo.Getinstance.getPenetrate();
		texts[3].text = "체력회복 : " + PlayerInfo.Getinstance.getHP();
		texts[4].text = "공격속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAttackSpeed());
		texts[5].text = "재장전 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getReloadSpeed()) + "/0.5";

		PriceText.text = "업그레이드 비용 : " + price;
	}

	// ** 상점을 열때마다 갱신
    public void PRESSED()
    {
		texts[0].text = "데미지 : " + PlayerInfo.Getinstance.getDamage();
		texts[1].text = "이동속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getSpeed());
		texts[2].text = "관통력 : " + PlayerInfo.Getinstance.getPenetrate();
		texts[3].text = "체력회복 : " + PlayerInfo.Getinstance.getHP() + "/100";
		texts[4].text = "공격속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAttackSpeed()) + "/0.1";
		texts[5].text = "재장전 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getReloadSpeed()) + "/0.5";
		PriceText.text = "업그레이드 비용 : " + price;
	}

    public void AddDamage()
	{
		if (PlayerInfo.Getinstance.getMoney() >= price)
		{
			PlayerInfo.Getinstance.setDamage(1);
			texts[0].text = "데미지 : " + PlayerInfo.Getinstance.getDamage();
			PriceText.text = "업그레이드 비용 : " + price;
			PlayerInfo.Getinstance.setMoney(-price);
			price += increasePrice;
		}
	}

	public void AddSpeed()
	{
		if (PlayerInfo.Getinstance.getSpeed() < 6.0)
		{
			if (PlayerInfo.Getinstance.getMoney() >= price)
			{
				PlayerInfo.Getinstance.setSpeed(0.2f);
				texts[1].text = "이동속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getSpeed());
				PriceText.text = "업그레이드 비용 : " + price;
				PlayerInfo.Getinstance.setMoney(-price);
				price += increasePrice;
			}
		}
	}

	public void AddPenetrate()
	{
		if (PlayerInfo.Getinstance.getMoney() >= price)
		{
			PlayerInfo.Getinstance.setPenetrate(1);
			texts[2].text = "관통력 : " + PlayerInfo.Getinstance.getPenetrate();
			PriceText.text = "업그레이드 비용 : " + price;
			PlayerInfo.Getinstance.setMoney(-price);
			price += increasePrice;
		}
	}

	public void AddHP()
	{
		if (PlayerInfo.Getinstance.getHP() < 100)
		{
			if (PlayerInfo.Getinstance.getMoney() >= 500)
			{
				PlayerInfo.Getinstance.addHP(10);
				if (PlayerInfo.Getinstance.getHP() >= 100)
					PlayerInfo.Getinstance.setHP(100);
				texts[3].text = "체력회복 : " + PlayerInfo.Getinstance.getHP() + "/100";
				PriceText.text = "업그레이드 비용 : " + price;
				PlayerInfo.Getinstance.setMoney(-500);
			}
		}
	}

	public void AddAttackSpeed()
	{
		if (PlayerInfo.Getinstance.getAttackSpeed() > 0.1f)
		{
			if (PlayerInfo.Getinstance.getMoney() >= price)
			{
				PlayerInfo.Getinstance.setAttackSpeed(0.05f);
				texts[4].text = "공격속도 : " + string.Format("{0:0.##}", PlayerInfo.Getinstance.getAttackSpeed()) + "/0.1";
				PriceText.text = "업그레이드 비용 : " + price;
				PlayerInfo.Getinstance.setMoney(-price);
				price += increasePrice;
			}
		}
	}
	   
	public void AddReloadSpeed()
	{
		if (PlayerInfo.Getinstance.getReloadSpeed() > 0.55f)
		{
			if (PlayerInfo.Getinstance.getMoney() >= price)
			{
				PlayerInfo.Getinstance.setReloadSpeed(0.05f);
				texts[5].text = "재장전 : " + string.Format("{0:0.##}",PlayerInfo.Getinstance.getReloadSpeed()) + "/0.5";
				PriceText.text = "업그레이드 비용 : " + price;
				PlayerInfo.Getinstance.setMoney(-price);
				price += increasePrice;
			}
		}
	}

	public void dropWeapon(int i)
	{
		PlayerInfo.Getinstance.dropWeapon(i);
	}

	public void buttonClick()
    {
		for(int i = 0;i<2;++i)
        {
			buttons[i].GetComponent<Button>().interactable = !buttons[i].GetComponent<Button>().IsInteractable();
        }
		Sub1.SetActive(!Sub1.activeSelf);
		Sub.GetComponent<SubWeaponContoller>().ScreenOpen();
    }
}
