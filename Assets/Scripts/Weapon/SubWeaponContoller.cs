using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubWeaponContoller : MonoBehaviour
{
	// ** 테스트용 지뢰 텍스트
	public List<Text> mineText = new List<Text>();
	// ** 테스트용 쉴드 텍스트
	public List<Text> ShieldText = new List<Text>();
	// ** 테스트용 드론 텍스트
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
			mineText[0].text = "범위:" + minecontoller.getRange();
			mineText[1].text = "데미지:" + minecontoller.getDamage();
			mineText[2].text = "쿨타임:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "초";
		}
		ShieldTextList();
		DroneTextList();
	}

	// 화면 열었을 때
    public void ScreenOpen()
    {
		// ** 지뢰 정보 갱신
		if (mine == null)
		{
			if (GameObject.Find("MineItem") != null)
			{
				mine = GameObject.Find("MineItem");
				minecontoller = mine.GetComponent<MineController>();
				mineText[0].text = "범위:" + string.Format("{0:0.#}", minecontoller.getRange());
				mineText[1].text = "데미지:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				mineText[2].text = "쿨타임:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "초";
			}
		}
		else
        {
			if (minecontoller != null)
			{
				mineText[0].text = "범위:" + string.Format("{0:0.#}",minecontoller.getRange());
				mineText[1].text = "데미지:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				mineText[2].text = "쿨타임:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "초";
			}
		}
		// ** 쉴드 정보 갱신
		ShieldTextList();
		DroneTextList();

		skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
	}

	// ** 드론 텍스트
	private void DroneTextList()
    {
		DroneText[0].text = "갯수:" + droneController.getAmount() + "/" + (3 + SaveController.Getinstance.AchiveList[2]);
		DroneText[1].text = "데미지:" + (int)(droneController.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
		DroneText[2].text = "관통:" + droneController.getPenetrate();
		DroneText[3].text = "공격속도:" + string.Format("{0:0.##}", droneController.getAS()) + "/0.1";
	}

	// ** 드론 업그레이드
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
				skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
			}
		}			

	}

	// ** 삽 텍스트
	private void ShieldTextList()
    {
		ShieldText[0].text = "갯수:" + shieldController.getAmount() + "/5";
		ShieldText[1].text = "데미지:" + shieldController.getDamage();
		ShieldText[2].text = "회전속도:" + string.Format("{0:0.#}", shieldController.getSpeed());
	}


	// ** 삽 업그레이드
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
						ShieldText[0].text = "갯수:" + shieldController.getAmount() + "/5";
						break;
					case 1:
						shieldController.addDamage();
						ShieldText[1].text = "데미지:" + shieldController.getDamage();
						break;
					case 2:
						shieldController.addSpeed();
						ShieldText[2].text = "회전속도:" + string.Format("{0:0.#}", shieldController.getSpeed());
						break;
				}
				PlayerInfo.Getinstance.setLPoint(-1);
				skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
    }

	// 밑에 지뢰 수치 관련은 나중에 Minecontroller나 Mine 스크립트로 이동하면 괜찮나? 아니면 위 삽처럼 한곳에 묶어서 보기 편하게 하기
    public void AddMineDamage()
	{
		if (minecontoller != null)
		{
			if (PlayerInfo.Getinstance.getLPoint() > 0)
			{
				PlayerInfo.Getinstance.setLPoint(-1);
				minecontoller.setDamage(9);
				mineText[1].text = "데미지:" + (int)(minecontoller.getDamage() * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
				skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
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
				mineText[0].text = "범위:" + string.Format("{0:0.#}", minecontoller.getRange());
				skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
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
				mineText[2].text = "쿨타임:" + string.Format("{0:0.#}", minecontoller.getCoolTime()) + "초";
				skillpoint.text = "스킬포인트:" + PlayerInfo.Getinstance.getLPoint();
			}
		}
	}


}
