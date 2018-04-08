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

		private bool hidden;

		public GameController gameController;

		private Collider2D cherryCollider;
		private Renderer cherryRenderer;

		// Use this for initialization
		void Start () {
			cherryCollider = GetComponent<Collider2D>();
			cherryRenderer = GetComponent<Renderer>();
		}
		
		// Update is called once per frame
		void Update () {
			if (!hidden &&	gameController!= null)
			{
				var playerCollider = gameController.playerCollider;
				if(playerCollider != null && playerCollider.bounds.Intersects(cherryCollider.bounds)) {
					gameController.AddScore(this);
					gameController.AddCherries(this);
					hidden = true;
					cherryRenderer.enabled = false;
				}
			}
			
		}
	}
}