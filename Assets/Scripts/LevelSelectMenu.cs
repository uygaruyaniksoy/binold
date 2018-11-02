using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour {
	public GameObject _right;
	public GameObject _left;

	public void CreateUI() {
		Game game = GameObject.Find("Game").GetComponent<Game>();
		int page = game.page;
		for (int i = 0; i < 10; i++) {
			gameObject.transform
				.GetChild(i + 1)
				.GetChild(0)
				.GetComponent<TextMeshProUGUI>().text = "" + (page * 10 + i + 1);
			gameObject.transform
				.GetChild(i + 1)
				.GetChild(1)
				.gameObject.SetActive((game.LevelStatus[page] & (1 << i)) != 0);
		}
		if (page == 0) _left.SetActive(false);
		else _left.SetActive(true);
		
		if (page == game.pageLimit - 1) _right.SetActive(false);
		else _right.SetActive(true);
	}

	public void OnLeft() {
		Game game = GameObject.Find("Game").GetComponent<Game>();
		game.page--;
		CreateUI();
	}
	
	public void OnRight() {
		Game game = GameObject.Find("Game").GetComponent<Game>();
		game.page++;
		CreateUI();
	}
}
