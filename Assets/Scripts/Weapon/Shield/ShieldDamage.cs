using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDamage : MonoBehaviour
{
	private float _angle, Speed, push_power;
	public float angle;

	private int Damage;

	void Start()
	{
		push_power = 0.05f;
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			angle += Time.deltaTime * Speed;
			transform.position = PlayerInfo.Getinstance.playerPos() + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f)) * 2.0f;
			thisAngle();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Enemy")
		{
			EnemyController enemyController = collision.GetComponent<EnemyController>();
			if (enemyController.HP != 0)
			{
				enemyController.HP -= Damage;
				enemyController.onHit();
				if (enemyController)
					StartCoroutine(push(collision.transform.gameObject, 0.1f, -push_power));
			}
		}
	}

	IEnumerator push(GameObject _Obj, float t, float power)
	{
		EnemyController enemyController = _Obj.GetComponent<EnemyController>();

		Vector3 objPos = _Obj.transform.position;
		Vector3 target = PlayerInfo.Getinstance.playerPos();
		Vector3 direction = new Vector3(target.x - objPos.x, target.y - objPos.y, 0.0f).normalized;

		while (t > 0.0f)
		{
			t -= Time.deltaTime;

			//enemyController.angle;
			if (!enemyController.getdead())
			{
				_Obj.transform.position = Vector3.Lerp(_Obj.transform.position,
					_Obj.transform.position + (direction * power),
					1f - t);
			}

			yield return null;
		}
	}

	void thisAngle()
	{
		Vector3 target = transform.position;
		Vector3 player = PlayerInfo.Getinstance.playerPos();
		Vector3 Direction = (player - target).normalized;
		float Aangle = Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg;

		transform.rotation = Quaternion.AngleAxis(-Aangle - 180f, Vector3.forward);
	}

	public void addDamage(int i)
	{
		Damage += i;
	}

	public void setDamage(int i) { Damage = i; }

	public void addSpeed(float i)
    {
		Speed += i;
    }
}

