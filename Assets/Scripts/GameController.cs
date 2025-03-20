using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class GameController : MonoBehaviourSingleton<GameController>
{
    [SerializeField] private GameSessionSettings sessionSettingsConfig;
    [SerializeField] private GameSessionController session;
    private int coutQuestion = 3; //temp solution for test 

    private DataBaseWorker db;
    private InventoryController inventoryController;
    public Action GameSessionStarted;

    private Stopwatch stopwatch;
    
    public void Init(DataBaseWorker _dataBase)
    {
        db = _dataBase;
        var hints = new List<Hint>(3);
        hints[0].count = 3;
        hints[1].count = 3;
        hints[2].count = 3;
        hints[0].hintType = HintsType.HalfAnswers;
        hints[1].hintType = HintsType.ReplaceQuestion;
        hints[3].hintType = HintsType.SecondChance;
        inventoryController = new InventoryController(hints);
        session.InitializeController(sessionSettingsConfig,inventoryController);
        
        Debug.Log("Game Controller Init");
    }

    private void EndTimer()
    {
        stopwatch.Stop();
        Debug.Log(stopwatch.ElapsedMilliseconds);
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