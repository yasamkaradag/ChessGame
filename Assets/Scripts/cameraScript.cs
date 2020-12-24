using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{

	public GameObject[,] BlokDizisi;
	public GameObject block;
	public GameObject knight;
	public GameObject king;
	public int blockSize;
	private int kareSayisi = 0;
	private int[] atKonumu = new int[2];
	private int[] atHedefKonum =new int[2];
	private int[] Kirmizi = new int[2];
	private int[] Yesil = new int[2];
	bool atYerlestirildi = false;
	public static int tempblockSize;
	public static int DistanceHolder;
	public static bool resetle = false;

	// Use this for initialization
	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		tempblockSize = blockSize;
		creator();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			modifier();
		}
		if( resetle ) {
			reset();
			resetle = false;
		}
	}

	void reset() {
		destroyer();
		blockSize = tempblockSize;
		creator();
	}

	void destroyer() {
		for(int i = 0; i < blockSize; i++) {
			for(int j = 0; j < blockSize; j++) {
				Destroy(BlokDizisi[i, j]);
			}
		}
	}

	void modifier() {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 mousePos2D = new Vector2 (mousePos.x, mousePos.y);

		RaycastHit2D hit = Physics2D.Raycast (mousePos2D, Vector2.zero);
		if (hit.collider != null) {
			for (int i = 0; i < blockSize; i++) {
				for (int j = 0; j < blockSize; j++) {
					if (BlokDizisi[i, j] == hit.collider.gameObject) {
						if (atYerlestirildi == false) {
							if ((Kirmizi [0] + Kirmizi [1]) % 2 == 0) {
								BlokDizisi[Kirmizi [0], Kirmizi [1]].GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0);
								BlokDizisi[Kirmizi [0], Kirmizi [1]].GetComponent<SpriteRenderer> ().sprite = block.GetComponent<SpriteRenderer>().sprite;
								BlokDizisi[Kirmizi [0], Kirmizi [1]].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
							} else {
								BlokDizisi[Kirmizi[0],Kirmizi[1]].GetComponent<SpriteRenderer>().color = new Color (255, 255, 255);
								BlokDizisi[Kirmizi [0], Kirmizi [1]].GetComponent<SpriteRenderer> ().sprite = block.GetComponent<SpriteRenderer>().sprite;
								BlokDizisi[Kirmizi [0], Kirmizi [1]].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
							}
							if ((Yesil [0] + Yesil [1]) % 2 == 0) {
								BlokDizisi[Yesil [0], Yesil [1]].GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0);
								BlokDizisi[Yesil [0], Yesil [1]].GetComponent<SpriteRenderer> ().sprite = block.GetComponent<SpriteRenderer>().sprite;
								BlokDizisi[Yesil [0], Yesil [1]].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
							} else {
								BlokDizisi[Yesil[0],Yesil[1]].GetComponent<SpriteRenderer> ().sprite = block.GetComponent<SpriteRenderer>().sprite;
								BlokDizisi[Yesil[0],Yesil[1]].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(1, 1, 1);
								BlokDizisi[Yesil[0],Yesil[1]].GetComponent<SpriteRenderer>().color = new Color (255, 255, 255);
							} 
							BlokDizisi[i, j].GetComponent<SpriteRenderer>().sprite  = knight.GetComponent<SpriteRenderer>().sprite;
							BlokDizisi[i, j].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.44f, 0.44f, 1);
							BlokDizisi[i, j].GetComponent<SpriteRenderer> ().color = new Color (255, 0, 0);
							atKonumu [0] = i;
							atKonumu [1] = j;
							atYerlestirildi = true;
							Kirmizi [0] = i;
							Kirmizi [1] = j;
							Debug.Log ("At yerlestirildi");
							
				
						} else {
							BlokDizisi[i, j].GetComponent<SpriteRenderer>().sprite  = king.GetComponent<SpriteRenderer>().sprite;
							BlokDizisi[i, j].GetComponent<SpriteRenderer>().transform.localScale = new Vector3(0.435f, 0.46f, 1);
							atYerlestirildi = false;
							BlokDizisi[i, j].GetComponent<SpriteRenderer> ().color = new Color (0, 255, 0);
							atHedefKonum [0] = i;
							atHedefKonum [1] = j;
							int a=Hesapla (atKonumu, atHedefKonum, blockSize);
							DistanceHolder=a;
							Debug.Log ("Hedef secildi");
							Yesil [0] = i;
							Yesil [1] = j;

							Debug.Log("distance is " + a.ToString());
							Debug.Log ("En kisa yol bulunuyor");
						}
						Debug.Log (hit.collider);

					}
				}
			}
		}
	}

	void creator() {
		BlokDizisi= new GameObject[blockSize, blockSize];
		for (int i = 0; i < blockSize; i++) {
			for (int j = 0; j < blockSize; j++) {
				GameObject temp = (GameObject)Instantiate (block, new Vector3 (i - blockSize / 2 + 0.5f, j  - blockSize / 2+ 0.5f, 0), Quaternion.identity);
				if ((i + j) % 2 == 0) {
					temp.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0);
				}
				BlokDizisi[i, j] = temp;
				kareSayisi += 1;
			}
		}
	}

	static int Hesapla(int[] atKonumu,int[] atHedef,int blockSize){

		// x and y direction, where a knight can move 
		int[] dx = { -2, -1, 1, 2, -2, -1, 1, 2 }; 
		int[] dy = { -1, -2, -2, -1, 1, 2, 2, 1 }; 

		// queue for storing states of knight in board 
		Queue<int[]> yolBloklari = new Queue<int[]>(); 

		// push starting position of knight with 0 distance 
		int[] a1 = {atKonumu [0], atKonumu [1], 0};
		yolBloklari.Enqueue(a1); 

		int[] t; 
		int x, y; 
		bool[, ] visit = new bool[blockSize + 1, blockSize + 1]; 

		// make all cell unvisited 
		for (int i = 1; i <= blockSize; i++) 
			for (int j = 1; j <=blockSize; j++) 
				visit[i, j] = false; 

		// visit starting state 
		visit[atKonumu[0], atKonumu[1]] = true; 

		// loop untill we have one element in queue 
		while (yolBloklari.Count != 0) { 
			t = yolBloklari.Peek(); 
			yolBloklari.Dequeue(); 

			// if current cell is equal to target cell, 
			// return its distance 
			if (t[0] == atHedef[0] && t[1]== atHedef[1]) 
				return t[2]; 

			// loop for all reachable states 
			for (int i = 0; i < 8; i++) { 
				x = t[0] + dx[i]; 
				y = t[1] + dy[i]; 

				// If reachable state is not yet visited and 
				// inside board, push that state into queue 
				if (isInside(x, y, blockSize) && !visit[x, y]) { 
					visit[x, y] = true; 
					int[] a2 = {x, y, t[2] + 1};
					yolBloklari.Enqueue(a2); 
				} 
			} 
		} 
		return int.MaxValue;
	}
	static bool isInside(int x,int y,int N){
	
		if (x >= 0 && x <= N && y >= 0 && y <= N) {
			Debug.Log ("disari cikti");
			return true;
		}
		return false;
	}
}