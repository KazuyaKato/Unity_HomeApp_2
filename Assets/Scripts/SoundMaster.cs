using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMaster : MonoBehaviour {

	public AudioSource BGMSource;
	public AudioSource SECrctSnd;
	public AudioSource SEClr;
	public AudioSource SEWrng;
	public AudioSource SEStgSnd;
    public AudioClip SEpaperFlip;
    //    public AudioSource SEPaperFlip; // 紙をめくる音
    AudioSource audioSource;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
	}

	// Use this for initialization
	void Start () {
        // Componentを取得
        audioSource = GetComponent<AudioSource>();
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
    public void PlaySEPaperFlip() // 紙をめくる音
    {
        audioSource.PlayOneShot(SEpaperFlip);
//        SEPaperFlip.Play();
    }
}
