using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScreen : BaseUIScreen
{
    [SerializeField] private Button playGameButton;
    
    public Action OnPlayButtonClicked;
    
    public override void InitializeView()
    {
        base.InitializeView();
      
        playGameButton.onClick.AddListener(() => {OnPlayButtonClicked?.Invoke();});
    }
}
