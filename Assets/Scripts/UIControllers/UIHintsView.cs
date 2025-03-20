using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHintsView : MonoBehaviour
{
    [SerializeField] private Color lockHintColor;
   
    [SerializeField] private GameSessionController controller;

    //[SerializeField] private InventoryController inventoryController;

    // 50/50 hints count
    [SerializeField] private TextMeshProUGUI halfHintsCount;
    [SerializeField] private TextMeshProUGUI secondChanceHintsCount;
    [SerializeField] private TextMeshProUGUI replaceQuestionHintsCount;
   
    [SerializeField] private TextMeshProUGUI halfHintsPrice;
    [SerializeField] private TextMeshProUGUI secondChanceHintsPrice;
    [SerializeField] private TextMeshProUGUI replaceQuestionHintsPrice;
   
    [SerializeField] private GameObject halfHintsPriceView;
    [SerializeField] private GameObject secondChanceHintsPriceView;
    [SerializeField] private GameObject replaceQuestionHintsPriceView;
   
    [SerializeField] private Button removeHalfHint;
    [SerializeField] private Button replaceQuestionHint;
    [SerializeField] private Button secondChanceHint;

    [SerializeField] private Image removeHalfHintBack;
    [SerializeField] private Image removeHalfHintIcon;
   
    [SerializeField] private Image replaceQuestionHintBack;
    [SerializeField] private Image replaceQuestionHintIcon;
   
    [SerializeField] private Image secondChanceHintBack;
    [SerializeField] private Image secondChanceHintIcon;
    
    public void InitializeView()
    {
        removeHalfHint.onClick.AddListener(() => { controller.TryUseHint(HintsType.HalfAnswers);});
        replaceQuestionHint.onClick.AddListener(() => { controller.TryUseHint(HintsType.ReplaceQuestion);});
        secondChanceHint.onClick.AddListener(() => { controller.TryUseHint(HintsType.SecondChance);});
      
        // controller.SessionWasInitialized += SetupHintsInfo;
        // controller.HintWasUsed += UpdateHintInfo;
        // controller.UpdateQuestionInfo += UpdateHintsInfo;
        // controller.UpdateHintInfoTutorial += UpdateHintInfoTutorial;

    }
}
