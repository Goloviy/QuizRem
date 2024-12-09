using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public abstract class BaseUIScreen : MonoBehaviour
{
    
    private readonly float duration = 0.5f;
    protected RectTransform rectTransform;
    public bool returnToPrevScreen;
    public Action screenActivated;
    public Action screenDeactivated;

    public bool IsViewActive => gameObject.activeSelf;
    public Vector2 ScreenSize => rectTransform.rect.size;

    public virtual void InitializeView()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public virtual void ShowScreen(bool _animated)
    {
        if (rectTransform)
        {
            if (_animated)
            {
                rectTransform.DOLocalMoveX(0, 0.5f);
            }
            else
            {
                rectTransform.position = new Vector3(0, rectTransform.position.y, 0);
            }
        }

        gameObject.SetActive(true);
    }

    public virtual void ShowScreen()
    {
        gameObject.SetActive(true);
    }

    public virtual void HideScreen()
    {
        gameObject.SetActive(false);
    }

    public virtual void HideScreen(int _direction, int _count = 1)
    {
        rectTransform.DOLocalMoveX(ScreenSize.x * _direction, duration).onComplete = () =>
        {
            ChangePosition(new Vector3(ScreenSize.x * _direction * math.max(1, _count), 0, 0));
            gameObject.SetActive(false);
        };
    }

    public void ChangePosition(Vector3 _newPosition)
    {
        rectTransform.localPosition = _newPosition;
    }

    protected virtual void OnEnable()
    {
        screenActivated?.Invoke();
    }

    protected void OnDisable()
    {
        screenDeactivated?.Invoke();
    }
}