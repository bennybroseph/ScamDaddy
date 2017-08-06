using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ManagerUI : MonoBehaviour
{
    private enum SelectedOption
    {
        None,
        Option1,
        Option2,
        Option3,
        Sell,
    }

    [SerializeField]
    private CustomerObject[] m_CustomerObjects;

    [SerializeField, Range(0f, 1f), Space]
    private float m_CrossfadeTime = 0.2f;

    [SerializeField, Space]
    private int m_CharacterTransformIndex;

    [SerializeField, Space]
    private Image m_BackgroundImage;

    [SerializeField, Space]
    private GameObject m_TextBoxPanel;
    [SerializeField]
    private bool m_HideTextOnOptions;

    [SerializeField, Space]
    private GameObject m_DialogueOptionsMenu;

    [SerializeField, Space]
    private ScrollingText m_TextBox;
    [SerializeField]
    private Text m_CharacterNameText;

    [SerializeField, Space]
    private Button m_SellButton;

    [SerializeField, Space]
    private Button m_Option1;
    [SerializeField]
    private Button m_Option2;
    [SerializeField]
    private Button m_Option3;

    [SerializeField, Space]
    private AudioSource m_DoorOpenSource;

    private FadeScript m_FadeScript;

    private Animator m_CharacterAnimator;

    private int m_DesireToBuy;

    private SelectedOption m_SelectedOption;

    private UnityEvent m_OnEnableTextBox = new UnityEvent();
    private UnityEvent m_OnEnableDialogueOptions = new UnityEvent();

    public UnityEvent onEnableTextBox { get { return m_OnEnableTextBox; } }
    public UnityEvent onEnableDialogueOptions { get { return m_OnEnableDialogueOptions; } }

    public Button sellButton { get { return m_SellButton; } }

    public Button option1 { get { return m_Option1; } }
    public Button option2 { get { return m_Option2; } }
    public Button option3 { get { return m_Option3; } }

    private void Awake()
    {
        m_FadeScript = FindObjectOfType<FadeScript>();

        m_Option1.onClick.AddListener(OnClickOption1);
        m_Option2.onClick.AddListener(OnClickOption2);
        m_Option3.onClick.AddListener(OnClickOption3);

        m_SellButton.onClick.AddListener(OnClickSell);
    }

    // Use this for initialization
    private IEnumerator Start()
    {
        m_DialogueOptionsMenu.SetActive(false);

        var onInitialCustomer = true;
        foreach (var customerObject in m_CustomerObjects)
        {
            m_BackgroundImage.sprite = customerObject.backgroundSprite;

            if (m_CharacterAnimator != null)
                Destroy(m_CharacterAnimator.gameObject);

            var characterGameObject = Instantiate(customerObject.characterPrefab);
            characterGameObject.transform.SetParent(transform, false);
            characterGameObject.transform.SetSiblingIndex(m_CharacterTransformIndex);

            m_CharacterAnimator = characterGameObject.GetComponent<Animator>();

            m_DesireToBuy = 0;

            EnableTextBox();

            m_CharacterAnimator.gameObject.SetActive(false);

            if (!onInitialCustomer && m_FadeScript)
            {
                m_DoorOpenSource.Play();

                var fadeIn = m_FadeScript.FadeIn();
                while (fadeIn.MoveNext())
                    yield return null;
            }

            m_CharacterNameText.text = "You";
            m_TextBox.text = customerObject.dialogue.openingStatement;

            onInitialCustomer = false;

            yield return WaitForUser();

            m_CharacterAnimator.gameObject.SetActive(true);

            var earlySale = false;
            foreach (var statement in customerObject.dialogue.statements)
            {
                m_CharacterNameText.text = customerObject.name;
                m_TextBox.text = statement.text;

                m_CharacterAnimator.CrossFade("Neutral", m_CrossfadeTime);

                EnableDialogueOptions();

                m_Option1.GetComponentInChildren<Text>().text = statement.options[0].text;
                m_Option2.GetComponentInChildren<Text>().text = statement.options[1].text;
                m_Option3.GetComponentInChildren<Text>().text = statement.options[2].text;

                m_SellButton.gameObject.SetActive(!customerObject.hideSellButton);

                m_SelectedOption = SelectedOption.None;
                while (m_SelectedOption == SelectedOption.None)
                    yield return null;

                yield return null;

                EnableTextBox();

                switch (m_SelectedOption)
                {
                    case SelectedOption.Option1:
                        m_TextBox.text = statement.options[0].response;
                        m_CharacterAnimator.CrossFade(statement.options[0].mood, m_CrossfadeTime);

                        m_DesireToBuy += statement.options[0].value;
                        break;
                    case SelectedOption.Option2:
                        m_TextBox.text = statement.options[1].response;
                        m_CharacterAnimator.CrossFade(statement.options[1].mood, m_CrossfadeTime);

                        m_DesireToBuy += statement.options[1].value;
                        break;
                    case SelectedOption.Option3:
                        m_TextBox.text = statement.options[2].response;
                        m_CharacterAnimator.CrossFade(statement.options[2].mood, m_CrossfadeTime);

                        m_DesireToBuy += statement.options[2].value;
                        break;

                    case SelectedOption.Sell:
                        earlySale = true;
                        yield return AttemptToSell(customerObject);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                if (earlySale)
                    break;

                yield return WaitForUser();
            }
            if (!earlySale)
                yield return AttemptToSell(customerObject);

            if (m_FadeScript)
            {
                var fadeOut = m_FadeScript.FadeOut();
                while (fadeOut.MoveNext())
                    yield return null;

                m_TextBox.text = string.Empty;
                m_CharacterNameText.text = string.Empty;
            }
        }
    }

    private void EnableTextBox()
    {
        m_TextBoxPanel.SetActive(true);

        m_OnEnableTextBox.Invoke();
    }

    private void EnableDialogueOptions()
    {
        m_DialogueOptionsMenu.SetActive(true);
        if (m_HideTextOnOptions)
            m_TextBoxPanel.SetActive(false);

        m_OnEnableDialogueOptions.Invoke();
    }

    private IEnumerator AttemptToSell(CustomerObject customerObject)
    {
        CustomerObject.DesireSettings.DesireResponse desireValue;
        if (m_DesireToBuy <= customerObject.desireSettings.min.value)
            desireValue = customerObject.desireSettings.min.responses;
        else if (m_DesireToBuy >= customerObject.desireSettings.max.value)
            desireValue = customerObject.desireSettings.max.responses;
        else
            desireValue = customerObject.desireSettings.neutralResponses;

        m_CharacterAnimator.CrossFade("Neutral", 0.2f);

        if (desireValue.salesPitch != string.Empty)
        {
            m_CharacterNameText.text = "You";
            m_TextBox.text = desireValue.salesPitch;

            yield return WaitForUser();
        }

        if (desireValue.characterResponse != string.Empty)
        {
            m_CharacterNameText.text = customerObject.name;

            m_TextBox.text = desireValue.characterResponse;
            m_CharacterAnimator.CrossFade(desireValue.mood, m_CrossfadeTime);

            yield return WaitForUser();
        }

        if (desireValue.selfResponse != string.Empty)
        {
            m_CharacterNameText.text = "You";

            m_TextBox.text = desireValue.selfResponse;

            yield return WaitForUser();
        }
    }

    private IEnumerator WaitForUser()
    {
        while (!Input.anyKeyDown || m_TextBox.isScrolling)
            yield return null;

        yield return null;
    }

    private void OnClickOption1()
    {
        m_SelectedOption = SelectedOption.Option1;
    }
    private void OnClickOption2()
    {
        m_SelectedOption = SelectedOption.Option2;
    }
    private void OnClickOption3()
    {
        m_SelectedOption = SelectedOption.Option3;
    }
    private void OnClickSell()
    {
        m_SelectedOption = SelectedOption.Sell;
    }
}
