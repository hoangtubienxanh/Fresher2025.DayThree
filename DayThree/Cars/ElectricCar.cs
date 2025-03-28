namespace DayThree.Cars;

public class ElectricCar : Car, IChargeable
{
    public DateTime ChargedAt { get; private set; }

    public void Charge(DateTime timeOfCharge)
    {
        ChargedAt = timeOfCharge;
    }
}