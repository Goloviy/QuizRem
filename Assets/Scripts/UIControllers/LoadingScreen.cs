using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LoadingScreen : BaseUIScreen
{
    [SerializeField] private GlobalInitializator controller; 
    
    [SerializeField] private ProgressBarController progressBar;
    [SerializeField] private TextMeshProUGUI hintText;
    [SerializeField] private Button backgroundButton;

    public Action OnLoadingComplete;

    private bool initializationComplete;
    private float progress = 0f;
    
    public override void InitializeView()
    {
        base.InitializeView();
        
        controller.InitializationComplete += () => initializationComplete = true;
        backgroundButton.onClick.AddListener(OnBackgroundClick);
        
        progressBar.InitializeView();
    }
    
    private void OnInitializationComplete()
    {
        initializationComplete = true;
        
        /*hintText.text = "Коснитесь, что бы продолжить";
        hintText.alpha = 0.75f;
        hintText.DOFade(1, 0.75f).SetLoops(-1, LoopType.Yoyo);
        hintText.transform.DOScale(new Vector3(1.15f, 1.35f, 1f), 0.75f).SetLoops(-1, LoopType.Yoyo);*/

        /*backgroundButton.interactable = true;*/
        OnLoadingComplete?.Invoke();
    }
    
    private void OnBackgroundClick()
    {
        OnLoadingComplete?.Invoke();
    }
    
    private IEnumerator FakeProgress()
    {
        while (!initializationComplete)
        {
            progress += Random.Range(0.05f, 0.35f);
            progress = math.min(0.9f, progress);
            progressBar.UpdateProgressSmoothly(progress, 0.1f, 0.3f);
            yield return new WaitForSecondsRealtime(0.3f);
        }
        
        progressBar.UpdateProgressSmoothly(1, 0.1f, 0.2f);
        OnInitializationComplete();
        
        CustomLogger.Log("FakeProgress ended");
    }

    public override void ShowScreen(bool _animated = false)
    {
        backgroundButton.interactable = false;
        initializationComplete = false;
        hintText.DOKill();
        progressBar.UpdateProgress(0.1f);
        base.ShowScreen(false);
        
        StartCoroutine(FakeProgress());
    }
}
