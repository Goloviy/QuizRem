using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class LoadingScreen : BaseUIScreen
{
    [SerializeField] private ProgressBarController progressBar;
    private bool initializationComplete;
    private float progress;

    public void InitializeView()
    {
        base.InitializeView();
        progressBar.InitializeView();
        ShowScreen(true);
    }

    public void OnInitializationComplete()
    {
        initializationComplete = true;
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

        yield return progressBar.UpdateProgressSmoothly(1, 0.1f, 0.2f);
        HideScreen();

        Debug.Log("FakeProgress ended");
    }

    public override void ShowScreen(bool _animated = false)
    {
        initializationComplete = false;
        progressBar.UpdateProgress(0.1f);
        base.ShowScreen(false);

        StartCoroutine(FakeProgress());
    }
}