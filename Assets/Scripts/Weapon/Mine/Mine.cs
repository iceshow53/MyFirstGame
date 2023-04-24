using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
	public float Range;
	public int Damage;
	private bool is_triggered;
	private GameObject explosion;
	private CircleCollider2D trigger, explosion_trigger;
	private SpriteRenderer spriteRenderer;

	private void Awake()
	{
		explosion = Resources.Load("Prefabs/Fx/Explosion") as GameObject;
		is_triggered = false;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!is_triggered)
		{
			if (collision.tag == "Enemy")
			{
				GameObject explosions = Instantiate(explosion);
				MineDamage minedamage = explosions.GetComponent<MineDamage>();
				trigger = explosions.GetComponent<CircleCollider2D>();
				spriteRenderer = transform.GetComponent<SpriteRenderer>();
				minedamage.Damage = Damage;
				spriteRenderer.enabled = false;
				StartCoroutine(disable(0.1f));
				explosions.transform.localScale *= Range;
				explosions.transform.position = transform.position;
				is_triggered = true;
			}
		}
	}

	IEnumerator disable(float i)
    {
		while(i > 0)
        {
			i -= Time.deltaTime;
			yield return null;
        }
		trigger.enabled = false;
    }
}
