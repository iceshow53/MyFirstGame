using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
	private GameObject Drone;
	public List<GameObject> Drones = new List<GameObject>();
	private int Damage, Amount, Penetrate, MaxAmount;
	private bool Able;
	private float AS;


	private void Awake()
	{
		Drone = Resources.Load("Prefabs/SubWeapon/Drone") as GameObject;
		Damage = 10;
		Penetrate = 1;
		Amount = 0;
		AS = 1.0f;
		MaxAmount = 3 + SaveController.Getinstance.AchiveList[2];
		Able = false;
	}

	// ** 드론 추가
	public void AddDrone()
	{
		if (Able)
		{
			GameObject _obj = Instantiate(Drone);
			Amount += 1;
			if (Amount > MaxAmount)
				return;

			_obj.transform.position = PlayerInfo.Getinstance.playerPos();

			_obj.GetComponent<DroneActive>().DroneSetting((int)(Damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f)), Penetrate + PlayerInfo.Getinstance.PlayerStats[8], AS);
			Drones.Add(_obj);
			_obj.transform.parent = GameObject.Find("DroneTest").transform;
		}
	}

	// ** 드론 업그레이드 목록
	public void setDamage(int i) 
	{ 
		Damage += i;
		for (int j = 0; j < Drones.Count; ++j)
			Drones[j].GetComponent<DroneActive>().DroneSetting((int)(Damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f)), Penetrate + PlayerInfo.Getinstance.PlayerStats[8], AS);
	}
	public void setPenetrate(int i) 
	{ 
		Penetrate += i;
		for (int j = 0; j < Drones.Count; ++j)
			Drones[j].GetComponent<DroneActive>().DroneSetting((int)(Damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f)), Penetrate + PlayerInfo.Getinstance.PlayerStats[8], AS);
	}
	public void setAS(float i) 
	{ 
		AS += i;
		if (AS < 0.1f)
			AS = 0.1f;
		for (int j = 0; j < Drones.Count; ++j)
			Drones[j].GetComponent<DroneActive>().DroneSetting((int)(Damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f)), Penetrate + PlayerInfo.Getinstance.PlayerStats[8], AS);
	}

	// ** 드론 정보 반환
	public int getAmount() { return Amount; }
	public int getPenetrate() { return Penetrate; }
	public int getDamage() { return Damage; }
	public float getAS() { return AS; }

	// ** 드론 사용 가능한지 여부
	public void setAble(bool i) { Able = i; }
	public bool getAble() { return Able; }
}
