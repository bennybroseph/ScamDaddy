using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scene2 : MonoBehaviour {
	[SerializeField]
	private Image image;
	[SerializeField]
	private float maxTime;


	// Use this for initialization
	void Start () {
		//StartCoroutine (FadeAnimation ());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator FadeAnimation() {
		this.image.color = new Color (this.image.color.r, this.image.color.g, this.image.color.b, 1);

		float deltaTime = 0.0f;
		while (deltaTime < maxTime) {
			this.image.color = new Color (this.image.color.r, this.image.color.g, this.image.color.b, 1 - deltaTime/this.maxTime);

			deltaTime += Time.deltaTime;
			yield return null;
		}
	}
}
