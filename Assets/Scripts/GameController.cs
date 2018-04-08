using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CherryBoom {
	public class GameController : MonoBehaviour {

		private int score = 0;
		private int lives = 3;
		private int cherries = 0;

		private Player player;
		private Text cherryCountText;
		private Text scoreText;



		// Use this for initialization
		void Start () {
			player = GetComponentInChildren<Player>();
			var texts = GetComponentsInChildren<Text>();
			cherryCountText = texts.FirstOrDefault(x => x.name == "CherryCount");
			scoreText = texts.FirstOrDefault(x => x.name == "Score");
			cherryCountText.text ="0";
			scoreText.text = "0";
		}
		
		// Update is called once per frame
		void Update () {
			
		}

    	public Collider2D playerCollider { get { return player != null ? player.playerCollider : null; }}

		public void AddCherries(ICherry cherry)
		{
			cherries += cherry.cherries;
			cherryCountText.text = cherries.ToString("n0");
		}

		public void AddScore(IScorable scorable)
		{
			score += scorable.points;
			scoreText.text = score.ToString("n0");
		}
	}
}