namespace Test;

internal class Inventory
{
    private readonly List<Item> _items = [];   
    private readonly Lock _lock = new ();
    private int _totalWeight;

    public int MaxWeight {get; }

    public Inventory(int maxWeight = 100) 
    {
        MaxWeight = maxWeight;
    }

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

    public int TotalWeight => _totalWeight;
    

    public void AddItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock) 
        {
            var newTotalWight = _totalWeight + item.Weight;

            if (newTotalWight > MaxWeight)
            {
                throw new InvalidOperationException("Inventory weight limit exceeded.");
            }

            var existingElement = _items.FirstOrDefault(x => x.Name.Equals(item.Name, StringComparison.OrdinalIgnoreCase));

            if (existingElement != null)
            {
                _items.Remove(existingElement);
                _items.Add(new Item(existingElement.Name, existingElement.Weight + item.Weight));              
            }
            else
            {
                _items.Add(item);
            }

            _totalWeight = newTotalWight;  
        }       
    }

    public bool RemoveItem(Item item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_lock)
        {
            var removeItem = _items.FirstOrDefault(x => x.Name == item.Name);

            if (removeItem == null) 
            {
                return false;
            }
           
            _totalWeight -= removeItem.Weight;
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
