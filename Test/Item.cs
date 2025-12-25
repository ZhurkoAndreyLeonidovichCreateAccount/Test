namespace Test;

internal sealed class Item
{
    public int Weight { get; } 
    public string Name { get; }

    public Item(string name, int weight)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException("name can not be null or empty");
        }

        if (weight < 0) 
        {
            throw new ArgumentOutOfRangeException("weight can not be less than 0");
        }

        Weight = weight;
        Name = name;
    }
}
