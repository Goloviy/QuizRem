using System;
using UnityEngine;

public class GlobalInitializator : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private TextAsset questionsDataBase;
    [SerializeField] private TextAsset fastGameQuestions;
    [SerializeField] private TextAsset tutorialQuestion;
    [SerializeField] private LoadingScreen loadingScreen;
    
    private DataBaseWorker db;
    public event Action InitializationComplete;
    
    protected void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        loadingScreen.InitializeView(ref InitializationComplete);
        db = new DataBaseWorker(questionsDataBase.text, fastGameQuestions.text,tutorialQuestion.text, OnDataBaseParsed, ShareResourcesManager.Instance.categoryIcons);
    }
    
    private void OnDataBaseParsed()
    {
        InitializationComplete?.Invoke();
        loadingScreen.Unsubscribe(ref InitializationComplete);
        Debug.Log("DatabaseReady");
    }
}
