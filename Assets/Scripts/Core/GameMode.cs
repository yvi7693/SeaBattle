using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    private bool showAiModePopup;
    private Rect aiModePopupRect;
    private GUIStyle aiModeWindowStyle;
    private GUIStyle aiModeMessageStyle;
    private GUIStyle aiModeButtonStyle;

    public void ChooseClassicMode()
    {
        SceneManager.LoadScene("DeployScene");
    }

    public void ChooseAiMode()
    {
        aiModePopupRect = new Rect(Screen.width / 2f - 300f, Screen.height * 0.31f, 600f, 250f);
        showAiModePopup = true;
    }

    // Временный код

    private void OnGUI()
    {
        if (!showAiModePopup)
        {
            return;
        }

        if (aiModeWindowStyle == null)
        {
            aiModeWindowStyle = new GUIStyle(GUI.skin.window)
            {
                fontSize = 24
            };
        }

        aiModePopupRect = GUI.ModalWindow(0, aiModePopupRect, DrawAiModePopup, "Attention", aiModeWindowStyle);
    }

    private void DrawAiModePopup(int windowId)
    {
        if (aiModeMessageStyle == null)
        {
            aiModeMessageStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 28,
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
        }

        if (aiModeButtonStyle == null)
        {
            aiModeButtonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = 24
            };
        }

        GUILayout.Space(40f);
        GUILayout.Label("Режим находится в разработке.", aiModeMessageStyle, GUILayout.Height(70f));
        GUILayout.Space(25f);

        if (GUILayout.Button("OK", aiModeButtonStyle, GUILayout.Height(50f)))
        {
            showAiModePopup = false;
        }
    }
}
