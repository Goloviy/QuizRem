using UnityEngine;

public class GlobalInitializator : MonoBehaviour
{
    [SerializeField] private LoadingScreen loadingScreen;
    
    [Header("Controllers")]
    [SerializeField] private GameController gameController;
    [SerializeField] private UIController uiController;
    
    [Header("Question for games")]
    [SerializeField] private TextAsset questionsDataBase;
    [SerializeField] private TextAsset fastGameQuestions;
    [SerializeField] private TextAsset tutorialQuestion;

    private DataBaseWorker db;
    
    protected void Awake()
    {
        Initialize();
    }

    private async void Initialize()
    {
        loadingScreen.InitializeView();
        db = new DataBaseWorker(questionsDataBase.text, fastGameQuestions.text,tutorialQuestion.text, 
            ShareResourcesManager.Instance.categoryIcons);
        await db.WaitForInitialization();
        gameController.Init(db);
        uiController.Init();
        uiController.InitSubscriptions();
        loadingScreen.OnInitializationComplete();
    }
}
