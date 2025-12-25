namespace Test;

internal class Inventory
{
    private readonly List<Item> _items = [];
    private const int MaxWeight = 100;
    private readonly Lock _lock = new ();   

    public IReadOnlyCollection<Item> Items 
    {
        get 
        {
            lock (_lock) 
            {
                return _items.ToList().AsReadOnly();
            }
            
        }
    }

    public int TotalWeight
    {
        get 
        {
            lock (_lock) 
            {
                return _items.Sum(x => x.Weight);
            }
            
        }
    }
    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock) 
        {
            var newTotalWight = TotalWeight + item.Weight;

            if (newTotalWight > MaxWeight)
            {
                throw new InvalidOperationException("Inventory weight limit exceeded.");
            }

            var existingElement = _items.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));

            if (existingElement != null)
            {
                _items.Remove(existingElement);
                _items.Add(new Item(existingElement.Name, existingElement.Weight + item.Weight));
                return;
            }

            _items.Add(item);
        }
        
    }

    public bool RemoveItem(Item item)
    {
        var removeItem = _items.FirstOrDefault(x => x.Name == item.Name);
        if (removeItem == null) 
        {
            return false;
        }

        lock (_lock) 
        {
            return _items.Remove(removeItem);
        }
        
    }

    public IReadOnlyList<Item> FindByName(string substring)
    {
        if (string.IsNullOrEmpty(substring))
        {
            return Array.Empty<Item>();
        }

        lock (_lock) 
        {
            return _items.Where(x => x.Name.Contains(substring, StringComparison.OrdinalIgnoreCase))
                     .ToList()
                     .AsReadOnly();
        }       
    }
}
