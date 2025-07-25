using AA2_CS.Model;
using AA2_CS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AA2_CS.Service
{
    public class ItemService : IService<Item>
    {
        private readonly ItemRepository _itemRepository;

        public ItemService(ItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        public int Add(Item entity)
        {
            return _itemRepository.Add(entity);
        }

        public int Delete(Item entity)
        {
            return _itemRepository.Delete(entity);
        }

        public List<Item> FindAll()
        {
            return _itemRepository.FindAll();
        }

        public List<Item> FindByCharacteristic(string name)
        {
            return _itemRepository.FindByCharacteristic(name);
        }

        public Item FindById(int id)
        {
            return _itemRepository.FindById(id);
        }

        public int Update(Item entity)
        {
            return _itemRepository.Update(entity);
        }

        public List<Item> GetRandomStrengthItems(int count = 2)
        {
            return _itemRepository.GetRandomStrengthItems(count);
        }

        public List<Item> GetRandomEnduranceItems(int count = 2)
        {
            return _itemRepository.GetRandomEnduranceItems(count);
        }

        public List<Item> GetRandomItems(int count = 4)
        {
            return _itemRepository.GetRandomItems(count);
        }

        public async Task<ActionResult<Item>> UpdateById(int id, Item updatedItem)
        {
            return await _itemRepository.UpdateByIdAsync(id, updatedItem);
        }

    }
}