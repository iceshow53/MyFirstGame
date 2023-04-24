using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
	public float Speed;
	public int HP, money, Damage;
	public float exp;
	public bool can_shoot;

	private float x,y;
	public float angle;
	private Vector3 target;
	private SpriteRenderer spriteRenderer;
	private Animator animator;

	public GameObject drop_item;
	public bool item_drop;
	public bool is_boss;

	private GameObject Bullet;
    private bool dead, canFire;
	private float Cooltime, FireSpeed;

	private AudioSource hit;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		hit = GameObject.Find("Hit").GetComponent<AudioSource>();
	}

	void Start()
	{
		Damage += EnemyManager.GetInstance.Damage;
		money += EnemyManager.GetInstance.Money;
		HP += EnemyManager.GetInstance.HP;
		dead = false;
        if (can_shoot)
        {
            Bullet = Resources.Load("Prefabs/Enemy/EnemyBullet") as GameObject;
			Cooltime = FireSpeed = 1.5f;
			canFire = true;
        }
    }

    void Update()
	{
		if (!dead)
		{
			if (HP <= 0)
				Dead();
			if (PlayerInfo.Getinstance.is_paused() == 0)
			{
				transform.GetComponent<CircleCollider2D>().enabled = true;
				target = PlayerInfo.Getinstance.playerPos();
				angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
				x = Mathf.Cos(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;
				y = Mathf.Sin(angle * Mathf.Deg2Rad) * Speed * Time.deltaTime;
				if (!can_shoot)
				{
					if (90 < angle || angle < -90)
						spriteRenderer.flipX = true;
					else
						spriteRenderer.flipX = false;
				}

				// ** 원거리 공격 몬스터가 플레이어로부터 일정거리 유지
				if (can_shoot && Vector3.Distance(target, transform.position) < 4.5f)
				{
					transform.position += new Vector3(-x, -y, 0.0f) * 0.5f;
					if (90 < angle || angle < -90)
						spriteRenderer.flipX = false;
					else
						spriteRenderer.flipX = true;
				}
				else if (can_shoot && Vector3.Distance(target, transform.position) > 5.0f)
				{
					transform.position += new Vector3(x, y, 0.0f);
					if (90 < angle || angle < -90)
						spriteRenderer.flipX = true;
					else
						spriteRenderer.flipX = false;
				}
				else
				{
					if (90 < angle || angle < -90)
						spriteRenderer.flipX = true;
					else
						spriteRenderer.flipX = false;
				}


				if (!can_shoot)
					transform.position += new Vector3(x, y, 0.0f);

				if (can_shoot)
				{
					if (!canFire)
					{
						if (Cooltime > 0)
							Cooltime -= Time.deltaTime;
						else
						{
							if (FireSpeed < 0.1f)
								Cooltime = 0.1f;
							else Cooltime = FireSpeed;
							canFire = true;
						}
					}
					else
						shoot();
				}

			}
			else
				transform.GetComponent<CircleCollider2D>().enabled = false;
		}
		else if (dead)
		{
			if (transform.GetComponent<BossPattern>() != null)
				transform.GetComponent<BossPattern>().Dead();
			transform.GetComponent<CircleCollider2D>().enabled = false;
		}
	}

	public void onHit()
    {
		if (!dead)
		{
			animator.SetTrigger("Hit");
			hit.Play();
		}
    }

	private void Dead()
    {
		if (item_drop == true)
        {
			if (drop_item != null)
			{
				GameObject drop = Instantiate(drop_item);
				drop.transform.position = transform.position;
				drop.name = drop_item.name;
			}
        }
		if (is_boss)
		{
			SaveController.Getinstance.AchiveList[3] = 1;
			SaveController.Getinstance.AchiveUp();
		}
		dead = true;
		animator.SetBool("Dead", true);
		PlayerInfo.Getinstance.Monster_kill(exp, money);
		SaveController.Getinstance.KILL += 1;
		Destroy(gameObject, 2);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerContoller ply = collision.GetComponent<PlayerContoller>();
            ply.OnHit(Damage);
        }
    }

	private void shoot()
    {
        canFire = false;
        GameObject Obj = Instantiate(Bullet);
        EnemyBulletController controller = Obj.GetComponent<EnemyBulletController>();
		Vector3 bullet = transform.position;
		target = PlayerInfo.Getinstance.playerPos();
        angle = Mathf.Atan2(target.y - bullet.y, target.x - bullet.x) * Mathf.Rad2Deg;
        Obj.transform.position = transform.position;
        controller.angle = angle;
    }

	public void setdead(bool i) { dead = i; }
	public bool getdead() { return dead; }

}
