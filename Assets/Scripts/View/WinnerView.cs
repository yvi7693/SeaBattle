
using TMPro;
using UnityEngine;

public class WinnerView : MonoBehaviour
{
    [SerializeField] private TMP_Text winnerNumber;


    private void Start()
    {
        BattleController battleController = GameSession.Instance.staff.GetBattleController();
        PlayerName winner = battleController.GetWinner();

        ShowWinner(winner);
    }


    public void ShowWinner(PlayerName Player)
    {
        winnerNumber.text = $"{Player}";
    }


}