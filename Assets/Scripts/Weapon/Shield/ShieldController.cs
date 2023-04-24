using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
	private float Speed;
	private int amount, damage;
	private GameObject bullet;
	private List<GameObject> bullets = new List<GameObject>();
	private bool Able;

    private void Awake()
    {
		bullet = Resources.Load("Prefabs/SubWeapon/ShieldEntity") as GameObject;
		damage = 10;
		amount = 1;
		Speed = 1.0f;
		Able = false;
    }

	// ** 삽 추가
	public void spawnBullet()
    {
		Vector3 playerPos = PlayerInfo.Getinstance.playerPos();
		for (int i = 0; i < amount; ++i)
		{
			GameObject obj = Instantiate(bullet);

			float _angle = (i * (Mathf.PI * 2.0f) / amount);

			obj.transform.position = playerPos + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f)) * 2.0f;
			ShieldDamage controller = obj.GetComponent<ShieldDamage>();
			controller.angle = _angle;
			controller.setDamage((int)(damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1]) * 0.1f)));
			controller.addSpeed(Speed);
			obj.transform.name = "Bullet";
			bullets.Add(obj);
			obj.transform.parent = gameObject.transform;
		}
	}

	// ** 삽 추가할때마다 위치 조정을 위해 삭제하고 다시 소환
    public void DestroyBullet()
    {
		for(int i = 0; i<bullets.Count; ++i)
        {
			Destroy(bullets[i]);
        }
		bullets.Clear();
    }

	// **  위 2개 함수를 합쳐 하나의 함수로 압축
	public void addBullet()
    {
		if (Able)
		{
			amount += 1;
			DestroyBullet();
			spawnBullet();
		}
    }

	// ** 삽 업그레이드
	public void addDamage()
    {
		damage += 5;
		for(int i = 0; i<bullets.Count; ++i)
        {
			bullets[i].GetComponent<ShieldDamage>().setDamage((int)(damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1] + SaveController.Getinstance.AchiveList[0]) * 0.1f)));
        }
    }

	public void addSpeed()
    {
		for (int i = 0; i < bullets.Count; ++i)
		{
			bullets[i].GetComponent<ShieldDamage>().addSpeed(0.2f);
		}
		Speed += 0.2f;
	}

	public int getDamage() { return (int)(damage * (1f + (PlayerInfo.Getinstance.PlayerStats[1] + SaveController.Getinstance.AchiveList[0]) * 0.1f)); }
	public int getAmount() { return amount; }
	public float getSpeed() { return Speed; }
	public bool getAble() { return Able; }
	public void SetAble(bool i) { Able = i; }
}
