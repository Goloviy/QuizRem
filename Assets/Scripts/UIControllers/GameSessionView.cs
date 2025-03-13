using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UIControllers;
using UnityEngine;

public class GameSessionView : BaseUIScreen
{
    [SerializeField] private GameSessionController controller;
    [SerializeField] private TextMeshProUGUI questionLabel;
    [SerializeField] private List<AnswerView> answerViews;
    [SerializeField] private UIRaycastReceiver uiRaycastBlocker;
    [SerializeField] private TimerUI timerView;
    
    [SerializeField] private Color answerSelected;
    [SerializeField] private Color answerCorrect;
    [SerializeField] private Color answerIncorrectColor;

    public override void InitializeView()
    {
        base.InitializeView();
        controller.UpdateQuestionInfo += SetupSessionInfo;
        controller.PlayerAnsweredQuestion += OnPlayerAnswered;
        controller.BlockUserInput += BlockerInputStatus;
        controller.GameSessionEnd += HideScreen;
        controller.gameTimer.OnTimerTick += timerView.UpdateTime;
        for (var i = 0; i < answerViews.Count; i++)
        {
            answerViews[i].InitializeView();
            answerViews[i].answerWasPressed += PlayerSelectAnswer;
        }
    }
    
    public void BlockerInputStatus(bool _status)
    {
        uiRaycastBlocker.gameObject.SetActive(_status);
    }
    
    private void PlayerSelectAnswer(int _index)
    {
        answerViews.FirstOrDefault(answer => answer.answerIndex == _index)?.RecolorAnswer(answerSelected);
        controller.PlayerChooseAnswer(_index);
    }
    
    private void OnPlayerAnswered(int _questionID, bool _result)
    {
        var correctIndex = controller.CurrentQuestion.correctAnswerIndex;
        
        for (int i = 0; i < answerViews.Count; i++)
        {
            if (answerViews[i].answerIndex == correctIndex)
            {
                answerViews[i].RecolorAnswer(answerCorrect);
                if (_result)
                {
                    answerViews[i].transform.DOPunchScale(new Vector3(.1f, .25f, 0), 0.5f, 1, 0);
                    //answerViews[i].PlayParticles();
                }
            }
        }
        controller.AnswerDelayOut(_result);
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