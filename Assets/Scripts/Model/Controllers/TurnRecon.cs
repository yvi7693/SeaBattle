using System;

public class TurnRecon
{
    private Sea sea1;
    private Sea sea2;
    private Sea queue;

    public TurnRecon(Sea sea1, Sea sea2, Sea queue)
    {
        this.sea1 = sea1;
        this.sea2 = sea2;

        this.queue = queue;
    }

    public Sea ReconOrder()
    {
        if (queue == sea1)
        {
            queue = sea2;
            return sea1;
        }

        else
        {
            queue = sea1;
            return sea2;
        }
    }

    public void KeepTurn()
    {
        this.ReconOrder();
    }
}