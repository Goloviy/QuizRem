using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSessionView : BaseUIScreen
{
    [SerializeField] private GameSessionController controller;
    [SerializeField] private TextMeshProUGUI questionLabel;
    [SerializeField] private List<AnswerView> answerViews;
    
    [SerializeField] private Color answerSelected;
    [SerializeField] private Color answerCorrect;

    public override void InitializeView()
    {
        base.InitializeView();
        controller.UpdateQuestionInfo += SetupSessionInfo;
        for (var i = 0; i < answerViews.Count; i++)
        {
            answerViews[i].InitializeView();
            answerViews[i].answerWasPressed += PlayerSelectAnswer;
        }
    }
    
    private void PlayerSelectAnswer(int _index)
    {
        for (int i = 0; i < answerViews.Count; i++)
        {
            if (answerViews[i].answerIndex == _index)
            {
                Debug.LogWarning("Player Select");
                answerViews[i].RecolorAnswer(answerSelected);
            }
        }
        //controller.PlayerChooseAnswer(_index);
    }

    private void SetupSessionInfo()
    {
        ChangeQuestion(controller.CurrentQuestion);
    }

    private void ChangeQuestion(Question _currentQuestion)
    {
        questionLabel.text = _currentQuestion.questionText;
        for (var i = 0; i < answerViews.Count; i++)
        {
            answerViews[i].ShowAnswer();
            answerViews[i].UpdateAnswer(_currentQuestion.answers[i].answerLabel,i);
        }
    }
}