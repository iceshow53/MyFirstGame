using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneActive : MonoBehaviour
{
	private bool CanFire;
	private float cooltime, fire_speed;
	private int damage, Penetrate;

	private float Speed;

	private GameObject Bullet, Enemy;
	private List<GameObject> EnemyList = new List<GameObject>();
	private float angle, f_angle, shortDis, dis;
	private Vector3 bullet, target;
	private GameObject TargetInfo;

	private AudioSource Shoot;

	private void Awake()
    {
		Bullet = Resources.Load("Prefabs/Weapons/Bullet") as GameObject;
		// 가장 가까운 적을 계산할 적 리스트
		Enemy = GameObject.Find("EnemyList");
		Shoot = GameObject.Find("Shoot").GetComponent<AudioSource>();
    }

	private void Start()
	{
		CanFire = true;
		Speed = 0f;
		f_angle = 0f;
		StartCoroutine(DroneChase());
	}

	private void Update()
	{
		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			targetSelect();

			if (!CanFire)
			{
				if (cooltime > 0)
					cooltime -= Time.deltaTime;
				else
				{
					if (fire_speed < 0.1f)
						cooltime = 0.1f;
					else cooltime = fire_speed;
					CanFire = true;
				}
			}
			else
			{
				if (EnemyList != null)
					fire();
			}

		}
	}

	private void fire()
	{
		Shoot.Play();
		CanFire = false;
		GameObject Obj = Instantiate(Bullet);
		BulletController controller = Obj.AddComponent<BulletController>();
		bullet = transform.position;
		angle = Mathf.Atan2(target.y - bullet.y, target.x - bullet.x) * Mathf.Rad2Deg;
		Obj.transform.position = transform.position;
		controller.angle = angle;
		controller.Penetrate = Penetrate;
		controller.Damage = damage;
	}

	// ** 가장 가까운 적 판별 직접 만든거라 하자있음 나중에 더 완벽한 코드 찾길바람
	private void targetSelect()
    {
		if (!Enemy.transform.GetChild(0).GetComponent<EnemyController>().getdead())
		{
			target = Enemy.transform.GetChild(0).position;
			shortDis = Vector3.Distance(PlayerInfo.Getinstance.playerPos(), target);
		}
		else
			shortDis = 15.0f;
		for (int i = 0; i < Enemy.transform.childCount; ++i)
		{
			if (!Enemy.transform.GetChild(i).GetComponent<EnemyController>().getdead())
			{
				dis = Vector3.Distance(PlayerInfo.Getinstance.playerPos(), Enemy.transform.GetChild(i).position);
				if (dis < shortDis)
				{
					target = Enemy.transform.GetChild(i).position;
					shortDis = Vector3.Distance(PlayerInfo.Getinstance.playerPos(), target);
				}
			}
		}
	}

	// ** 드론 업그레이드 할 때 적용할 함수
	public void DroneSetting(int _damage, int _Penetrate, float _fireSpeed)
    {
		damage = _damage;
		Penetrate = _Penetrate;
		fire_speed = _fireSpeed;
    }
	
	// ** 가속도를 설정할 코루틴 함수인데 이미 한곳에 때려박아서 참고용
	IEnumerator TransSpeed(int i)
    {
		switch(i)
        {
			case 0:
				while(Speed > 0.0f)
                {
					Speed -= Time.deltaTime;
					yield return null;
                }
				Speed = 0.0f;
				break;
			case 1:
				while (Speed < 1.0f)
				{
					Speed += Time.deltaTime;
					yield return null;
				}
				Speed = 1.0f;
				break;
			case 2:
				while(Speed > - 2.0f)
                {
					Speed -= Time.deltaTime;
					yield return null;
                }
				Speed = -2.0f;
				break;
        }
    }

	// ** 플레이어와 일정거리 내에 있을 때 적을 향해 따라가고 일정거리 밖으로 나가면 다시 돌아오는 코루틴 함수 적이 죽었을 때...
	IEnumerator DroneChase()
    {
		while(true)
        {
			if (PlayerInfo.Getinstance.is_paused() == 0)
			{
				if (Vector3.Distance(PlayerInfo.Getinstance.playerPos(), transform.position) < 3)
				{
					if (Speed < 0.0f)
					{
						Speed += Time.deltaTime * 3;
						angle = Mathf.Atan2(PlayerInfo.Getinstance.playerPos().y - transform.position.y, PlayerInfo.Getinstance.playerPos().x - transform.position.x) * Mathf.Rad2Deg;
						transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, Mathf.Sin(angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, 0.0f);
					}
					else
					{
						if (Vector3.Distance(transform.position, target) < 0.5f)
						{
							if (Speed > 0.0f)
								Speed -= Time.deltaTime * 2;
							if (angle > f_angle)
							{
								f_angle += Time.deltaTime * 240;
								transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
							}
							else if (angle < f_angle)
							{
								f_angle -= Time.deltaTime * 240;
								transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
							}
							else
								transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
						}
						else
						{
							if (Speed < 2.0f)
								Speed += Time.deltaTime;
						}
						angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
						if (angle > f_angle)
						{
							f_angle += Time.deltaTime * 240;
							transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
						}
						else if (angle < f_angle)
						{
							f_angle -= Time.deltaTime * 240;
							transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
						}
						else
							transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, Mathf.Sin(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime, 0.0f);
					}
				}
				else if (Vector3.Distance(PlayerInfo.Getinstance.playerPos(), transform.position) >= 3)
				{
					if (Speed > (PlayerInfo.Getinstance.getSpeed() * -1) - 1.0f)
						Speed -= Time.deltaTime * 1.5f;
					angle = Mathf.Atan2(PlayerInfo.Getinstance.playerPos().y - transform.position.y, PlayerInfo.Getinstance.playerPos().x - transform.position.x) * Mathf.Rad2Deg;
					if (angle > f_angle)
					{
						f_angle += Time.deltaTime * 300;
						transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, 0.0f);
					}
					else if (angle < f_angle)
					{
						f_angle -= Time.deltaTime * 300;
						transform.position += new Vector3(Mathf.Cos(f_angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, Mathf.Sin(f_angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, 0.0f);
					}
					else
						transform.position += new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, Mathf.Sin(angle * Mathf.Deg2Rad) * (Speed * -1) * Time.deltaTime, 0.0f);
				}
			}
			yield return null;
        }
    }
}
