using System;
using System.Collections.Generic;
using UnityEngine;
using SelfTimer;

public class GameSessionController : MonoBehaviour
{
    private GameSessionSettings config;
    private List<Question> sessionQuestions;
    private List<Question> questionsForReplace;

    //public List<AnsweredQuestionInfo> answeredQuestions { get; private set; }

    public Question CurrentQuestion { get; private set; }
    public Action SessionWasInitialized;
    public Action UpdateQuestionInfo;
    public Action<int, bool> PlayerAnsweredQuestion;
    public Action UpdateRewardsInfo;
    public Action GameSessionEnd;
    public Action<bool> BlockUserInput;
    public int answeredQuestionsCount { get; private set; }
    public int maxQuestionsCount => sessionQuestions.Count;
    public Timer gameTimer { get; private set; }

    public void InitializeController(GameSessionSettings _config)
    {
        Debug.Log("GameSessionController initialized empty");
        config = _config;
        var time = (long)config.data.mainTimerTime * 1000;
        gameTimer = TimerManager.Instance.CreateTimer(time, EndSession);
        //need  create and init timer
    }

    public void InitializeGameSession(List<Question> _mainQuestions, List<Question> _replaceQuestions)
    {
        sessionQuestions = _mainQuestions;
        questionsForReplace = _replaceQuestions;
        CurrentQuestion = sessionQuestions[0];
        BlockUserInput?.Invoke(false);
        SessionWasInitialized?.Invoke();
        gameTimer.Start();
        ShowNextQuestion();
    }

    private void EndSession()
    {
        GameSessionEnd?.Invoke();
        sessionQuestions = null;
        questionsForReplace = null;
        CurrentQuestion = null;
    }

    public void PlayerChooseAnswer(int _answerID)
    {
        BlockUserInput?.Invoke(true);
        gameTimer.Stop();
        var isAnswerCorrect = false;

        isAnswerCorrect = CurrentQuestion.answers[_answerID].isRightOne;

        if (isAnswerCorrect)
        {
            answeredQuestionsCount++;
        }

        var tempTimer = TimerManager.Instance.CreateTimer((long)(config.data.answerDelayTime * 1000),
            () => { PlayerAnsweredQuestion?.Invoke(CurrentQuestion.id, isAnswerCorrect); });
        tempTimer.Start();
    }

    public void AnswerDelayOut(bool _answerIsCorrect)
    {
        var tempTimer = TimerManager.Instance.CreateTimer(2000, () => { CheckAnswer(_answerIsCorrect); });
        tempTimer.Start();
    }

    private void CheckAnswer(bool _answerIsCorrect)
    {
        Debug.Log("CheckAnswer called");
        if (_answerIsCorrect)
        {
            UpdateRewardsInfo?.Invoke();
            ShowNextQuestion();
            gameTimer.ResetTime();
        }
        else
        {
            EndSession();
        }
    }

    private void ShowNextQuestion()
    {
        BlockUserInput?.Invoke(false);
        if (answeredQuestionsCount >= maxQuestionsCount)
        {
            GameSessionEnd?.Invoke();
        }
        else
        {
            CurrentQuestion = sessionQuestions[answeredQuestionsCount];
            UpdateQuestionInfo?.Invoke();
        }
    }
}