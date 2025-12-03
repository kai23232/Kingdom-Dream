using UnityEngine;
using UnityEngine.Events;

public class BaseEventListener<T> : MonoBehaviour
{
    public BaseEventSO<T> eventSO;
    public UnityEvent<T> response;
    
    private void OnEnable()
    {
        eventSO.onEventRaised += OnEventRaised;
    }
    private void OnDisable()
    {
        eventSO.onEventRaised -= OnEventRaised;
    }
    private void OnEventRaised(T value)
    {
        response.Invoke(value);
    }
}
