using CarConsole.Models;

public interface IRepository
{
    public void Create(Car car);
    public Car GetById(Guid id);
    public List<Car> GetAll();
    public void Update(Car car);
    public void Delete(Guid id);
}
