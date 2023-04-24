using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
	private int Damage;
	private float range, mineCooltime, initial_c;
	private GameObject mine;


	private void Awake()
	{
		mine = Resources.Load("Prefabs/Weapons/Mine") as GameObject;

		// ** ���� �⺻ ����
		mineCooltime = 1.0f;
		initial_c = 9.0f;
		Damage = 10;
		range = 2.0f;

	}

	// Start is called before the first frame update
	void Start()
	{
		// ** �κ��丮�� ������ ���� ��ġ ����
		StartCoroutine(SpawningMine());
	}

	// ** ���� ��ġ ���� �����?
	public void StartMine() { StartCoroutine(SpawningMine()); }

	IEnumerator SpawningMine()
	{
		while (true)
		{
			while (mineCooltime > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				mineCooltime -= Time.deltaTime;
				yield return null;
			}
			spawnMine();
			mineCooltime = initial_c;
			yield return null;
		}
	}
	private void spawnMine()
	{
		GameObject obj = Instantiate(mine);
		Mine controller = obj.GetComponent<Mine>();
		controller.Damage = (int)(Damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f));
		controller.Range = range;
		obj.transform.position = PlayerInfo.Getinstance.playerPos();
	}


	// ** ���� ���׷��̵�
	public void setDamage(int i) { Damage += i; }
	public int getDamage() { return Damage; }
	public void setRange(float i) { range += i; }
	public float getRange() { return range; }
	public void setCoolTime(float i) { initial_c -= i; }
	public float getCoolTime() { return initial_c; }
}
