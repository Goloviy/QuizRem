using System;
using System.Collections.Generic;
using Coffee.UIExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI answerText;
    [SerializeField] private Button button;
    [SerializeField] private Image background;
    [SerializeField] private Image expertSelection;
    [SerializeField] private List<UIParticle> winParticles;

    public int answerIndex { get; private set; }
    public Action<int> answerWasPressed;

    public void InitializeView()
    {
        button.onClick.AddListener(AnswerClicked);
    }

    public void SetInteractableButton(bool _interactable)
    {
        button.interactable = _interactable;
    }

    public void UpdateAnswer(string _text, int _index)
    {
        answerText.text = _text;
        answerIndex = _index;
        background.color = Color.white;
        gameObject.SetActive(true);
        // expertSelection.DOKill();
        expertSelection.gameObject.SetActive(false);
        // expertBack.gameObject.SetActive(false);
    }
    
    public void PlayParticles()
    {
        for (int i = 0; i < winParticles.Count; i++)
        {
            winParticles[i].Stop();
            winParticles[i].Play();
        }
    }

    public void RecolorAnswer(Color _color)
    {
        background.color = _color;
    }

    private void AnswerClicked()
    {
        answerWasPressed?.Invoke(answerIndex);
    }

    public void HideAnswer()
    {
        gameObject.SetActive(false);
    }

    public void ShowAnswer()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        gameObject.SetActive(true);
    }
}