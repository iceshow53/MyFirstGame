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

	// ** ź�� ���ư��鼭 �����ֱ�� ź�� �¿�� ���ư�
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

	// ** -90�� ������ �߻�
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

	// ** 90�� ������ �߻�
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

	// ** �÷��̾� �ֺ� ���׶��� ź�� ������ �÷��̾� �������� ���� ���� ��� ǥ���ϰų� ���� ��� �� �����ð� ü���ϴ� ����� �����Ͱ���.
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

	// ** ������Ʈ �������� �÷��̾�� źȯ�� �л������� �߻�
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

	// ** ���� �������� ���� ���鼭 ���ư��� ź �߻�
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

	// ** �� ������ �������� �ϳ� ������ ����
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
