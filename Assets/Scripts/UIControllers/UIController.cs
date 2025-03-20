 using System;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviourSingleton<UIController>
{
    [SerializeField] private MainMenuScreen mainMenuScreen;
    [SerializeField] private GameSessionView gameSessionView;
    [SerializeField] private GameController gameController;

    [Header("Default Screen")] [SerializeField]
    private BaseUIScreen defaultScreen;

    //TODO : Find a better way to do this
    private List<BaseUIScreen> screensOrder;

    private Stack<BaseUIScreen> screens = new Stack<BaseUIScreen>();

    public Action<string> OnShowScreen;
    
    public void Init()
    {
        screensOrder = new List<BaseUIScreen>(4) { mainMenuScreen };
        mainMenuScreen.InitializeView();
        gameSessionView.InitializeView();
        
        Debug.Log("UIController Init");
    }

    public void InitSubscriptions()
    {
        mainMenuScreen.OnPlayButtonClicked += ShowGameSession;
        gameController.GameSessionStarted += SetupGameSessionView;
    }
    
    private void ShowGameSession()
    {
        gameController.StartGameSession();
    }
    
    private void SetupGameSessionView()
    {
        //staticPanel.gameObject.SetActive(false);
        ShowScreen(gameSessionView);
    }
    
    public void  EndGameSession()
    {
        //staticPanel.gameObject.SetActive(true);
        HideScreen(gameSessionView);
    }

    //TODO : Bug - returnToPrevScreen not working
    public void ShowScreen(BaseUIScreen _screen)
    {
        var showAnimated = false;
        if (screens.Count > 0)
        {
            var screen = screens.Peek();
            if (screen == _screen)
            {
                screen.ShowScreen();
                OnShowScreen?.Invoke(screen.name);
                return;
            }

            if (!screen.returnToPrevScreen)
            {
                screens.Pop();
            }

            if (screensOrder.Contains(screen) && screensOrder.Contains(_screen))
            {
                var startIndex = screensOrder.IndexOf(screen);
                var endIndex = screensOrder.IndexOf(_screen);
                var dif = startIndex - endIndex;
                if (dif > 0)
                {
                    for (int i = startIndex; i > endIndex; i--)
                    {
                        screensOrder[i].HideScreen(1, i);
                    }
                    /*screen.HideScreen(1);*/
                }
                else
                {
                    for (int i = startIndex; i < endIndex; i++)
                    {
                        screensOrder[i].HideScreen(-1, i);
                    }
                }

                showAnimated = true;
            }
            else
            {
                screen.HideScreen();
            }
        }

        OnShowScreen?.Invoke(_screen.name);
        screens.Push(_screen);
        _screen.ShowScreen(showAnimated);
    }

    public void HideScreen(BaseUIScreen _screen)
    {
        Debug.Log("HideScreen");
        screens.Pop().HideScreen();
        if (screens.Count > 0)
        {
            screens.Peek().ShowScreen();
        }
        else
        {
            ShowScreen(defaultScreen);
        }
    }
}