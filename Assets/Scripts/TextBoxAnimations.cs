using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TextBoxAnimations : MonoBehaviour
{
    [SerializeField]
    private float m_MaxTime = 0.75f;

    private RectTransform m_RectTransform;

    // Use this for initialization
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();

        var managerUI = FindObjectOfType<ManagerUI>();

        managerUI.onEnableTextBox.AddListener(OnEnableTextBox);
        managerUI.onEnableDialogueOptions.AddListener(OnEnableDialogueOptions);
    }

    private void OnEnableTextBox()
    {
        StartCoroutine(AppearanceAnimation());
    }

    private void OnEnableDialogueOptions()
    {

    }

    private IEnumerator AppearanceAnimation()
    {
        var oldPosition = transform.position;

        var offScreenPoint =
            Camera.main.ViewportToScreenPoint(new Vector3(0f, -m_RectTransform.anchorMax.y));

        var newPosition =
            new Vector3(transform.position.x, offScreenPoint.y);

        var deltaTime = 0f;
        while (deltaTime < m_MaxTime)
        {
            transform.position = Vector3.Lerp(newPosition, oldPosition, deltaTime / m_MaxTime);

            deltaTime += Time.deltaTime;
            yield return null;
        }
    }
}
