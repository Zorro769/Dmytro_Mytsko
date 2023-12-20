using System;
using System.Collections.Generic;

public interface IMapComponent
{
    void Draw(int x, int y);
    IMapComponent FindChild(string name);
}

public abstract class MapComponent : IMapComponent
{
    protected int _x;
    protected int _y;

    public MapComponent(int x, int y)
    {
        _x = x;
        _y = y;
    }

    public abstract void Draw(int x, int y);

    public abstract IMapComponent FindChild(string name);
}

public class CompositeMapComponent : MapComponent
{
    private List<IMapComponent> _components;

    public CompositeMapComponent(int x, int y) : base(x, y)
    {
        _components = new List<IMapComponent>();
    }

    public override void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing CompositeMapComponent at ({_x + x}, {_y + y})");
        foreach (var component in _components)
        {
            component.Draw(_x + x, _y + y);
        }
    }

    public override IMapComponent FindChild(string name)
    {
        foreach (var component in _components)
        {
            var foundComponent = component.FindChild(name);
            if (foundComponent != null)
                return foundComponent;
        }
        return null;
    }

    public void AddComponent(IMapComponent component)
    {
        _components.Add(component);
    }
}
public class SimpleMapComponent : MapComponent
{
    private string _name;

    public SimpleMapComponent(string name, int x, int y) : base(x, y)
    {
        _name = name;
    }

    public override void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing SimpleMapComponent {_name} at ({_x + x}, {_y + y})");
    }

    public override IMapComponent FindChild(string name)
    {
        return _name == name ? this : null;
    }
}

class Program
{
    static void Main()
    {
        var map = new CompositeMapComponent(0, 0);

        var city1 = new CompositeMapComponent(10, 10);
        city1.AddComponent(new SimpleMapComponent("Supermarket", 1, 1));
        city1.AddComponent(new SimpleMapComponent("Market", 3, 3));

        var city2 = new CompositeMapComponent(20, 20);
        city2.AddComponent(new SimpleMapComponent("Bank", 2, 2));

        map.AddComponent(city1);
        map.AddComponent(city2);

        map.Draw(0, 0);

        var foundComponent = map.FindChild("Market");
        if (foundComponent != null)
        {
            Console.WriteLine("Found: " + foundComponent.GetType().Name);
        }
        else
        {
            Console.WriteLine("Not found");
        }
    }
}
