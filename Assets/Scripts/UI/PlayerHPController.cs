using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPController : MonoBehaviour
{
	private Slider HP;
	private float regen;

	private void Awake()
	{
		HP = GameObject.Find("Playerhp").GetComponent<Slider>();
		regen = 1.0f;
	}

    private void Start()
    {
		HP.maxValue = PlayerInfo.Getinstance.getHP();
		HP.value = HP.maxValue;
		StartCoroutine(HealthRegen());
    }

    void Update()
	{
		HP.value = PlayerInfo.Getinstance.getHP();
		if (PlayerInfo.Getinstance.getHP() <= 0)
			Destroy(gameObject);
	}

	IEnumerator HealthRegen()
	{
		while (true)
		{
			while (regen > 0)
			{
				while (PlayerInfo.Getinstance.is_paused() > 0)
					yield return null;
				regen -= Time.deltaTime;
				yield return null;
			}
			if (PlayerInfo.Getinstance.getHP() < PlayerInfo.Getinstance.getMHP())
			{
				PlayerInfo.Getinstance.addHP(PlayerInfo.Getinstance.getMHP() / 100);
				if (PlayerInfo.Getinstance.getHP() > PlayerInfo.Getinstance.getMHP())
					PlayerInfo.Getinstance.setHP(PlayerInfo.Getinstance.getMHP());
			}
			regen = 1.0f;
			yield return null;
		}
	}
}
