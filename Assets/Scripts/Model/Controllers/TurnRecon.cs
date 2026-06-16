using System;

public class TurnRecon
{
    private Sea sea1;
    private Sea sea2;
    private Sea queue;

    public TurnRecon()
    {
        this.sea1 = new Sea();
        this.sea2 = new Sea();

        this.queue = sea1;
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