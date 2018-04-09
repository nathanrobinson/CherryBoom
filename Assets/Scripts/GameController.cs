using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;      //Allows us to use SceneManager

namespace CherryBoom {
	public class GameController : MonoBehaviour {

    	public float restartGameDelay = 1.5f;        //Delay time in seconds to restart game.
		private int score = 0;
		private int lives = 3;
		private int cherries = 0;

		private Player player;
		private Text cherryCountText;
		private Text scoreText;
		private Text lifeText;



		// Use this for initialization
		void Start () {
			player = GetComponentInChildren<Player>();
			var texts = GetComponentsInChildren<Text>();
			cherryCountText = texts.FirstOrDefault(x => x.name == "CherryCount");
			scoreText = texts.FirstOrDefault(x => x.name == "Score");
			lifeText = texts.FirstOrDefault(x => x.name == "LifeCount");
			cherryCountText.text ="0";
			scoreText.text = "0";
			lifeText.text = lives.ToString("n0");
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

		public void Die()
		{
			lives -= 1;
			lifeText.text = lives.ToString("n0");

			if (lives == 0){				
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Invoke ("Restart", restartGameDelay);
			}
		}

		//Restart reloads the scene when called.
        private void Restart ()
        {
            //Load the last scene loaded, in this case Main, the only scene in the game.
            SceneManager.LoadScene (0);
        }
	}
}