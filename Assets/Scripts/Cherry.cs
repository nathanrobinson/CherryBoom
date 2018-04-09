using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CherryBoom {
	public class Cherry : MonoBehaviour, ICherry {

		public int points { get; set; }
		public int cherries { get; set; }

		public Cherry() {
			points = 100;
			cherries = 1;
		}

		public GameController gameController;
		
		void OnTriggerEnter2D(Collider2D other) {
			if (gameController!= null &&
				other.gameObject.CompareTag("Player"))
			{
				gameController.AddScore(this);
				gameController.AddCherries(this);
				Destroy(gameObject);
			}
			
		}
	}
}