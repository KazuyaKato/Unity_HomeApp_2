using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JukeBox : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string Music_load(int _no){

		switch (_no) {
		case 0:
			return "Songs/00_Birthplace";

		case 1:
			return "Songs/01_The Nutcracker";
		
		case 2:
			return "Songs/02_Swan";
		
		case 3:
			return "Songs/03_Red Dragonfly";
		
		case 4:
			return "Songs/04_Pictures at a Exhibition";
		
		case 5:
			return "Songs/05_From the New World";
		
		case 6:
			return "Songs/06_Jupiter";
		
		case 7:
			return "Songs/07_Song of the Beach";
		
		case 8:
			return "Songs/08_Moon over the Ruined Castle";
		
		case 9:
			return "Songs/09_Stood Me Up";
		
		case 10:
			return "Songs/10_Soap Bubble";
		
		case 11:
			return "Songs/11_Trout";
		
		case 12:
			return "Songs/12_Tannhauser";
		
		case 13:
			return "Songs/13_Sea of Spring";
		
		case 14:
			return "Songs/14_Light Cavalry";
		
		case 15:
			return "Songs/15_Bolero";
		
		case 16:
			return "Songs/16_PROKOFIEV-PeterAndTheWolf";
		
		case 17:
			return "Songs/17_179-Mozart-Magic-Flute-Overture";
		
		case 18:
			return "Songs/18_Rokudan";
		
		case 21:
			return "Songs/21_Rising-sun Flag";
			
		case 23:
			return "Songs/23_Spring has Come";
			
		case 25:
			return "Songs/25_Spring Stream";
			
		case 26:
			return "Songs/26_Fuji";
			
		case 27:
			return "Songs/27_PastureMorning";
			
		case 28:
			return "Songs/28_Maple";
			
		case 30:
			return "Songs/30_Etenraku Imayo";
			
		case 31:
			return "Songs/31_Moonlit Night";
			
		case 32:
			return "Songs/32_American Patrol";
			
		case 33:
			return "Songs/33_Toy Soldier";
			
		case 35:
			return "Songs/35_Damcomg Doll";
			
		case 36:
			return "Songs/36_Feuerfest";
			
		case 37:
			return "Songs/37_Cuckoo Waltz";
			
		case 38:
			return "Songs/38_S.Prokofev._Syuita_Zimnij_kostyor_soch.122_-_Otezd_(xMuzic.me)";
			
		case 39:
			return "Songs/39_Beethoven-Turkish-March";
			
		case 40:
			return "Songs/40_Menuet from Alcina";
			
		case 41:
			return "Songs/41_Dvorak-Humoresque";
			
		case 42:
			return "Songs/42_Golden Wedding Anniversary";
			
		case 43:
			return "Songs/43_Gold and Silver";
			
		case 44:
			return "Songs/44_Bizet-LArlesienne-Menuett";
			
		case 45:
			return "Songs/45_Beethoven-Menuett-inG";
			
		case 46:
			return "Songs/46_Bach-SuiteNo2-Bdenerie";
			
		case 47:
			return "Songs/47_Schubert-March-Military-No1";
			
		case 48:
			return "Songs/48_219-Waldteufel-The-Skaters-Waltz";
			
		case 49:
			return "Songs/49_Norway Dance Music";
			
		case 50:
			return "Songs/50_Rossini-Guillaume-Tell-Overture";
			
		case 52:
			return "Songs/52_Hakone Hachiri";
			
		case 53:
			return "Songs/53_Rentaro-Taki-Four-Seasons-Hana";
			
		case 54:
			return "Songs/54_This Road";
			
		case 56:
			return "Songs/56_Grieg-PeerGynt-Morning";
			
		case 57:
			return "Songs/57_Nomadic Tribe";
		default:
			return "";
		}
	}
}
