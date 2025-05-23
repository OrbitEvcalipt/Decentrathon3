using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace FunnyBlox.GUI
{
  [RequireComponent(typeof(CanvasGroup))]
  public class GUICanvasGroup : MonoBehaviour
  {
    private CanvasGroup canvasGroup;

    [SerializeField] private bool gameOnPause;

    [SerializeField] private bool hasCloseButton;

    [ShowIf("hasCloseButton")] [SerializeField]
    private Button closeButton;

    public Action hideWindowCloseButtonAction;

    [SerializeField] private Ease ease = Ease.Linear;

    [SerializeField] internal float fadeTime = 0f;
    [SerializeField] internal float showDelay = 0f;
    [SerializeField] private float hideDelay = 0f;
    [SerializeField] private bool fullInteractable = true;

    [SerializeField] internal bool closeByPhysicalBack = false;
    internal bool currentScreenIsActive = false;

    void Start()
    {
      Initialize();

      if (hasCloseButton)
      {
        closeButton.onClick.AddListener(() =>
        {
          hideWindowCloseButtonAction?.Invoke();
          Hide();
        });
      }
    }

    private void Initialize()
    {
      canvasGroup = GetComponent<CanvasGroup>();
    }

    public virtual void Show(Action onCompleted = null)
    {
      if (!canvasGroup) Initialize();
      canvasGroup.DOFade(1f, fadeTime)
        .SetEase(ease)
        .OnComplete(() =>
        {
          if (fullInteractable)
          {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
          }

          currentScreenIsActive = true;
          onCompleted?.Invoke();
        })
        .SetUpdate(true)
        .SetDelay(showDelay);

      if (gameOnPause)
        Time.timeScale = 0f;
    }

    public virtual void Show()
    {
      if (!canvasGroup) Initialize();
      canvasGroup.DOFade(1f, fadeTime)
        .SetEase(ease)
        .OnComplete(() =>
        {
          if (fullInteractable)
          {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
          }

          currentScreenIsActive = true;
        })
        .SetUpdate(true)
        .SetDelay(showDelay);

      if (gameOnPause)
        Time.timeScale = 0f;
    }

    public virtual void Hide(Action onCompleted = null)
    {
      if (gameOnPause)
        Time.timeScale = 1f;

      if (!canvasGroup) Initialize();
      canvasGroup.blocksRaycasts = false;
      canvasGroup.interactable = false;
      currentScreenIsActive = false;
      canvasGroup.DOFade(0f, fadeTime).SetEase(ease)
        .OnComplete(() =>
        {
          hideWindowCloseButtonAction?.Invoke();
          onCompleted?.Invoke();
        })
        .SetUpdate(true)
        .SetDelay(hideDelay);
    }

    public virtual void Hide()
    {
      if (gameOnPause)
        Time.timeScale = 1f;

      if (!canvasGroup) Initialize();
      currentScreenIsActive = false;
      canvasGroup.blocksRaycasts = false;
      canvasGroup.interactable = false;
      canvasGroup.DOFade(0f, fadeTime)
        .SetEase(ease)
        .SetDelay(hideDelay)
        .SetUpdate(true);
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Escape) && currentScreenIsActive && closeByPhysicalBack)
      {
        currentScreenIsActive = false;
        hideWindowCloseButtonAction?.Invoke();
        Hide();
      }
    }
  }
}