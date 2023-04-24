using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Monster
{
	Zombie,
	Skeleton,
	Tombstone
}

public class EnemyManager : MonoBehaviour
{
	private EnemyManager() { }
	private static EnemyManager Instance = null;

	public static EnemyManager GetInstance
	{
		get
		{
			if (Instance == null)
				return null;
			return Instance;
		}
	}

	private GameObject EnemyList, Zombie, Skeleton, Boss, TrueBoss;
	public GameObject HardModeBox;
	private GameObject[] MonsterList;
	private float CoolDown, BossCool, StrongCool, skeletonCool;
	public int HP, Money, Damage;
	private int Waves, Zombie_Base_HP,Skeleton_Base_HP;
	private float radius, Add_EXP;
	private bool is_spawned;

	private UIController uIController;

    private void Awake()
    {
        if(Instance == null)
        {
			Instance = this;

			EnemyList = GameObject.Find("EnemyList");
			Zombie = Resources.Load("Prefabs/Enemy/Zombie") as GameObject;
			Skeleton = Resources.Load("Prefabs/Enemy/Skeleton") as GameObject;
			Boss = Resources.Load("Prefabs/Enemy/Boss") as GameObject;
			TrueBoss = Resources.Load("Prefabs/Enemy/TrueBoss") as GameObject;
			uIController = GameObject.Find("UI").GetComponent<UIController>();

			if (!SaveController.Getinstance.hardmod)
				Destroy(HardModeBox);
		}
    }

    private void Start()
    {
		is_spawned = false;
		skeletonCool = 5.0f;
		CoolDown = 40.0f;
		BossCool = 10.0f;
		StrongCool = 60.0f;
		Waves = 20;
		HP = Money = 0 ;
		Damage = 1;
		Zombie_Base_HP = 2;
		Skeleton_Base_HP = 2;
		radius = 15.0f;
		Add_EXP = PlayerInfo.Getinstance.PlayerStats[3] * 0.1f;
		StartCoroutine(GameStart());
		if (SaveController.Getinstance.hardmod)
			StartCoroutine(zzenlwufk());
		else
			StartCoroutine(SkeletonsStart());
		StartCoroutine(ZombiesWaves());
		StartCoroutine(BossWave());
		StartCoroutine(EnemyStrong());
    }

	// ** 하드모드 일반몬스터
	public IEnumerator zzenlwufk()
	{
		while (true)
		{
			while (skeletonCool > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				skeletonCool -= Time.deltaTime;
				yield return null;
			}
			zzBossSpawn();
			skeletonCool = 5.0f;
			yield return null;
		}
	}

	public IEnumerator GameStart()
    {
		while (true)
		{
			while (PlayerInfo.Getinstance.is_paused() > 0)
            {
				yield return null;
            }
			Zombies();
			if (uIController == null)
				uIController = GameObject.Find("UI").GetComponent<UIController>();
			else
			{
				if (uIController.getRemain() < 60.0f)
					if (!is_spawned)
						BossSpawn();
			}
			yield return new WaitForSeconds(1.0f);
		}
    }

