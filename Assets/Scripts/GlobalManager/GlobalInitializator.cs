using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInitializator : MonoBehaviourSingleton<GlobalInitializator>
{
    [SerializeField] private TextAsset questionsDataBase;
    [SerializeField] private TextAsset fastGameQuestions;
    [SerializeField] private TextAsset tutorialQuestion;
    private DataBaseWorker db;
    
    protected override void SingletonAwakened()
    {
        Initialize();
    }

    protected void Initialize()
    {
        db = new DataBaseWorker(questionsDataBase.text, fastGameQuestions.text,tutorialQuestion.text, OnDataBaseParsed, ShareResourcesManager.Instance.categoryIcons);
    }
    
    private void OnDataBaseParsed()
    {
        /*Debug.LogWarning("DatabaseReady");*/
    }
}
