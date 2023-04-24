using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
	private float Speed;
	private float x, y;
	private int Damage;

	public bool RoundAttack;

	public Vector3 position;

	public float angle { get; set; }
	private float new_angle;
	public Vector3 Direction;
	Quaternion quaternion;

    private void Awake()
    {
		RoundAttack = false;
    }

    void Start()
	{
		Speed = 5.0f;
		Damage = 5;
		new_angle = 0.0f;
	}

	void Update()
	{
		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			if (RoundAttack)
			{
				new_angle += Time.deltaTime * Speed * 1f;
				angle += Time.deltaTime * Speed * 0.5f;
				if (GameObject.Find("TrueBoss") == null)
					Destroy(gameObject);
				transform.position = position + (new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0.0f)) * new_angle;
			}
			else
			{
				x = Mathf.Cos(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;
				y = Mathf.Sin(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;
				Direction = new Vector3(x, y, 0.0f);
				transform.position += Direction;
				transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			PlayerContoller ply = collision.GetComponent<PlayerContoller>();
			ply.OnHit(Damage);

			Destroy(gameObject);
		}
		if (collision.name == "DONOTCROSS")
			Destroy(gameObject);
	}
}
