using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineDamage : MonoBehaviour
{
	public int Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.tag == "Enemy")
		{
			//collision.transform.gameObject
			EnemyController enemyController = collision.transform.GetComponent<EnemyController>();
			StartCoroutine(push(collision.transform.gameObject));
			if (enemyController.HP != 0)
			{
				enemyController.HP -= Damage;
				enemyController.onHit();
			}
		}
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
		if(collision.transform.tag == "Enemy")
        {
			//collision.transform.gameObject
			EnemyController enemyController = collision.transform.GetComponent<EnemyController>();
			if (enemyController.HP != 0)
			{
				enemyController.HP -= Damage;
				enemyController.onHit();
				StartCoroutine(push(collision.transform.gameObject));
			}
		}
	}

	IEnumerator push(GameObject _Obj)
    {
		EnemyController enemyController = _Obj.GetComponent<EnemyController>();
		float fTime = 0.0f;

		Vector3 objPos = _Obj.transform.position;
		Vector3 target = transform.position;
		Vector3 direction = new Vector3(target.x - objPos.x, target.y - objPos.y, 0.0f).normalized;

		while (fTime < 1.0f)
        {
			fTime += Time.deltaTime * 30.0f;

			//enemyController.angle;
			if (!enemyController.getdead())
			{
				_Obj.transform.position = Vector3.Lerp(_Obj.transform.position,
					_Obj.transform.position + (direction * -0.5f),
					fTime);
			}

			yield return null;
        }
    }
}
