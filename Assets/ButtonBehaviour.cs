using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private bool isToggle;
    [SerializeField] private Animator animator;
    
    public UnityEvent OnPress;
    
    private static readonly int Play = Animator.StringToHash("Play");
    private bool _toggleState = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnPress?.Invoke();

        if (!isToggle)
        {
            animator.SetTrigger(Play);
        }
        else
        {
            _toggleState = !_toggleState;
            animator.SetTrigger(_toggleState ? "On" : "Off");
        }
    }
}
