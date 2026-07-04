using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNumber;

    public void ShowWinner(int numberPlayer)
    {
        winnerNumber.text = $"Player {numberPlayer}";
    }

    private void Start()
    {
        BattleController battleController = GameSession.Instance.staff.GetBattleController();
        int numberWinner = battleController.GetWinnerNumber();

        ShowWinner(numberWinner);
    }
}