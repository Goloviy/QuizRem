using TMPro;
using UnityEngine;

public class GameSessionView : BaseUIScreen
{
    [SerializeField] private GameSessionController controller;
    [SerializeField] private TextMeshProUGUI questionLabel;

    public override void InitializeView()
    {
        base.InitializeView();
        controller.UpdateQuestionInfo += SetupSessionInfo;
    }

    private void SetupSessionInfo()
    {
        ChangeQuestion(controller.CurrentQuestion);
    }

    private void ChangeQuestion(Question _currentQuestion)
    {
        questionLabel.text = _currentQuestion.questionText;
    }
}