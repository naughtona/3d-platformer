using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	#region singleton
	public static SoundManager ins;
	public bool bossSoundOn = false;
	private void Awake()
	{
		ins = this;
	}
	#endregion

	public AudioClip audioBGMClip;
	private void Start()
	{
		audioBGM.clip = audioBGMClip;

		audioBGM.loop = true;
		PlayBGM();

		// Boss
		audioBoss.loop = true;
	}

	#region BGM Group
	public AudioSource audioBGM;
	public AudioSource audioBoss;
	
	
	// Update is called once per frame
	public void PlayBGM()
    {
		// Remove all sound before
		audioBoss.Stop();
		bossSoundOn = false;
		// BGM
		audioBGM.Stop();
		audioBGM.Play();
    }

	public void PlayBoss()
	{
		// Remove all sound before
		audioBGM.Stop();
		bossSoundOn = true;
		// Boss music enters
		audioBoss.Stop();
		audioBoss.Play();
	}
	#endregion

	#region PlayCoin....
	public List<AudioSource> audioCoin = new List<AudioSource>();
	int index;
	public void PlayCoin()
	{
		if (audioCoin.Count <= 0) return;

		audioCoin[index].Play();
		index = (index + 1) % audioCoin.Count;
	}

	#endregion

	#region PlayDamaged....
	public List<AudioSource> audioDamaged = new List<AudioSource>();
	int indexDamaged;
	public void PlayDamaged()
	{
		if (audioDamaged.Count <= 0) return;

		audioDamaged[indexDamaged].Play();
		indexDamaged = (indexDamaged + 1) % audioDamaged.Count;
	}

	#endregion


	//SoundManager.ins.PlayTeleport();
	#region PlayTeleport....
	public AudioSource audioTeleport;
	public void PlayTeleport()
	{
		if (audioTeleport == null) return;

		audioTeleport.Play();
	}

	#endregion

	#region PlayBossDeath....
	public AudioSource audioBossDeath;
	public void PlayBossDeath()
	{
		if (audioBossDeath == null) return;

		audioBossDeath.Play();
	}

	#endregion

	#region PlayShot....
	public List<AudioSource> audioShot = new List<AudioSource>();
	int indexShot = 0;
	public void PlayShot()
	{
		if(audioShot.Count <= 0)return;

		audioShot[indexShot].Play();
		indexShot = (indexShot + 1) % audioShot.Count;
	}

	#endregion

	
}
