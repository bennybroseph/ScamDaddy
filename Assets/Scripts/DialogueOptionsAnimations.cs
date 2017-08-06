using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionsAnimations : MonoBehaviour
{
    [SerializeField]
    private AnimationCurve m_AnimationCurve;
    [SerializeField]
    private float m_MaxTime = 0.5f;

    private Button[] m_Buttons;
    private Vector3[] m_OldPositions;
    private Vector3[] m_NewPositions;

    // Use this for initialization
    private void Awake()
    {
        m_Buttons = GetComponentsInChildren<Button>();

        m_OldPositions = m_Buttons.Select(button => button.transform.position).ToArray();
        m_NewPositions =
            m_Buttons.Select(
                button =>
                    new Vector3(
                        Camera.main.ViewportToScreenPoint(new Vector3(0f, 0f)).x,
                        button.transform.position.y)).ToArray();

        var managerUI = FindObjectOfType<ManagerUI>();
        managerUI.onEnableDialogueOptions.AddListener(OnEnableDialogueOptions);
        managerUI.onEnableTextBox.AddListener(OnEnableTextBox);
    }

    private void OnEnableDialogueOptions()
    {
        StartCoroutine(PlayAnimation());
    }

    private void OnEnableTextBox()
    {
        if (gameObject.activeSelf)
            StartCoroutine(PlayAnimation(true));
    }

    private IEnumerator PlayAnimation(bool reverse = false)
    {
        m_Buttons[3].image.CrossFadeAlpha(!reverse ? 0f : 1f, 0f, false);
        m_Buttons[3].image.CrossFadeAlpha(!reverse ? 1f : 0f, m_MaxTime, false);

        var deltaTime = 0f;
        while (deltaTime < m_MaxTime)
        {
            for (var i = 0; i < m_Buttons.Length - 1; i++)
            {
                m_Buttons[i].transform.position =
                    Vector3.LerpUnclamped(
                        m_NewPositions[i],
                        m_OldPositions[i],
                        !reverse ?
                            m_AnimationCurve.Evaluate(deltaTime / m_MaxTime) :
                            1f - m_AnimationCurve.Evaluate(deltaTime / m_MaxTime));
            }

            deltaTime += Time.deltaTime;
            yield return null;
        }

        for (var i = 0; i < m_Buttons.Length - 1f; i++)
            m_Buttons[i].transform.position = !reverse ? m_OldPositions[i] : m_NewPositions[i];
    }
}
