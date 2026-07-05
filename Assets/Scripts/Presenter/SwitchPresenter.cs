using UnityEngine;

public class SwitchPresenter : MonoBehaviour
{
    [SerializeField] private BattlePresenter battlePresenter;
    [SerializeField] private AiBattlePresenter aiBattlePresenter;

    private void Awake()
    {
        if (GameSession.Instance.GetMode() == Mode.Classic)
        {
            battlePresenter.enabled = true;
            aiBattlePresenter.enabled = false;
        }
        
        else if (GameSession.Instance.GetMode() == Mode.Ai)
        {
            aiBattlePresenter.enabled = true;
            battlePresenter.enabled = false;
        } 
    }
}