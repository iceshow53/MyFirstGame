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
		// ** ���� ����Ʈ
		WeaponList.Add(Resources.Load("Prefabs/Weapons/Shotgun") as GameObject);
		WeaponList.Add(Resources.Load("Prefabs/Weapons/MachineGun") as GameObject);
		WeaponList.Add(Resources.Load("Prefabs/SubWeapon/MineItem") as GameObject);

		// ** ��������
		WeaponList.Add(GameObject.Find("shield"));
		WeaponList.Add(GameObject.Find("drone"));

		// ** �нú�
		GameObject passive = GameObject.Find("Passive");
		for(int i = 0; i < passive.transform.childCount;++i)
        {
			WeaponList.Add(passive.transform.GetChild(i).gameObject);
        }
		/*
		�нú� ����Ʈ
		0 �߰� ������
		1 �̵��ӵ�
		2 ����
		3 ����
		4 ������
		5 �ִ� ü��
		 */

		// ** ���� ���� �߰� ������ ���⸮��Ʈ���� ���ʴ��
		NameList.Add("����\n\nźâ : 10��\n�߰� ������ : 1\n�߻� �ӵ� : -0.2\n������ �ӵ� : -0.3��");
		NameList.Add("�ӽŰ�\n\nźâ : 30��\n �߻� �ӵ� :\n+ 0.1");
		NameList.Add("����\n\n�⺻ ��ġ �ӵ� : 9.0��\n������ : 10\n���� : 2\n�̹� ������ �ִٸ�\n������ +9");
		NameList.Add("��\n\n���� + 1\n�̹� 5����\n������ + 5");
		NameList.Add("���\n\n���� + 1\n�̹� 2����\n������ + 7");
		NameList.Add("�߰� ������\n\n\n+5%");
		NameList.Add("�̵��ӵ�\n\n\n0.5 ����");
		NameList.Add("�����\n\n\n+1");
		NameList.Add("���ݼӵ�\n\n\n0.05 ����");
		NameList.Add("������ �ӵ�\n\n\n0.1 ����");
		NameList.Add("�ִ� ü��\n\n\n+10");

	}

	public void OnActivate()
	{
		// ���� ������ �� UI ������ ���
		PlayerInfo.Getinstance.set_pause(1);
		rand.Clear();
		// ** ���� ��տ� �������� �� �������� ����Ʈ
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
		// ** ������ ���̴� �Լ�
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
		// ** ������ ���� PlayerInfo�� ����
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

	// ** �ߺ��Ǵ� ���ھ��� ������ ���ڸ� �����ϰ� ��ȯ�ϴ� �Լ� ���ϰ��� List �������� ���� �迭�������� ����ҷ��� ���� �ʿ�
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
