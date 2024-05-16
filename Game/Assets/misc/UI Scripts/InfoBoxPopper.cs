using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InfoBoxPopper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Item optionalInfo;

    [SerializeField] public bool popped;

    public void OnPointerEnter(PointerEventData pointerEventData) {
        InfoBox.ib.UpdateInfo(optionalInfo);
        Vector2 mousePos = Mouse.current.position.ReadValue();
        // Debug.Log(mousePos);
        Vector3 camdis = new Vector3(mousePos.x, mousePos.y, 13f);
        Vector3 pos = Camera.main.ScreenToWorldPoint(camdis);
        InfoBox.ib.PopBox(pos);
        popped = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        InfoBox.ib.UnpopBox();
        popped = false;
    }
}
