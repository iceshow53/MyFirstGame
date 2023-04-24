using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPattern : MonoBehaviour
{
	private GameObject Bullet;
	private List<GameObject> Control = new List<GameObject>();
	private int count;
	private float cool;

	private void Awake()
	{
		Bullet = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
		count = 0;
		cool = 5.0f;
	}

	void Start()
	{
		StartCoroutine(Continue());
	}

	// ** 탄이 날아가면서 일정주기로 탄이 좌우로 날아감
	private void SplitAttack()
	{
		GameObject _obj = Instantiate(Bullet);
		Control.Add(_obj);
		Control[count].transform.position = transform.position;
		Vector3 target = PlayerInfo.Getinstance.playerPos();
		float angle = Mathf.Atan2(target.y - Control[count].transform.position.y, target.x - Control[count].transform.position.x) * Mathf.Rad2Deg;
		EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
		controller.angle = angle;
		StartCoroutine(Increase(angle,count));
		StartCoroutine(Decrease(angle,count));

		++count;
	}

	// ** -90도 각도로 발사
	IEnumerator Increase(float angle,int _count)
	{
		while (Control[_count] != null)
		{
			GameObject _obj = Instantiate(Bullet);
			float _angle = angle - 90f;
			_obj.transform.position = Control[_count].transform.position;

			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.angle = _angle;
			yield return new WaitForSeconds(1.0f);
		}
	}

	// ** 90도 각도로 발사
	IEnumerator Decrease(float angle, int _count)
	{
		while (Control[_count] != null)
		{
			GameObject _obj = Instantiate(Bullet);
			float _angle = angle + 90f;
			_obj.transform.position = Control[_count].transform.position;

			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.angle = _angle;
			yield return new WaitForSeconds(1.0f);
		}
	}

	// ** 플레이어 주변 동그랗게 탄을 생성해 플레이어 방향으로 공격 사전 경고 표시하거나 공격 출발 전 일정시간 체류하는 방법이 좋을것같음.
	private void RoundAttack()
	{
		int random = Random.Range(0, 5);
		for(int i = 0; i<5; ++i)
		{
			float _angle = (i + random * (Mathf.PI * 2.0f) / 5);
			GameObject _obj = Instantiate(Bullet);

			_obj.transform.position = PlayerInfo.Getinstance.playerPos() + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f)) * 5.0f;
			Vector3 target = PlayerInfo.Getinstance.playerPos();
			float angle = Mathf.Atan2(target.y - _obj.transform.position.y, target.x - _obj.transform.position.x) * Mathf.Rad2Deg;
			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.RoundAttack = false;
			controller.angle = angle;
		}
	}

	private void BossRoundAttack()
	{
		for (int i = 0; i < 25; ++i)
		{
			float _angle = (i * (Mathf.PI * 2.0f) / 25);
			GameObject _obj = Instantiate(Bullet);

			Vector3 target = transform.position;
			_obj.transform.position = transform.position + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f));
			float angle = Mathf.Atan2(target.y - _obj.transform.position.y, target.x - _obj.transform.position.x) * Mathf.Rad2Deg;
			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.RoundAttack = false;
			controller.angle = angle;
		}
	}

	// ** 오브젝트 기준으로 플레이어에게 탄환을 분사형으로 발사
	private void ShotgunAttack()
	{
		Vector3 target = PlayerInfo.Getinstance.playerPos();
		float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
		angle -= (3 * (Mathf.PI * 2.0f) / 2);

		float random = Random.Range(0.0f, 360.0f);
		for(int i = 0; i<5; ++i)
		{
			float _angle = (i * (Mathf.PI * 2.0f) / 2) + angle;
			GameObject _obj = Instantiate(Bullet);
			_obj.transform.position = transform.position;
			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.RoundAttack = false;
			controller.angle = _angle;
		}
	}

	// ** 몬스터 기준으로 빙빙 돌면서 나아가는 탄 발사
	private void AroundAttack()
	{
		for (int i = 0; i < 5; ++i)
		{
			float _angle = (i * (Mathf.PI * 2.0f) / 5);
			GameObject _obj = Instantiate(Bullet);
			_obj.transform.position = transform.position + (new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0.0f)) * 2.0f;
			_obj.transform.name = "roundBullet";
			EnemyBulletController controller = _obj.GetComponent<EnemyBulletController>();
			controller.position = transform.position;
			controller.RoundAttack = true;
			controller.angle = _angle;
		}
	}

	// ** 위 패턴중 랜덤으로 하나 선택해 시전
	IEnumerator Continue()
	{
		while (true)
		{
			while (cool > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				cool -= Time.deltaTime;
				yield return null;
			}
			cool = 5.0f;

			switch (Random.Range(0, 5))
			{
				case 0:
					RoundAttack();
					break;
				case 1:
					SplitAttack();
					break;
				case 2:
					ShotgunAttack();
					break;
				case 3:
					AroundAttack();
					break;
				case 4:
					BossRoundAttack();
					break;

			}
			yield return null;
		}
	}

	public void Dead()
    {
		StopAllCoroutines();
    }
}
