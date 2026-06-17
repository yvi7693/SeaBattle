using System;

public class BattleController
{
    private Fleet fleet1;
    private Fleet fleet2;
    private Fleet winner;

    public BattleController()
    {
        this.fleet1 = new Fleet();
        this.fleet2 = new Fleet();
    }

    public bool IsDeclareWinner()
    {
        if (!(fleet1.HasSurvivors()))
            {
                winner = fleet2;
                return true;
            }

        else if (!(fleet2.HasSurvivors()))
        {
            winner = fleet1;
            return true;
        }
            

        return false;

    }
}