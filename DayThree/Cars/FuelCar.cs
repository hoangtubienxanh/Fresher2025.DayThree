namespace DayThree.Cars;

public class FuelCar : Car, IFuelable
{
    public DateTime FueledAt { get; private set; }

    public void Refuel(DateTime timeOfRefuel)
    {
        throw new NotImplementedException();
    }
}