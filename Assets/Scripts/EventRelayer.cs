using HyperGames;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventRelayer : MonoBehaviour, IScrollHandler {

    void Update() {
        if (Input.GetKey(KeyCode.Plus)) {
            Messenger.Dispatch(new ScrollEvent() { direction = Vector2.up });
        }

        if (Input.GetKey(KeyCode.Alpha0)) {
            Messenger.Dispatch(new ScrollEvent() { direction = -Vector2.up });
        }
    }

    public void OnScroll(PointerEventData eventData) {
        Messenger.Dispatch(new ScrollEvent() {
            direction = eventData.scrollDelta.normalized
        });
    }

}
