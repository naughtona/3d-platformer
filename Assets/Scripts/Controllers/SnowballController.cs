using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballController : MonoBehaviour
{
	public ParticleSystem effect;
	public float snowballSpeed;
    public Quaternion rotation;
    public float shooterSpeed;
	[SerializeField] float surfaceMarge = 0.2f;
	[SerializeField] LayerMask mask = 0 << 0;

	void Update() {
		float _speed = snowballSpeed + shooterSpeed;
		float _distance = _speed * Time.deltaTime + 0.5f;
		Ray _ray = new Ray(transform.position, transform.forward);
		RaycastHit _hit;
		//Debug.DrawRay(_ray.origin, _ray.direction * _distance, Color.red);
		if(Physics.Raycast(_ray, out _hit, _distance, mask))
		{
			BossHealthController _boss = _hit.transform.GetComponent<BossHealthController>();
			if (_boss)
			{
				_boss.TakeDamage();
			}
			Vector3 _dir = (transform.position - _hit.point).normalized * surfaceMarge;
			Instantiate(effect, _hit.point + _dir, Quaternion.identity);
			Destroy(gameObject);
		}


		//my forward (blue arrow ) move...
		transform.Translate(Vector3.forward * _speed * Time.deltaTime);
	}
}
