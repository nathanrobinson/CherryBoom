using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CherryBoom {
	public class EndOfLevel : MonoBehaviour {

		void OnTriggerEnter2D(Collider2D other) {
			if (other.gameObject.CompareTag("Player"))
			{
				var player = other.GetComponent<Player>();
				player.StopInput();
			}			
		}
	}
}