namespace DayThree.Cars;

public interface ICarsService
{
    Car? Add(Car itemToBeAdded);
    Car? Edit(Car itemToBeEdited);
    int RemoveAll(Predicate<Car> predicate);
    IReadOnlyList<Car> GetAll(Predicate<Car> predicate);
}