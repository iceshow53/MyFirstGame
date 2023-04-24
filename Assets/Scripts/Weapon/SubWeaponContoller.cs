using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubWeaponContoller : MonoBehaviour
{
	// ** �׽�Ʈ�� ���� �ؽ�Ʈ
	public List<Text> mineText = new List<Text>();
	// ** �׽�Ʈ�� ���� �ؽ�Ʈ
	public List<Text> ShieldText = new List<Text>();
	// ** �׽�Ʈ�� ��� �ؽ�Ʈ
	public List<Text> DroneText = new List<Text>();

	public Text skillpoint;

	private GameObject mine;
	private MineController minecontoller;

	private ShieldController shieldController;
	private DroneController droneController;

    private void Awake()
    {
		if (GameObject.Find("MineItem") != null)
		{
			mine = GameObject.Find("MineItem");
			minecontoller = mine.GetComponent<MineController>();
		}
		shieldController = GameObject.Find("shield").GetComponent<ShieldController>();
		droneController = GameObject.Find("drone").GetComponent<DroneController>();
    }

    private void Start()
    {
		if (mine != null)
		{
			mineText[0].text = "����:" + minecontoller.getRange();
			mineText[1].text = "������:" + minecontoller.getDamage();
			mineText[2].text = "��Ÿ��:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "��";
		}
		ShieldTextList();
		DroneTextList();
	}

	// ȭ�� ������ ��
    public void ScreenOpen()
    {
		// ** ���� ���� ����
		if (mine == null)
		{
			if (GameObject.Find("MineItem") != null)
			{
				mine = GameObject.Find("MineItem");
				minecontoller = mine.GetComponent<MineController>();
				mineText[0].text = "����:" + string.Format("{0:0.#}", minecontoller.getRange());
				mineText[1].text = "������:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				mineText[2].text = "��Ÿ��:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "��";
			}
		}
		else
        {
			if (minecontoller != null)
			{
				mineText[0].text = "����:" + string.Format("{0:0.#}",minecontoller.getRange());
				mineText[1].text = "������:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				mineText[2].text = "��Ÿ��:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "��";
			}
		}
		// ** ���� ���� ����
		ShieldTextList();
		DroneTextList();

		skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
	}

	// ** ��� �ؽ�Ʈ
	private void DroneTextList()
    {
		DroneText[0].text = "����:" + droneController.getAmount() + "/" + (3 + SaveController.Getinstance.AchiveList[2]);
		DroneText[1].text = "������:" + (int)(droneController.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
		DroneText[2].text = "����:" + droneController.getPenetrate();
		DroneText[3].text = "���ݼӵ�:" + string.Format("{0:0.##}", droneController.getAS()) + "/0.1";
	}

	// ** ��� ���׷��̵�
	public void DroneUpgrade(int i)
    {
		if (PlayerInfo.Getinstance.getLPoint() > 0)
        {
			if (droneController.getAble())
			{
				switch (i)
				{
					case 0:
						if (droneController.getAmount() >= 3 + SaveController.Getinstance.AchiveList[2])
							return;
						droneController.AddDrone();
						DroneTextList();
						break;
					case 1:
						droneController.setDamage(4);
						DroneTextList();
						break;
					case 2:
						droneController.setPenetrate(1);
						DroneTextList();
						break;
					case 3:
						if (droneController.getAS() <= 0.1f)
							return;
						droneController.setAS(-0.05f);
						DroneTextList();
						break;
				}
				PlayerInfo.Getinstance.setLPoint(-1);
				skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
			}
		}			

	}

	// ** �� �ؽ�Ʈ
	private void ShieldTextList()
    {
		ShieldText[0].text = "����:" + shieldController.getAmount() + "/5";
		ShieldText[1].text = "������:" + shieldController.getDamage();
		ShieldText[2].text = "ȸ���ӵ�:" + string.Format("{0:0.#}", shieldController.getSpeed());
	}


	// ** �� ���׷��̵�
	public void ShieldUpgrade(int i)
    {
		if (PlayerInfo.Getinstance.getLPoint() > 0)
		{
			if (shieldController.getAble())
			{
				switch (i)
				{
					case 0:
						if (shieldController.getAmount() >= 5)
							return;
						shieldController.addBullet();
						ShieldText[0].text = "����:" + shieldController.getAmount() + "/5";
						break;
					case 1:
						shieldController.addDamage();
						ShieldText[1].text = "������:" + shieldController.getDamage();
						break;
					case 2:
						shieldController.addSpeed();
						ShieldText[2].text = "ȸ���ӵ�:" + string.Format("{0:0.#}", shieldController.getSpeed());
						break;
				}
				PlayerInfo.Getinstance.setLPoint(-1);
				skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
    }

	// �ؿ� ���� ��ġ ������ ���߿� Minecontroller�� Mine ��ũ��Ʈ�� �̵��ϸ� ������? �ƴϸ� �� ��ó�� �Ѱ��� ��� ���� ���ϰ� �ϱ�
    public void AddMineDamage()
	{
		if (minecontoller != null)
		{
			if (PlayerInfo.Getinstance.getLPoint() > 0)
			{
				PlayerInfo.Getinstance.setLPoint(-1);
				minecontoller.setDamage(9);
				mineText[1].text = "������:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
	}

	public void AddMineRange()
	{
		if (minecontoller != null)
		{
			if (PlayerInfo.Getinstance.getLPoint() > 0)
			{
				PlayerInfo.Getinstance.setLPoint(-1);
				minecontoller.setRange(0.2f);
				mineText[0].text = "����:" + string.Format("{0:0.#}", minecontoller.getRange());
				skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
	}

	public void AddMineCool()
	{
		if (minecontoller != null)
		{
			if (PlayerInfo.Getinstance.getLPoint() > 0)
			{
				if (minecontoller.getCoolTime() <= 5.0f)
					return;
				PlayerInfo.Getinstance.setLPoint(-1);
				minecontoller.setCoolTime(0.5f);
				mineText[2].text = "��Ÿ��:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "��";
				skillpoint.text = "��ų����Ʈ:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
	}


}
