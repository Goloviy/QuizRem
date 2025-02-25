using System;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionController : MonoBehaviour
{
    private List<Question> sessionQuestions;
    private List<Question> questionsForReplace;
    
    public Question CurrentQuestion { get; private set; }
    public Action SessionWasInitialized;
    public Action UpdateQuestionInfo;
    public Action GameSessionEnd;
    public int answeredQuestionsCount { get; private set; }
    public int maxQuestionsCount => sessionQuestions.Count;

    public void InitializeController()
    {
        Debug.Log("GameSessionController initialized empty");
    }

    public void InitializeGameSession(List<Question> _mainQuestions, List<Question> _replaceQuestions)
    {
        sessionQuestions = _mainQuestions;
        questionsForReplace = _replaceQuestions;
        CurrentQuestion = sessionQuestions[0];
        SessionWasInitialized?.Invoke();
        ShowNextQuestion();
    }

    private void ShowNextQuestion()
    {
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