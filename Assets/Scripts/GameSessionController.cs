using System;
using System.Collections.Generic;

public class GameSessionController
{
    private List<Question> sessionQuestions;
    private List<Question> questionsForReplace;
    public Question CurrentQuestion { get; private set; }
    public Action SessionWasInitialized;
    
    public GameSessionController(List<Question> _mainQuestions, List<Question> _replaceQuestions)
    {
        sessionQuestions = _mainQuestions;
        questionsForReplace = _replaceQuestions;
        CurrentQuestion = sessionQuestions[0];
        SessionWasInitialized?.Invoke();
    }
}