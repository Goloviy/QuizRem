using System;
using System.Collections.Generic;
using UnityEngine;

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

    public void InitializeController(GameSessionSettings _config)
    {
        Debug.Log("GameSessionController initialized empty");
        config = _config;
        //need  create and init timer
    }

    public void InitializeGameSession(List<Question> _mainQuestions, List<Question> _replaceQuestions)
    {
        sessionQuestions = _mainQuestions;
        questionsForReplace = _replaceQuestions;
        CurrentQuestion = sessionQuestions[0];
        BlockUserInput?.Invoke(false);
        SessionWasInitialized?.Invoke();
        ShowNextQuestion();
    }

    public void PlayerChooseAnswer(int _answerID)
    {
        BlockUserInput?.Invoke(true);
        var isAnswerCorrect = false;

        isAnswerCorrect = CurrentQuestion.answers[_answerID].isRightOne;

        if (isAnswerCorrect)
        {
            answeredQuestionsCount++;
        }

        PlayerAnsweredQuestion?.Invoke(CurrentQuestion.id, isAnswerCorrect);
    }

    public void AnswerDelayOut(bool _answerIsCorrect)
    {
        // delaysTimer.ResetTimer(2f, () =>
        // {
        //     if (_answerIsCorrect)
        //     {
        //         UpdateRewardsInfo?.Invoke();
        //     }
        //     else
        //     {
        //         GameSessionEnd?.Invoke();
        //     }
        // });
        if (_answerIsCorrect)
        {
            UpdateRewardsInfo?.Invoke();
            ShowNextQuestion();
        }
        else
        {
            GameSessionEnd?.Invoke();
            sessionQuestions = null;
            questionsForReplace = null;
            CurrentQuestion = null;
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