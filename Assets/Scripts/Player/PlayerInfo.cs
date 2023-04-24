using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
	private PlayerInfo() { }
	private static PlayerInfo Instance = null;
	private GameObject ui;

	enum WeaponList
	{
		Sniper,
		Shotgun,
		Machinegune
	}

	public static PlayerInfo Getinstance
	{
		get
		{
			if (Instance == null)
				return null;
			return Instance;
		}
	}



	public GameObject[] Weapons = new GameObject[2];
	public GameObject[] SubWeapons = new GameObject[4];
	private GameObject player;
	private PlayerContoller playerContoller;
	private int HP, M_HP, Level, Penetrate, Damage, Money, Kill, LevelPoint, baseDamage;
	private float Speed, EXP, AttackSpeed, ReloadSpeed, Add_Damage;
	public int is_pause;
	private bool ShopOpen;

	public List<int> PlayerStats = new List<int>();


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			player = GameObject.Find("Player");
			playerContoller = player.GetComponent<PlayerContoller>();
			PlayerStats = SaveController.Getinstance.getStatData();
			ui = GameObject.Find("UI");
			HP = 100 + (PlayerStats[0] * 100);
			M_HP = 100 + (PlayerStats[0] * 100);
			Money = Kill = 0;
			LevelPoint = 1; // ** 테스트용 스킬포인트 빌드할때는 1으로 수정
			Level = Penetrate = 1;
			Add_Damage = 1.0f + ((PlayerStats[1] + SaveController.Getinstance.AchiveList[8]) * 0.1f); // ** 데미지 배율
			baseDamage = 10;
			Speed = 3.0f;
			EXP = 0f;
			AttackSpeed = 0.5f;
			ReloadSpeed = 2.0f;
			is_pause = 0;
			ShopOpen = false;
		}
	}

    private void Start()
    {
		Damage = (int)(baseDamage * Add_Damage); // ** 플레이어 데미지는 모든 총기와 보조무기 통합예정 일단 추가데미지만 적용
		AttackSpeed *= 1f - ((PlayerStats[6] + SaveController.Getinstance.AchiveList[4]) * 0.1f);
		ReloadSpeed *= 1f - ((PlayerStats[5] + SaveController.Getinstance.AchiveList[5]) * 0.1f);
		Speed *= 1f + ((PlayerStats[4] + SaveController.Getinstance.AchiveList[3]) * 0.1f);
		Penetrate += PlayerStats[8] + SaveController.Getinstance.AchiveList[7];
    }

    // ** 플레이어 위치정보
    public Vector3 playerPos() { return player.transform.position; }
	public float getAddDmg() { return Add_Damage; }
	// ** 플레이어 스탯 정보 수정 및 확인
	public int getHP() { return HP; }
	public int getMHP() { return M_HP; }
	public void setHP(int i) { HP = i; }
	public void addHP(int i) { HP += i; }
	public int getLevel() { return Level; }
	public void setLevel(int i) { Level += i; }
	public int getLPoint() { return LevelPoint; }
	public void setLPoint(int i) { LevelPoint += i; }
	public int getPenetrate() { return Penetrate; }
	public void setPenetrate(int i) { Penetrate += i; }
	public int getDamage() { return Damage; }
	public void setDamage(int i) { Damage += i; }
	public float getSpeed() { return Speed; }
	public void setSpeed(float i) { Speed += i; }
	public float getAttackSpeed() { return AttackSpeed; }
	public void setAttackSpeed(float i) { AttackSpeed -= i; }
	public float getReloadSpeed() { return ReloadSpeed; }
	public void setReloadSpeed(float i) { ReloadSpeed -= i; }
	public int getMoney() { return Money; }
	public void setMoney(int i) { Money += i; }
	public int getKilled() { return Kill; }
	public float getEXP() { return EXP; }
	public void setOpened(bool open) { ShopOpen = open; }
	public bool getOpened() { return ShopOpen; }
	// **

	// ** 플레이어 무기 장착 및 해제
	public void setWeapon(GameObject weapon) 
	{
		if(weapon.tag == "SubWeapon")
        {
			if(weapon.name == "MineItem")
            {
				GameObject mine = Instantiate(weapon);
				mine.transform.parent = player.transform.Find("SubWeapon");
				mine.name = "MineItem";
				mine.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
            }
			return;
        }

		if (Weapons[0] == null)
		{
			Weapons[0] = weapon;
			GameObject Weapon0 = Instantiate(Weapons[0]);
			Weapon0.transform.parent = player.transform.Find("Weapon");
			Weapon0.name = "Weapon0";
			Weapon0.transform.position = new Vector3(player.transform.position.x - 0.311f, player.transform.position.y - 0.111f, 0.0f);
		}
		else
		{
			if (Weapons[1] == null)
			{
				Weapons[1] = weapon;
				GameObject Weapon1 = Instantiate(Weapons[1]);
				Weapon1.name = "Weapon1";
				Weapon1.transform.position = new Vector3(player.transform.position.x + 0.311f, player.transform.position.y - 0.111f, 0.0f);
				Weapon1.transform.parent = player.transform.Find("Weapon");
			}
		}

		if(Weapons[0] != null && Weapons[1] != null) { }
	}
	public void dropWeapon(int i) 
	{
		// ** 무기 버리는 코드
		if (Weapons[i] != null)
		{
			Weapons[i] = null;
			switch(i)
            {
				case 0:
					Destroy(GameObject.Find("Weapon0"));
					break;
				case 1:
					Destroy(GameObject.Find("Weapon1"));
					break;
            }
		}
	}
	public GameObject getWeapon(int i) { return Weapons[i]; }

	// ** 게임내 모든 정보들을 일시정지 추후 게임매니저로 옮길예정?
	public int is_paused() { return is_pause; }
	public void set_pause(int i) { is_pause += i; }

	// ** 몬스터가 죽었을때 경험치와 돈을 받아오는 함수
	public void Monster_kill(float exp, int money) 
	{ 
		EXP += exp; 
		if(EXP >= 1f)
		{
			EXP -= 1f;
			Level += 1;
			LevelPoint += 1;
		}
		Kill += 1; 
		Money += money;
	}

	public void PassiveUpgrade(int i)
	{
		switch(i)
		{
			case 0:
				Add_Damage += 0.05f;
				Damage = (int)(baseDamage * Add_Damage); // ** 플레이어 데미지는 모든 총기와 보조무기 통합예정 일단 추가데미지만 적용
				break;
			case 1:
				Speed += 0.5f;
				break;
			case 2:
				Penetrate += 1;
				break;
			case 3:
				AttackSpeed -= 0.05f;
				break;
			case 4:
				ReloadSpeed -= 0.1f;
				break;
			case 5:
				M_HP += 10;
				break;
		}
	}
}
