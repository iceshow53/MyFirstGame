using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{


    private void Start()
    {
    }

	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.tag == "Area")
		{
			Vector3 Ppos = PlayerInfo.Getinstance.playerPos();
			float angle = Mathf.Atan2(Ppos.y - transform.position.y + 16f, Ppos.x - transform.position.x + 16f) * Mathf.Rad2Deg;

			if (45 <= angle && angle <= 135)
				transform.position += new Vector3(0f, 64f, 0f);
			else if (135 <= angle && angle <= 225)
				transform.position += new Vector3(-64f, 0f, 0f);
			else if (225 <= angle || angle <= -45)
				transform.position += new Vector3(0f, -64f, 0f);
			else if (-45 <= angle && angle <= 45)
				transform.position += new Vector3(64f, 0f, 0f);
		}
	}

}
