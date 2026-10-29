namespace GestionOrdinateurs.API;

public class StockService
{

    private readonly object _verrou = new();

    public int StockSSD { get; set; } = 10;

    public void ConsommerSSD()
    {

        lock (_verrou)
        {
            Task.Delay(10).Wait();
            StockSSD--;
        }
    }
}