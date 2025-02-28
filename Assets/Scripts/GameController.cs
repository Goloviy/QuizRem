using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviourSingleton<GameController>
{
    [SerializeField] private GameSessionSettings sessionSettingsConfig;
    [SerializeField] private GameSessionController session;
    private int coutQuestion = 3; //temp solution for test 

    private DataBaseWorker db;
    public Action GameSessionStarted;

    public void Init(DataBaseWorker _dataBase)
    {
        db = _dataBase;
        session.InitializeController(sessionSettingsConfig);
        Debug.Log("Game Controller Init");
    }

    public void StartGameSession()
    {
        var sessionQuestions = new List<Question>();
        var replaceQuestions = new List<Question>();
        for (var i = 0; i <sessionSettingsConfig.data.sessionQuestions.Count; i++)
        {
            sessionQuestions.AddRange(db.GetQuestions(sessionSettingsConfig.data.sessionQuestions[i].difficulty, 
                QuestionType.QuestionMain, sessionSettingsConfig.data.sessionQuestions[i].count));
            replaceQuestions.AddRange(db.GetQuestions(sessionSettingsConfig.data.sessionQuestions[i].difficulty, 
                QuestionType.QuestionMain, sessionSettingsConfig.data.sessionQuestions[i].count));
        }

        session.InitializeGameSession(sessionQuestions, replaceQuestions);
        GameSessionStarted?.Invoke();
        Debug.Log("StartGameSession");
    }
}