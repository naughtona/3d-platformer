using UnityEngine;
using System.Collections;

// Cartoon FX  - (c) 2015 Jean Moreno

// Automatically destructs an object when it has stopped emitting particles and when they have all disappeared from the screen.
// Check is performed every 0.5 seconds to not query the particle system's state every frame.
// (only deactivates the object if the OnlyDeactivate flag is set, automatically used with CFX Spawn System)

[RequireComponent(typeof(ParticleSystem))]
public class CFX_AutoDestructShuriken : MonoBehaviour
{
	ParticleSystem particle;
	public float duration = -1;

	void Awake()
	{
		if (particle == null)
		{
			particle = GetComponent<ParticleSystem>();
		}
	}

	void OnEnable()
	{
		CancelInvoke();
		if (duration <= 0)
		{
			//duration = particle.startLifetime;
			duration = particle.main.duration;
		}
		Invoke("PoolReturn", duration);
	}

	void OnDisalbe()
	{
		CancelInvoke();
	}

	public void PoolReturn()
	{
		//gameObject.SetActive(false);
		Destroy(gameObject);
	}
}
