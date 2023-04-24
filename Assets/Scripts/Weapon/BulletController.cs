using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
	private float Speed;
	private float x;
	private float y;

	public float push_power;

	public int Penetrate;
	public int Damage;

	public float angle { get; set; }
	public Vector3 Direction;
	Quaternion quaternion;

	void Start()
	{
		Speed = 20.0f;
		push_power = 0.01f;
	}

	// Update is called once per frame
	void Update()
	{
		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			x = Mathf.Cos(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;
			y = Mathf.Sin(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;

			Direction = new Vector3(x, y, 0.0f);
			transform.position += Direction;
			transform.rotation = Quaternion.AngleAxis(angle-90f, Vector3.forward);
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
				if(enemyController)
				StartCoroutine(push(collision.transform.gameObject, 0.1f, -push_power));

				--Penetrate;
				if (Penetrate == 0)
					Destroy(this.gameObject);
			}
		}
	}

	// ** 총알이 적한테 닿았을 때 미는 힘
	IEnumerator push(GameObject _Obj,float t, float power)
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

	// ** 총알 삭제(최적화)
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "AmmoBlock")
			Destroy(this.gameObject);
	}

}
