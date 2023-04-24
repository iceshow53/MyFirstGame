using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerContoller : MonoBehaviour
{
	private float Speed;

	private Vector3 movement;

	private Animator animator;
	private SpriteRenderer spriteRenderer;
	private GameObject uiobject, Stats, Sub, Subs2, button1, button2;
	private UIController uIController;

	private AudioSource Lose;

	public GameObject Weapons;

	private int M_HP, C_HP;

	private float Hor, Ver, hit_cool;
	private bool died, hit;

	// ** ���콺 �������� �߻��� ���� ���
	private float angle;
	Vector2 target, mouse;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
		Stats = GameObject.Find("PlayerStats");
		Sub = GameObject.Find("SubWeaponSystem");
		Subs2 = GameObject.Find("SubWeapon");
		Lose = GameObject.Find("Lose").GetComponent<AudioSource>();
		button1 = GameObject.Find("StatShop");
		button2 = GameObject.Find("SubWeaponShop");
	}

	void Start()
	{
		StartCoroutine(GetCompo());

		died = hit = false;
		hit_cool = 1f;

		// ** �ϴ� ��Ȱ��ȭ�� UI�� ����� ���Ƴֱ�
		if(Stats != null)
			Stats.SetActive(false);
		if(Sub != null)
			Sub.SetActive(false);
		if (button1 != null)
			button1.SetActive(false);		
		if (button2 != null)
			button2.SetActive(false);
	}

	void Update()
	{
		Speed = PlayerInfo.Getinstance.getSpeed();

		Hor = Input.GetAxisRaw("Horizontal");
		Ver = Input.GetAxisRaw("Vertical");
		if(!died)
		{
			if (PlayerInfo.Getinstance.is_paused() == 0)
			{
				if (Hor < 0)
				{
					spriteRenderer.flipX = true;
				}
				else if (Hor > 0)
				{
					spriteRenderer.flipX = false;
				}

				if (Input.GetMouseButton(0))
				{
					Attack();
				}

				if (Hor != 0 || Ver != 0)
					animator.SetBool("Move", true);
				else
					animator.SetBool("Move", false);


				transform.position += new Vector3(Hor * Time.deltaTime * Speed, Ver * Time.deltaTime * Speed, 0.0f);
				Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5.0f);
			}

			// ** ���� ����
			if(Input.GetKeyDown(KeyCode.B))
			{ 
				if (!Sub.activeSelf)
				{
					Sub.SetActive(true);
					Subs2.GetComponent<SubWeaponContoller>().ScreenOpen();
					PlayerInfo.Getinstance.set_pause(1);
					PlayerInfo.Getinstance.setOpened(true);
				}
				else
				{
					Sub.SetActive(false);
					PlayerInfo.Getinstance.set_pause(-1);
					PlayerInfo.Getinstance.setOpened(false);
				}
			}
			// ** �÷��̾� ���� ����
			if(Input.GetKeyDown(KeyCode.I))
            {
				if (!Stats.activeSelf)
				{
					Stats.SetActive(true);
					Stats.GetComponent<PlayerInfoUI1>().PRESSED();
					PlayerInfo.Getinstance.set_pause(1);
					PlayerInfo.Getinstance.setOpened(true);
				}
				else
				{
					Stats.SetActive(false);
					PlayerInfo.Getinstance.set_pause(-1);
					PlayerInfo.Getinstance.setOpened(false);
				}
			}
		}
		else
		{

		}
	}

	private void Attack()
	{
		// ** ����ĭ�� ���Ⱑ ������ �߻�
		if (Weapons.transform.Find("Weapon0") != null)
		{
			Weapons.transform.Find("Weapon0").GetComponent<WeaponSystem>().OnFire();
			if (uIController == null)
				StartCoroutine(GetCompo());
			else
				uIController.Ammo1(Weapons.transform.Find("Weapon0").GetComponent<WeaponSystem>().getAmmo(), Weapons.transform.Find("Weapon0").GetComponent<WeaponSystem>().currentAmmo());
		}
		if (Weapons.transform.Find("Weapon1") != null)
		{
			Weapons.transform.Find("Weapon1").GetComponent<WeaponSystem>().OnFire();
			if (uIController == null)
				StartCoroutine(GetCompo());
			else
				uIController.Ammo2(Weapons.transform.Find("Weapon1").GetComponent<WeaponSystem>().getAmmo(), Weapons.transform.Find("Weapon1").GetComponent<WeaponSystem>().currentAmmo());
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// ** ���� ȹ��
		if(collision.tag == "Entity")
        {
			if (collision.name == "Box")
				collision.GetComponent<BoxContoller>().OpenBox();
			if(collision.name == "key")
				collision.GetComponent<Key>().GetKey();
        }
		if (collision.name == "Lock")
			collision.GetComponent<Door>().interaction();
	}

	public void OnHit(int D)
	{
		// ** �¾��� �� ��ȣ�ۿ�
		if (!hit)
		{
			// �¾��� �� �����ð� ����
			PlayerInfo.Getinstance.addHP(-D);
			animator.SetTrigger("Hit");
			hit = true;

			if (PlayerInfo.Getinstance.getHP() <= 0)
			{
				PlayerInfo.Getinstance.set_pause(1);
				if (!died)
				{
					SaveController.Getinstance.DIE += 1;
					if (SaveController.Getinstance.DIE >= 2)
						SaveController.Getinstance.AchiveList[8] = 1;
					died = true;
				}
				Lose.Play();
				SaveController.Getinstance.onSave();
				animator.SetBool("Dead", true);
				uIController.DEAD = true;
				if (Weapons.transform.Find("Weapon0") != null)
					Destroy(Weapons.transform.Find("Weapon0").gameObject);
				if (Weapons.transform.Find("Weapon1") != null)
					Destroy(Weapons.transform.Find("Weapon1").gameObject);
			}

			StartCoroutine(HitCoolDown());
		}
	}

	IEnumerator GetCompo()
    {
		while(uIController == null)
        {
			uIController = GameObject.Find("UI").GetComponent<UIController>();

			yield return null;
        }
    }

	IEnumerator HitCoolDown()
    {
		while(hit_cool > 0)
        {
			hit_cool -= Time.deltaTime;
			yield return null;
        }
		hit_cool = 1.0f;
		hit = false;
    }
}
