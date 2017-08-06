using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField]
    private Image image;
    [SerializeField]
    private float maxTime;
    [SerializeField]
    private FadeScript script;

    private AsyncOperation sceneLoader;

    // Use this for initialization
    void Start()
    {
        this.sceneLoader = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
        this.sceneLoader.allowSceneActivation = false;
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void StartClick()
    {
        if (this.sceneLoader.progress == 0.9f)
        {
            // Start Animation
            //StartCoroutine(FadeAnimation());
            this.script.FadeToScene(sceneLoader);
        }
    }

    //	private IEnumerator FadeAnimation()
    //	{
    //		this.image.color = new Color (this.image.color.r, this.image.color.g, this.image.color.b, 0f);
    //
    //		float deltaTime = 0f;
    //		while (deltaTime < maxTime) {
    //			this.image.color = new Color (this.image.color.r, this.image.color.g, this.image.color.b, deltaTime / this.maxTime);
    //
    //			deltaTime += Time.deltaTime;
    //			yield return null;
    //		}
    //
    //		this.sceneLoader.allowSceneActivation = true;
    //	}
}
