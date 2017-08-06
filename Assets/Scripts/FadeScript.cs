using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeScript : MonoBehaviour
{

    [SerializeField]
    private Image image;
    [SerializeField]
    private float maxTime;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void FadeToScene(AsyncOperation loadScene)
    {
        StartCoroutine(Fade(loadScene));
    }

    private IEnumerator Fade(AsyncOperation loadScene)
    {
        yield return FadeOut();

        loadScene.allowSceneActivation = true;

        yield return FadeIn();
    }

    public IEnumerator FadeOut()
    {
        this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, 0f);

        float deltaTime = 0f;
        while (deltaTime < maxTime)
        {
            this.image.color =
                new Color(
                    this.image.color.r,
                    this.image.color.g,
                    this.image.color.b,
                    deltaTime / this.maxTime);

            deltaTime += Time.deltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeIn()
    {
        this.image.color = new Color(this.image.color.r, this.image.color.g, this.image.color.b, 1);

        var deltaTime = 0.0f;
        while (deltaTime < maxTime)
        {
            this.image.color =
                new Color(
                    this.image.color.r,
                    this.image.color.g,
                    this.image.color.b,
                    1 - deltaTime / this.maxTime);

            deltaTime += Time.deltaTime;
            yield return null;
        }
    }
}
