using Misc;
using UnityEngine;
using UnityEngine.EventSystems;

public class BackClickDetector : MonoSingleton<BackClickDetector>, IPointerClickHandler
{
    public event System.Action OnBackClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBackClick?.Invoke();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
