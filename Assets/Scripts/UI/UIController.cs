using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
	private float EXP, time;
	private Text money, Killed, timer, level;
	public GameObject buttons;
	private Animator clear, dead;
	private Slider exp, ammo1, ammo2;
	private int AMMO1, AMMO2;
	public bool DEAD;
	private bool setpause;
	private bool is_clear;


	private AudioSource Win;
	private AudioSource bgm;

	private GameObject Pause, Stats, subweapon, button1, button2;

	private void Awake()
	{
		money = GameObject.Find("Money").GetComponent<Text>();
		Killed = GameObject.Find("Kill").GetComponent<Text>();
		timer = GameObject.Find("Timer").GetComponent<Text>();
		level = GameObject.Find("Level").GetComponent<Text>();
		exp = GameObject.Find("EXP").GetComponent<Slider>();
		ammo1 = GameObject.Find("Ammo1").GetComponent<Slider>();
		ammo2 = GameObject.Find("Ammo2").GetComponent<Slider>();
		clear = GameObject.Find("GAMEOVER").GetComponent<Animator>();
		dead = GameObject.Find("DEAD").GetComponent<Animator>();
		Pause = GameObject.Find("Pause");
		Win = GameObject.Find("Win").GetComponent<AudioSource>();
		bgm = GameObject.Find("BGM").GetComponent<AudioSource>();
		DEAD = is_clear = false;
		setpause = false;
		Stats = GameObject.Find("PlayerStats");

		subweapon = GameObject.Find("SubWeaponSystem");
		button1 = GameObject.Find("StatShop");
		button2 = GameObject.Find("SubWeaponShop");
		time = 300f;
	}
	
	void Start()
	{
		level.text = "LEVEL:" + PlayerInfo.Getinstance.getLevel();
		Pause.SetActive(false);
	}

	void Update()
	{
		if (!DEAD)
		{
			if (time > 0.0f)
			{
				if (PlayerInfo.Getinstance.is_paused() == 0)
					time -= Time.deltaTime;
			}
			else
			{
				time = 0;
				if (!is_clear)
				{
					SaveController.Getinstance.CLEAR += 1;
					if(SaveController.Getinstance.CLEAR >= 5)
					{
						SaveController.Getinstance.AchiveList[7] = 1;
					}
					SaveController.Getinstance.StatAdd(5);
					SaveController.Getinstance.onSave();
					is_clear = true;
				}
				if (PlayerInfo.Getinstance.getKilled() > SaveController.Getinstance.MOSTKILL)
					SaveController.Getinstance.MOSTKILL = PlayerInfo.Getinstance.getKilled();
				bgm.Stop();
				Win.Play();
				clear.SetTrigger("Clear");
				PlayerInfo.Getinstance.set_pause(1);
			}
		}
		else
		{
			if (PlayerInfo.Getinstance.getKilled() > SaveController.Getinstance.MOSTKILL)
				SaveController.Getinstance.MOSTKILL = PlayerInfo.Getinstance.getKilled();
			SaveController.Getinstance.onSave();
			bgm.Stop();
			dead.SetTrigger("Clear");
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (PlayerInfo.Getinstance.getOpened())
			{
				subweapon.SetActive(false);
				Stats.SetActive(false);
				PlayerInfo.Getinstance.set_pause(-1);
				PlayerInfo.Getinstance.setOpened(false);
			}
			else
			{
				setpause = !setpause;
				if (setpause)
				{
					PlayerInfo.Getinstance.set_pause(1);
					bgm.Pause();
				}
				else
				{
					PlayerInfo.Getinstance.set_pause(-1);
					bgm.Play();
				}
				SaveController.Getinstance.onSave();

				Pause.SetActive(setpause);
				buttons.gameObject.SetActive(setpause);
			}
		}
		if (time > 10.0f)
			timer.text = (((int)time / 60 % 60)).ToString() + ":" + ((int)time % 60).ToString();
		else if (time < 10.0f)
			timer.text = "00:0" + ((int)time % 60).ToString();
		setText();
	}

	public void setText()
	{
		level.text = "LEVEL:" + PlayerInfo.Getinstance.getLevel();
		exp.value = PlayerInfo.Getinstance.getEXP();
		money.text = "MONEY:" + PlayerInfo.Getinstance.getMoney();
		Killed.text = "Kill:" + PlayerInfo.Getinstance.getKilled();
	}

	public void Ammo1(int AMMO1,int full_ammo1)
	{
		ammo1.value = AMMO1;
		ammo1.maxValue = full_ammo1;
	}

	public void Ammo2(int AMMO2, int full_ammo2)
	{
		ammo2.value = AMMO2;
		ammo2.maxValue = full_ammo2;
	}

	public float getRemain() { return time; }
}
