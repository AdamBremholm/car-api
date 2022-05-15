using CarConsole.Models;
using FluentAssertions;
using NUnit.Framework;

namespace CarConsoleTests;

public class Tests
{

    [Test]
    public void Test1()
    {
        var car = new Car { Name = "volvo" };
        car.Should().NotBeNull();
        car.Name.Should().Be("volvo");
    }

    [Test]
    public void Test2()
    {
        var car = new Car { };
        car.Should().NotBeNull();
        car.Name.Should().Be("Default");
    }

    [Test]
    public void Test3()
    {
        var car = new Car { };
        car.Should().NotBeNull();
        car.Name.Should().Be("Default");
    }
    [Test]
    public void ShouldBe_CalledVolvo()
    {
        //Given
        var car = new Car {Name = "volvo"};
        //When

        //Then
        car.Name.Should().Be("volvo");
    }

    [Test]
    public void TestName()
    {
    //Given
    var a = new Car{Name = "mercedes"};

    //When

    //Then
    }
}
