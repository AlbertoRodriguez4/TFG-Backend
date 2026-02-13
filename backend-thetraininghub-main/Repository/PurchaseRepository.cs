using AA2_CS.Database;
using AA2_CS.Model;
using Microsoft.EntityFrameworkCore;
// Asegúrate de importar donde tengas tu PurchaseDTO
// using AA2_CS.DTOs; 

namespace AA2_CS.Repository
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        private readonly AppDbContext _context;

        public PurchaseRepository(AppDbContext context)
        {
            _context = context;
        }

        // ... Add, Delete, FindAll, FindById, Update SE QUEDAN IGUAL ...
        public int Add(Purchase purchase)
        {
            var user = _context.Users.FirstOrDefault(u => u.id == purchase.userid);
            var item = _context.Items.FirstOrDefault(i => i.id == purchase.itemid);
            purchase.purchasedate = purchase.purchasedate.ToUniversalTime();

            if (user == null || item == null)
                throw new ArgumentException("User or Item not found.");

            // NOTA: Idealmente esta lógica de oro debería ir en el Service (como la XP),
            // pero está bien dejarla aquí por ahora para no romper nada más.
            if (user.gold < item.price)
                throw new InvalidOperationException("El usuario no tiene suficiente oro.");

            user.gold -= item.price;

            _context.Purchases.Add(purchase);
            _context.SaveChanges();

            return purchase.id;
        }

        public int Delete(Purchase purchase)
        {
            var record = _context.Purchases.FirstOrDefault(p => p.id == purchase.id);
            if (record != null)
            {
                _context.Purchases.Remove(record);
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }

        public List<Purchase> FindAll()
        {
            return _context.Purchases.ToList();
        }

        public Purchase FindById(int id)
        {
            return _context.Purchases.FirstOrDefault(p => p.id == id);
        }

        public List<Purchase> FindByCharacteristic(string name)
        {
            throw new NotImplementedException("Search by characteristic not implemented.");
        }

        public int Update(Purchase purchase)
        {
            var existing = _context.Purchases.FirstOrDefault(p => p.id == purchase.id);
            if (existing != null)
            {
                existing.userid = purchase.userid;
                existing.itemid = purchase.itemid;
                existing.purchasedate = purchase.purchasedate;
                _context.SaveChanges();
                return 1;
            }
            return 0;
        }
        public List<PurchaseDTO> FindByUserId(int userId)
        {

            var purchases = (from p in _context.Purchases
                             join item in _context.Items on p.itemid equals item.id
                             join user in _context.Users on p.userid equals user.id
                             where p.userid == userId
                             select new PurchaseDTO
                             {
                                 PurchaseId = p.id,
                                 UserId = p.userid,
                                 UserName = user.name,
                                 UserEmail = user.email,
                                 ItemId = item.id,
                                 ItemName = item.name,
                                 ItemType = item.type,
                                 ItemBonus = item.bonus,
                                 ItemPrice = item.price,
                                 ImageUrl = item.imageUrl,
                                 PurchaseDate = p.purchasedate

                             }).ToList();

            return purchases;
        }
    }
}