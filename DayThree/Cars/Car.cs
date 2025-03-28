namespace DayThree.Cars;

public abstract class Car
{
    public required int Id { get; set; }
    public string? Make { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public DateTime LastMaintenanceDate { get; set; }
}