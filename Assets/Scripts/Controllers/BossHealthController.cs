using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthController : MonoBehaviour
{
    public Slider healthBar;
	public ParticleSystem effect;
    private float health;
    private float maxHealth;

    void Start() {
        maxHealth = 1f;
        health = maxHealth;
        healthBar.value = CalculateHealth();
    }

    void Update() {
        healthBar.value = CalculateHealth();

        if (health <= 0) {
            ShooterManager shooterManager = GameObject.Find("ShooterManager").GetComponent<ShooterManager>();
            shooterManager.GetPlayer().GetComponent<ShooterController>().bossDamage = 0;
			// raise stage
			StageController _stage = GameObject.Find("Stage").GetComponent<StageController>();
			_stage.StagePassed();

			Vector3 _pos = transform.position - Vector3.up * 4.5f;
			Quaternion _rot = Quaternion.Euler(90f, 0, 0);
			ParticleSystem _particle =  Instantiate(effect, _pos , _rot) as ParticleSystem;
			ParticleSystem _particle2 = Instantiate(effect, _pos + Vector3.right*2f, _rot) as ParticleSystem;
			ParticleSystem _particle3 = Instantiate(effect, _pos + Vector3.left * 2f, _rot) as ParticleSystem;
			ParticleSystem _particle4 = Instantiate(effect, _pos + Vector3.forward * 2f, _rot) as ParticleSystem;
			ParticleSystem _particle5 = Instantiate(effect, _pos + Vector3.back * 2f, _rot) as ParticleSystem;
			_particle.transform.SetParent(_stage.transform);
			_particle2.transform.SetParent(_stage.transform);
			_particle3.transform.SetParent(_stage.transform);
			_particle4.transform.SetParent(_stage.transform);
			_particle5.transform.SetParent(_stage.transform);


			SoundManager.ins.PlayBossDeath();			
			SoundManager.ins.PlayBGM();
            Destroy(gameObject);
        }

        if (health > maxHealth) health = maxHealth;
    }

    float CalculateHealth() {
        return health / maxHealth;
    }

	public void TakeDamage()
	{		
		health -= 1f / 100f;
	}
}
