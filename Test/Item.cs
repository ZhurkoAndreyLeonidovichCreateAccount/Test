namespace Test;

internal sealed class Item
{   
    public string Name { get; }
    public int Weight { get; }

    public Item(string name, int weight)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentNullException("Name can not be null or empty");
        }

        if (weight < 0) 
        {
            throw new ArgumentOutOfRangeException("Weight can not be less than 0");
        }

        Name = name;
        Weight = weight;       
    }
}
