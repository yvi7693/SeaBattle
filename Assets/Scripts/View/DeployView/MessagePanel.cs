using UnityEngine;

public class MessagePanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    public void PanelOn()
    {
        panel.SetActive(true);
    }

    public void PanelOff()
    {
        panel.SetActive(false);
    }
}