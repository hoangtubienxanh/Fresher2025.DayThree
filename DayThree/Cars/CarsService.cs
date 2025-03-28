namespace DayThree.Cars;

public class CarsService() : ICarsService
{
    private readonly List<Car> _backingStore = [];
    private int _sequentialOrder = 0;

    public Car? Add(Car itemToBeAdded)
    {
        if (_backingStore.FirstOrDefault(x => Equals(x.Id, itemToBeAdded.Id)) is not null)
        {
            throw new ArgumentException("Car with the same id already exists.");
        }

        if (itemToBeAdded.Id == 0)
        {
            itemToBeAdded.Id = _sequentialOrder++;
        }

        _backingStore.Add(itemToBeAdded);
        _sequentialOrder = _backingStore.Max(x => x.Id);

        return itemToBeAdded;
    }

    public Car? Edit(Car itemToBeEdited)
    {
        var existingItem = _backingStore.FirstOrDefault(x => Equals(x.Id, itemToBeEdited.Id));
        if (existingItem is null)
        {
            throw new ArgumentException("Car with the same id wasn't founds.");
        }

        _backingStore.Remove(existingItem);
        _backingStore.Add(itemToBeEdited);
        return itemToBeEdited;
    }

    public int RemoveAll(Predicate<Car> predicate)
    {
        return _backingStore.RemoveAll(predicate);
    }

    public IReadOnlyList<Car> GetAll(Predicate<Car> predicate)
    {
        return _backingStore.Where(x => predicate(x)).ToList();
    }
}