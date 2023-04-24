using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUIController : MonoBehaviour
{
	public List<GameObject> WeaponList = new List<GameObject>();
	private Animator animator;
	private SubWeaponContoller subWeapon;
	private List<string> NameList = new List<string>();
	public List<Sprite> sprites = new List<Sprite>();
	public List<Image> buttons = new List<Image>();
	public List<Text> texts = new List<Text>();
	private PassiveController P_passive;

	private List<int> rand = new List<int>();

	private bool is_item;


	private AudioSource Select;
	enum Weapons
    {
		Shotgun,
		MachineGun,
		Mine
    }

	private void Awake()
	{
		is_item = false;

		Select = GameObject.Find("Select").GetComponent<AudioSource>();
		P_passive = GameObject.Find("Passive").GetComponent<PassiveController>();

		animator = transform.GetComponent<Animator>();
		// ** 무기 리스트
		WeaponList.Add(Resources.Load("Prefabs/Weapons/Shotgun") as GameObject);
		WeaponList.Add(Resources.Load("Prefabs/Weapons/MachineGun") as GameObject);
		WeaponList.Add(Resources.Load("Prefabs/SubWeapon/MineItem") as GameObject);

		// ** 보조무기
		WeaponList.Add(GameObject.Find("shield"));
		WeaponList.Add(GameObject.Find("drone"));

		// ** 패시브
		GameObject passive = GameObject.Find("Passive");
		for(int i = 0; i < passive.transform.childCount;++i)
        {
			WeaponList.Add(passive.transform.GetChild(i).gameObject);
        }
		/*
		패시브 리스트
		0 추가 데미지
		1 이동속도
		2 관통
		3 공속
		4 재장전
		5 최대 체력
		 */

		// ** 무기 설명 추가 순서는 무기리스트부터 차례대로
		NameList.Add("샷건\n\n탄창 : 10발\n추가 데미지 : 1\n발사 속도 : -0.2\n재장전 속도 : -0.3초");
		NameList.Add("머신건\n\n탄창 : 30발\n 발사 속도 :\n+ 0.1");
		NameList.Add("지뢰\n\n기본 설치 속도 : 9.0초\n데미지 : 10\n범위 : 2\n이미 가지고 있다면\n데미지 +9");
		NameList.Add("삽\n\n갯수 + 1\n이미 5개면\n데미지 + 5");
		NameList.Add("드론\n\n갯수 + 1\n이미 2개면\n데미지 + 7");
		NameList.Add("추가 데미지\n\n\n+5%");
		NameList.Add("이동속도\n\n\n0.5 증가");
		NameList.Add("관통력\n\n\n+1");
		NameList.Add("공격속도\n\n\n0.05 감소");
		NameList.Add("재장전 속도\n\n\n0.1 감소");
		NameList.Add("최대 체력\n\n\n+10");

	}

	public void OnActivate()
	{
		// 상자 열었을 때 UI 나오는 모션
		PlayerInfo.Getinstance.set_pause(1);
		rand.Clear();
		// ** 총을 양손에 착용했을 때 보여지는 리스트
		if (!is_item)
		{
			rand = MakeRandomNumbers(0, 5);
			is_item = true;
		}
		else
		{
			rand = MakeRandomNumbers(2, WeaponList.Count);
		}
		for (int i = 0; i<3; ++i)
        {
			buttons[i].sprite = sprites[rand[i]];
			texts[i].text = NameList[rand[i]];
        }
		
		animator.SetBool("BoxOpen", true);
	}

	IEnumerator visible(int t)
    {
		// ** 서서히 보이는 함수
		float ton = 0;
		while(ton < 255)
        {
			ton += Time.deltaTime * 2f;
			texts[t].color = new Color(255, 255, 255, ton);

			yield return null;
        }
    }

	public void GetWeapon(int i)
	{
		Select.Play();
		// ** 선택한 무기 PlayerInfo로 전달
		if (WeaponList[rand[i]].name == "shield")
		{
			WeaponList[rand[i]].GetComponent<ShieldController>().SetAble(true);
			if (WeaponList[rand[i]].GetComponent<ShieldController>().getAmount() >= 5)
				WeaponList[rand[i]].GetComponent<ShieldController>().addDamage();
			else
				WeaponList[rand[i]].GetComponent<ShieldController>().addBullet();
			animator.SetBool("BoxOpen", false);
			PlayerInfo.Getinstance.set_pause(-1);
			return;
		}
		if(WeaponList[rand[i]].name == "drone")
        {
			WeaponList[rand[i]].GetComponent<DroneController>().setAble(true);
			if (WeaponList[rand[i]].GetComponent<DroneController>().getAmount() >= 3)
				WeaponList[rand[i]].GetComponent<DroneController>().setDamage(4);
			else
				WeaponList[rand[i]].GetComponent<DroneController>().AddDrone();
			animator.SetBool("BoxOpen", false);
			PlayerInfo.Getinstance.set_pause(-1);
			return;
        }
		if(WeaponList[rand[i]].name == "MineItem")
        {
			if (GameObject.Find("MineItem") == null)
				PlayerInfo.Getinstance.setWeapon(WeaponList[rand[i]]);
			else
				GameObject.Find("MineItem").GetComponent<MineController>().setDamage(9);
			animator.SetBool("BoxOpen", false);
			PlayerInfo.Getinstance.set_pause(-1);
			return;
		}
		if(WeaponList[rand[i]].name == "AdditionalDamage") 	PlayerInfo.Getinstance.PassiveUpgrade(0);
		if(WeaponList[rand[i]].name == "MovementSpeed") PlayerInfo.Getinstance.PassiveUpgrade(1);
		if(WeaponList[rand[i]].name == "Piercing") PlayerInfo.Getinstance.PassiveUpgrade(2);
		if(WeaponList[rand[i]].name == "AttackSpeed") 	PlayerInfo.Getinstance.PassiveUpgrade(3);
		if(WeaponList[rand[i]].name == "ReloadSpeed") PlayerInfo.Getinstance.PassiveUpgrade(4);
		if(WeaponList[rand[i]].name == "MaxHP") PlayerInfo.Getinstance.PassiveUpgrade(5);
		if(rand[i] < 2)	 PlayerInfo.Getinstance.setWeapon(WeaponList[rand[i]]);


		animator.SetBool("BoxOpen", false);
		PlayerInfo.Getinstance.set_pause(-1);
	}

	// ** 중복되는 숫자없이 임의의 숫자를 생성하고 반환하는 함수 리턴값은 List 형식으로 기존 배열형식으로 사용할려면 수정 필요
	public static List<int> MakeRandomNumbers(int minValue, int maxValue, int randomSeed = 0)
	{
		if (randomSeed == 0)
			randomSeed = (int)DateTime.Now.Ticks;

		List<int> values = new List<int>();
		for (int v = minValue; v < maxValue; v++)
		{
			values.Add(v);
		}

		List<int> result = new List<int>(maxValue - minValue);
		System.Random random = new System.Random(Seed: randomSeed);
		int i = 0;
		while (values.Count > 0)
		{
			int randomValue = values[random.Next(0, values.Count)];
			result.Add(randomValue);
			++i;

			if (!values.Remove(randomValue))
			{
				// Exception
				break;
			}
		}

		return result;
	}
}
