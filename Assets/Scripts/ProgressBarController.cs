using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
   [SerializeField] private Image progressBar;
   [SerializeField] private Image progressBarBack;

   [SerializeField] private TextMeshProUGUI progressText;

   [SerializeField] private int backDeltaSize;

   [SerializeField] private Image fillImageReplacer;
   [SerializeField] private float minSize;
   
   private float deltaPercent;

   public Action OnProgressFilled;

   private TweenerCore<Vector2, Vector2, VectorOptions> currentTween;
   
   public void InitializeView()
   {
      if (progressBar == null)
      {
         Debug.LogError(name);
      }
      deltaPercent = math.abs((progressBarBack.rectTransform.sizeDelta.x + backDeltaSize) / 100);
      
      progressBar.rectTransform.sizeDelta = new Vector2(0, progressBar.rectTransform.sizeDelta.y);
      progressBar.gameObject.SetActive(true);
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="_newProgress"> percents from 0 to 1</param>
   public void UpdateProgress(float _newProgress)
   {
      if (_newProgress < 0.005f)
      {
         progressBar.rectTransform.sizeDelta = new Vector2(0, progressBar.rectTransform.sizeDelta.y);
      }
      else
      {
         _newProgress *= 100;
         var delta = math.min(_newProgress * deltaPercent, progressBarBack.rectTransform.sizeDelta.x + backDeltaSize);
         delta = math.max(delta, minSize);
         progressBar.rectTransform.sizeDelta = new Vector2(delta, progressBar.rectTransform.sizeDelta.y);

         if (_newProgress >= 99.99f)
         {
            OnProgressBarFilled();
         }
         else
         {
            if (fillImageReplacer != null)
            {
               fillImageReplacer.gameObject.SetActive(false);
            }
         }
      }
   }

   /// <summary>
   /// 
   /// </summary>
   /// <param name="_newProgress"> percents from 0 to 1</param>
   public void UpdateProgressSmoothly(float _newProgress, float _delay = 0.33f, float _time = 0.5f)
   {
      Debug.Log("TryKIllTwee");
      currentTween?.Kill();

      _newProgress *= 100;
      var delta = math.min(_newProgress * deltaPercent, progressBarBack.rectTransform.sizeDelta.x + backDeltaSize);
      currentTween = progressBar.rectTransform.DOSizeDelta(new Vector2(delta, progressBar.rectTransform.sizeDelta.y), _time).SetDelay(_delay)
         .OnComplete( () => Debug.Log("UpdateProgressSmoothly Ended"));
      
      if (_newProgress >= 99.99f)
      {
         Debug.Log("NewProgress > 99");
         currentTween.OnComplete(OnProgressBarFilled);
      }
      
      Debug.Log("End UpdateProgressSmoothly");
   }

   private void OnProgressBarFilled()
   {
      OnProgressFilled?.Invoke();  
            
      if (fillImageReplacer != null)
      {
         fillImageReplacer.gameObject.SetActive(true);
      }
   }

   public void ChangeProgressText(string _newProgress)
   {
      if (progressText != null)
      {
         progressText.text = _newProgress;
      }
   }

   public void ChangeFillImageView(bool _isShown)
   {
      progressBar.gameObject.SetActive(_isShown);
   }
}
