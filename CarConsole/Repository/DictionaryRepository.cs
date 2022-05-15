using CarConsole.Models;

public class DictionaryRepository : IRepository
{
    private readonly IDictionary<Guid, Car> _cars;

    public DictionaryRepository(IDictionary<Guid, Car> cars)
    {
        _cars = cars;
    }

    public void Create(Car car)
    {
        if (car is null)
        {
            return;
        }
        _cars[car.Id] = car;
    }

    public void Delete(Guid id)
    {
        _cars.Remove(id);
    }

    public Car GetById(Guid id)
    {
        return _cars[id];
    }

    public void Update(Car car)
    {
        var existingCar = GetById(car.Id);
        if (existingCar is null)
        {
            return;
        }
        _cars[car.Id] = car;
    }

    public List<Car> GetAll()
    {
        return _cars.Values.ToList();
    }
}
