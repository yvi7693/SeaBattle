using System;

public class TurnRecon
{
    private Sea sea1;
    private Sea sea2;
    private Sea queue;

    public TurnRecon(Fleet fleet1, Fleet fleet2)
    {
        this.sea1 = new Sea(fleet1);
        this.sea2 = new Sea(fleet2);

        this.queue = sea1;
    }

    public Sea GetQueue()
    {
        return queue;
    }

    public Sea GetSea1()
    {
        return sea1;
    }

    public Sea GetSea2()
    {
        return sea2;
    }

    public Sea GetSeaDeploy()
    {
        if (sea1.IsShipsDeploy() && sea2.IsShipsDeploy())
            throw new Exception("All ships are stationed");

        if (sea1.IsShipsDeploy())
            return sea2;

        return sea1;
        
    }

    public void SwitchQueue()
    {
        if (queue == sea1)
        {
            queue = sea2;
        }

        else
        {
            queue = sea1;
        }
    }
}