using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
	public int ammo;
	private int ammo_c;
	public int damage;
	public float fire_speed;
	private float cooltime;
	public float reload_speed;
	public float reload_time;
	public int Penetrate;
	private int add_Damage;

	private AudioSource Shoot;

	public bool is_shotgun;

	public bool CanFire;

	public GameObject Bullet;
	private float angle;
	Vector2 target, mouse;

    private void Awake()
    {
		Shoot = GameObject.Find("Shoot").GetComponent<AudioSource>();
		ammo += (PlayerInfo.Getinstance.PlayerStats[2] + SaveController.Getinstance.AchiveList[6]) * 10;
		add_Damage = 0;
    }

    void Start()
	{
		CanFire = true;
		cooltime = PlayerInfo.Getinstance.getAttackSpeed() - fire_speed;
		reload_time = PlayerInfo.Getinstance.getReloadSpeed() - reload_speed;
		ammo_c = ammo;
	}

	void Update()
	{
		if (!CanFire)
		{
			if (cooltime > 0)
				cooltime -= Time.deltaTime;
			else
            {
				if (PlayerInfo.Getinstance.getAttackSpeed() <= 0.1f)
				{
					cooltime = 0.1f - fire_speed;
					if (cooltime <= 0f)
						cooltime = 0.05f;
				}
				else cooltime = PlayerInfo.Getinstance.getAttackSpeed() - fire_speed;
				CanFire = true;
			}
		}
		if(ammo == 0)
        {
			if (reload_time > 0)
				reload_time -= Time.deltaTime;
			else
            {
				if (PlayerInfo.Getinstance.getReloadSpeed() <= 0.1f)
				{
					reload_time = 0.1f - reload_speed;
				}
				else reload_time = PlayerInfo.Getinstance.getReloadSpeed() - reload_speed;
				ammo = ammo_c;
            }
        }
	}

	public void OnFire()
	{
		if (!CanFire)
			return;
		if (ammo <= 0)
			return;

		ammo -= 1;
		CanFire = false;
		target = transform.position;
		mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
		if (is_shotgun)
		{
			int addbullet = 3;
			addbullet += PlayerInfo.Getinstance.PlayerStats[7] + SaveController.Getinstance.AchiveList[7];
			angle -= addbullet * 0.5f;
			for (int i = 0; i < addbullet; ++i)
			{
				GameObject Obj = Instantiate(Bullet);
				BulletController controller = Obj.AddComponent<BulletController>();
				Obj.transform.position = transform.position;
				controller.angle = angle;
				controller.Penetrate = Penetrate + PlayerInfo.Getinstance.getPenetrate();
				controller.Damage = damage + add_Damage + PlayerInfo.Getinstance.getDamage();
				angle += 1f;
			}
		}
		else
		{
			int addbullet = 1;
			addbullet += PlayerInfo.Getinstance.PlayerStats[7] + SaveController.Getinstance.AchiveList[7];
			angle -= addbullet * 0.5f;
			for (int i = 0; i < addbullet; ++i)
			{
				GameObject Obj = Instantiate(Bullet);
				BulletController controller = Obj.AddComponent<BulletController>();
				Obj.transform.position = transform.position;
				controller.angle = angle;
				controller.Penetrate = Penetrate + PlayerInfo.Getinstance.getPenetrate();
				controller.Damage = damage + add_Damage + PlayerInfo.Getinstance.getDamage();
				angle += 1f;
			}
		}
		Shoot.Play();
	}

	public int getAmmo()
    {
		return ammo;
    }

	public int currentAmmo()
    {
		return ammo_c;
    }

	public void AddDamage(int i) { add_Damage += i; }
}
