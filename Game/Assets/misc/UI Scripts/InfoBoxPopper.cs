using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InfoBoxPopper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Item optionalInfo;
    public State state = new State(ClassItem.Empty);

    [HideInInspector] public bool popped;

    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (!enabled) return;
        state.Invoke(true);
        InfoBox.ib.UpdateInfo(optionalInfo);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        // Debug.Log(mousePos);
        Vector3 camdis = new Vector3(mousePos.x, mousePos.y, 13f);
        Vector3 pos = Camera.main.ScreenToWorldPoint(camdis);
        InfoBox.ib.PopBox(pos);
        popped = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!enabled && !popped) return;
        state.Invoke(false);
        InfoBox.ib.UnpopBox();
        popped = false;
    }
}
