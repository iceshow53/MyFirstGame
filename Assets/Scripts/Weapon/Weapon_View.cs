using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_View : MonoBehaviour
{
	private float angle;
	private SpriteRenderer spriteRenderer;
	
	Vector2 target, mouse;

	private void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	private void Update()
	{
		// ** 플레이어의 마우스가 있는 방향으로 바라보게 하는거
		if (PlayerInfo.Getinstance.is_paused() == 0)
		{
			target = transform.position;
			mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			angle = Mathf.Atan2(mouse.y - target.y, mouse.x - target.x) * Mathf.Rad2Deg;
			if (90 < angle || angle < -90)
				spriteRenderer.flipY = true;
			else
				spriteRenderer.flipY = false;
			this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		}
	}
}