	// ** 찐보스 소환
	private void BossSpawn()
    {
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();
		float _angle = Random.Range(0.0f, 360.0f);
		GameObject obj = Instantiate(TrueBoss);
		obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0)) * radius;
		is_spawned = true;
		EnemyController controller = obj.GetComponent<EnemyController>();
		if(SaveController.Getinstance.AchiveList[5] == 0)
			controller.item_drop = true;
		else
			controller.item_drop = false;
		controller.is_boss = true;
		obj.transform.name = "TrueBoss";
		obj.transform.parent = EnemyList.transform;
	}	

	// ** 하드모드용 몬스터 소환
	private void zzBossSpawn()
    {
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();
		float _angle = Random.Range(0.0f, 360.0f);
		GameObject obj = Instantiate(TrueBoss);
		obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0)) * radius;
		is_spawned = true;
		EnemyController controller = obj.GetComponent<EnemyController>();
		controller.item_drop = false;
		controller.HP = 150 * PlayerInfo.Getinstance.getLevel();
		controller.Damage = 50;
		controller.money = 250;
		controller.exp = 0.5f * (1.0f + Add_EXP);
		obj.transform.name = "TrueBoss";
		obj.transform.parent = EnemyList.transform;
	}

	// ** 기본 좀비
	private void Zombies()
	{
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();

		float _angle = Random.Range(0.0f, 360.0f);

		GameObject obj = Instantiate(Zombie);
		obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0)) * radius;

		EnemyController controller = obj.GetComponent<EnemyController>();
		controller.item_drop = false;
		if (SaveController.Getinstance.hardmod)
		{
			controller.HP = 100 * PlayerInfo.Getinstance.getLevel();
			controller.exp = 0.5f;
			controller.Damage = 5 * Damage;
			controller.Speed = 3.5f;
		}
		else
		{
			controller.HP = Zombie_Base_HP * PlayerInfo.Getinstance.getLevel();
			controller.exp = 0.1f * (1.0f + Add_EXP);
		}
		controller.money = 10 * PlayerInfo.Getinstance.getLevel();
		obj.transform.name = "Zombie";
		obj.transform.parent = EnemyList.transform;
	}



	public IEnumerator ZombiesWaves()
    {
		while (true)
		{
			while (CoolDown > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				CoolDown -= Time.deltaTime;
				yield return null;
			}
			Zombies_Wave();
			CoolDown = 20.0f;
			yield return null;
        }
	}

	// ** 좀비가 여러마리 몰려오는 함수
	private void Zombies_Wave()
    {
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();

		for (int i = 0; i < Waves; ++i)
        {
			GameObject obj = Instantiate(Zombie);

			float _angle = (i * (Mathf.PI * 2.0f) / Waves) + Random.Range(0f,3.0f);

			obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f)) * Random.Range(radius - 1.5f, radius + 1.5f);
			EnemyController controller = obj.GetComponent<EnemyController>();
			controller.item_drop = false;
			if (SaveController.Getinstance.hardmod)
			{
				controller.HP = 100 * PlayerInfo.Getinstance.getLevel();
				controller.Damage = 5 * Damage;
				controller.exp = 0.3f;
				controller.Speed = 3.5f;
			}
			else
			{
				controller.HP = Zombie_Base_HP * PlayerInfo.Getinstance.getLevel();
				controller.exp = 0.1f * (1.0f + Add_EXP);
			}
			controller.money = 10 * PlayerInfo.Getinstance.getLevel();
			obj.GetComponent<EnemyController>().item_drop = false;
			obj.transform.name = "Zombie";
			obj.transform.parent = EnemyList.transform;
		}
	}

	
	public IEnumerator SkeletonsStart()
	{
		while (true)
		{
			while (skeletonCool > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				skeletonCool -= Time.deltaTime;
				yield return null;
			}
			Skeletons();
			skeletonCool = 7.0f;
			yield return null;
		}
	}

	// ** 원거리 공격 몬스터
	private void Skeletons()
    {
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();

		float _angle = Random.Range(0.0f, 360.0f);

		GameObject obj = Instantiate(Skeleton);
		obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0)) * radius;
		EnemyController controller = obj.GetComponent<EnemyController>();
		controller.item_drop = false;
		controller.HP = Skeleton_Base_HP * PlayerInfo.Getinstance.getLevel();
		controller.money = 10 * PlayerInfo.Getinstance.getLevel();
		controller.exp = 0.2f * (1.0f + Add_EXP);
		obj.transform.name = "Skeleton";
		obj.transform.parent = EnemyList.transform;
	}


	
	public IEnumerator BossWave()
    {
		while(true)
        {
			while (BossCool > 0)
            {
				while (PlayerInfo.Getinstance.is_paused() > 0) 
					yield return null;
				BossCool -= Time.deltaTime;
				yield return null;
            }
			BossSetting();
			BossCool = 20.0f;
			yield return null;
        }
    }

	// ** 상자를 떨어트리는 엘리트 몬스터 소환
	private void BossSetting()
    {
		GameObject obj = Instantiate(Boss);
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();

		float _angle = Random.Range(0.0f, 360.0f);

		obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0)) * radius;
		EnemyController controller = obj.GetComponent<EnemyController>();
		controller.item_drop = true;
		controller.HP = 10 * PlayerInfo.Getinstance.getLevel();
		controller.money = 50 * PlayerInfo.Getinstance.getLevel();
		controller.exp = 0.8f * (1.0f + Add_EXP);
		obj.transform.name = "Boss";
		obj.transform.parent = EnemyList.transform;
	}

	public IEnumerator EnemyStrong()
    {
		while(true)
        {
			while (StrongCool > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				StrongCool -= Time.deltaTime;
				yield return null;
			}
			EnemySetting();
			StrongCool = 20.0f;
			yield return null;
		}
    }

	// ** 시간에 따라 적이 성장하는 함수
	public void EnemySetting()
    {
		HP = 3 * PlayerInfo.Getinstance.getLevel();
		Money += 20;
		if(Damage < 30)
			Damage += 1;
		Waves += 10;
    }

	
}
