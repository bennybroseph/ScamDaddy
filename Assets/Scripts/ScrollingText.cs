﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingText : Text
{
    [SerializeField, Range(0f, 0.25f), Space]
    private float m_TextSpeed = 0.1f;

    private bool m_IsScrolling;

    public bool isScrolling { get { return m_IsScrolling; } }

    public override string text
    {
        get
        {
            return m_Text;
        }
        set
        {
            m_Text = value;
            StopAllCoroutines();
            StartCoroutine(ScrollCoroutine());
        }
    }


    private IEnumerator ScrollCoroutine()
    {
        m_IsScrolling = true;

        var i = 0;
        var deltaTime = 0f;
        var originalText = m_Text;

        m_Text = "<color=#00000000>" + m_Text + "</color>";
        UpdateGeometry();

        while (i <= originalText.Length)
        {
            if (Input.anyKeyDown)
            {
                m_Text = originalText;
                UpdateGeometry();

                yield return null;
                break;
            }

            if (deltaTime > m_TextSpeed)
            {
                m_Text =
                    originalText.Substring(0, i) +
                    "<color=#00000000>" +
                    originalText.Substring(i) +
                    "</color>";

                UpdateGeometry();

                ++i;
                deltaTime -= m_TextSpeed;
            }
            else
            {
                deltaTime += Time.deltaTime;
                yield return null;
            }
        }

        m_IsScrolling = false;
    }
}
