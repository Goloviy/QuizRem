using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    [SerializeField] private GameSessionController session;
    private int coutQuestion = 3;//temp solution for test 

    private DataBaseWorker db;
    public Action GameSessionStarted;

    public void Init(DataBaseWorker _dataBase)
    {
        db = _dataBase;
        session.InitializeController();
        Debug.Log("Game Controller Init");
    }

    public void StartGameSession()
    {
        var sessionQuestions = new List<Question>();
        var replaceQuestions = new List<Question>();
        for (int i = 0; i < coutQuestion; i++)
        {
            sessionQuestions.AddRange(db.GetQuestions(i+1,QuestionType.QuestionMain,3));
            replaceQuestions.AddRange(db.GetQuestions(i+1,QuestionType.QuestionMain,3));
        }
        
        session.InitializeGameSession(sessionQuestions, replaceQuestions);
        GameSessionStarted?.Invoke();
        Debug.Log("StartGameSession");
    }
}