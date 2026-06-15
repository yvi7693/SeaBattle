using System;

public class BattleController
{
    private Fleet fleet1;
    private Fleet fleet2;
    private Fleet winner;

    public BattleController(Fleet fleet1, Fleet fleet2)
    {
        this.fleet1 = fleet1;
        this.fleet2 = fleet2;
    }
}