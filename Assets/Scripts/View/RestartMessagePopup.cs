using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RestartMessagePopup : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string message = "Хватит вам";

    private GameObject popup;

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowMessage();
    }

    public void ShowMessage()
    {
        if (popup != null)
        {
            popup.SetActive(true);
            popup.transform.SetAsLastSibling();
            return;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
            return;

        popup = new GameObject("RestartMessagePopup", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image), typeof(Button));
        popup.transform.SetParent(canvas.transform, false);

        RectTransform popupRect = popup.GetComponent<RectTransform>();
        popupRect.anchorMin = Vector2.zero;
        popupRect.anchorMax = Vector2.one;
        popupRect.offsetMin = Vector2.zero;
        popupRect.offsetMax = Vector2.zero;

        Image overlay = popup.GetComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.55f);

        Button overlayButton = popup.GetComponent<Button>();
        overlayButton.transition = Selectable.Transition.None;
        overlayButton.onClick.AddListener(HideMessage);

        GameObject panel = new GameObject("MessagePanel", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        panel.transform.SetParent(popup.transform, false);

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;
        panelRect.sizeDelta = new Vector2(520f, 180f);

        Image panelImage = panel.GetComponent<Image>();
        panelImage.color = new Color(0.95f, 0.95f, 0.95f, 1f);

        GameObject textObject = new GameObject("MessageText", typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
        textObject.transform.SetParent(panel.transform, false);

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(24f, 20f);
        textRect.offsetMax = new Vector2(-24f, -20f);

        TextMeshProUGUI text = textObject.GetComponent<TextMeshProUGUI>();
        text.text = message;
        text.color = new Color(0.12f, 0.12f, 0.12f, 1f);
        text.fontSize = 44f;
        text.fontStyle = FontStyles.Bold;
        text.alignment = TextAlignmentOptions.Center;
        text.enableWordWrapping = true;
    }

    private void HideMessage()
    {
        popup.SetActive(false);
    }
}
