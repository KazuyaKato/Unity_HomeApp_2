using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour {

	public AudioSource BGMSource;
	public AudioSource SECrctSnd;
	public AudioSource SEClr;
	public AudioSource SEWrng;
	public AudioSource SEStgSnd;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayBGM(){
		BGMSource.Play ();
	}

	public void StopBGM(){
		BGMSource.Stop ();
	}

	public void PlaySECrctSnd(){
		SECrctSnd.Play ();
	}

	public void PlaySEClr(){
		SEClr.Play ();
	}

	public void PlaySEWrng(){
		SEWrng.Play ();
	}
	public void PlaySEStgSnd(){
		SEStgSnd.Play ();
	}
}
