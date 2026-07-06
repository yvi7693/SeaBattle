
using TMPro;
using UnityEngine;

public class WinnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNumber;


    private void Start()
    {
        BattleController battleController = GameSession.Instance.staff.GetBattleController();
        PlayerName winner = battleController.GetWinner();

        if (GameSession.Instance.GetMode() == Mode.Ai && winner == PlayerName.Player2)
            winner = PlayerName.Ai;

        ShowWinner(winner);
    }


    public void ShowWinner(PlayerName Player)
    {
        winnerNumber.text = $"{Player}";
    }


}