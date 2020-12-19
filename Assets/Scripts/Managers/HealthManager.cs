using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour 
{
    public UnityEvent zeroHealthEvent;
    public Slider playerHealthBar;
    public GameObject playerHitPanel;

    private int currentHealth;
    private bool isCoroutineStarted = false;
    private Coroutine damageEffect;

	void Start () {
        this.ResetHealthToStarting();
	}

    // Reset health to original starting health
    public void ResetHealthToStarting() {
        currentHealth = GlobalOptions.maxTemp;
        playerHealthBar.maxValue = currentHealth;
        playerHealthBar.value = currentHealth;
    }

    // Reduce the health of the object by a certain amount
    // If health lte zero, destroy the object
    public void ApplyDamage(int damage) {
        currentHealth -= damage;
        playerHealthBar.value -= damage;
        if (!isCoroutineStarted) {
            damageEffect = StartCoroutine(DamageEffect()); 
        }

        if (currentHealth <= 0){
            StopCoroutine(damageEffect);
            this.zeroHealthEvent.Invoke();
            isCoroutineStarted = false;
        }
    }

    IEnumerator DamageEffect() {
        isCoroutineStarted = true;
        playerHitPanel.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        playerHitPanel.SetActive(false);
        isCoroutineStarted = false;
    }

    // Get the current health of the object
    public int GetHealth() {
        return this.currentHealth;
    }
}
