using FluentAssertions;
using Test;

namespace UnitTesting
{
    public class InventoryTest
    {
        [Fact]
        public void AddItem_WhenItemIsValid_AddsItemToInventory() 
        {
            //Arrange
            var inventory = new Inventory();
            var item = new Item("Sword", 10);

            //Act
            inventory.AddItem(new Item("Sword", 10));

            //Assert
            inventory.Items.Should().HaveCount(1);

            var addedItem = inventory.Items.FirstOrDefault();

            addedItem.Name.Should().Be("Sword");
            addedItem.Weight.Should().Be(10);
        }

        [Fact]
        public void AddItem_WhenItemWithSameNameExists_AddsItemToInventory() 
        {
            //Arrange
            var inventory = new Inventory();
            var item1 = new Item("Sword", 10);
            var item2 = new Item("Sword", 10);

            //Act
            inventory.AddItem(item1);
            inventory.AddItem(item2);

            //Assert
            inventory.Items.Should().HaveCount(1);
            var addedItem = inventory.Items.FirstOrDefault();
            addedItem.Weight.Should().Be(20);
        }

        [Fact]
        public void AddItem_WhenAddingItemExceedsMaxWeight_ThrowsInvalidOperationException() 
        {
            //Arrange
            var invertory = new Inventory();
            var item1 = new Item("Sword", 50);
            var item2 = new Item("Sword1", 60);

            //Act
            invertory.AddItem(item1);
            Action act = () => invertory.AddItem(item2);

            //Assert
            act.Should().Throw<InvalidOperationException>();

        }

        [Fact]
        public void RemoveItem_WhenItemExistsInInventory_RemovesItem()
        {
            //Arrange
            var inventory = new Inventory();
            var item1 = new Item("Sword", 50);
            inventory.AddItem(item1);

            //Act
          var result = inventory.RemoveItem(item1);

            //Assert
            inventory.Items.Should().BeEmpty();
            result.Should().BeTrue();
        }

        [Fact]
        public void FindByName_WhenItemExistsInInventory_FindsItem() 
        {
            //Arrange
            var inventory = new Inventory();
            var item1 = new Item("Sword", 50);
            inventory.AddItem(item1);

            //Act
           var result = inventory.FindByName(item1.Name);

            //Assert           
            result.Should().ContainSingle(result => result.Name == item1.Name);
        }

        [Fact]
        public async Task AddItem_WhenItemsAreAddedConcurrently_AddsAllItems()
        {
            //Arrage
            var inventory = new Inventory();
            var tasks = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
            {
                inventory.AddItem(new Item($"Sword{i}", 10));
            })).ToArray();

            await Task.WhenAll(tasks);

            //Act
            var items = inventory.Items;
            var totalWeight = inventory.TotalWeight;

            //Assert
            items.Should().HaveCount(10);
            totalWeight.Should().Be(100);
        }
    }
}
