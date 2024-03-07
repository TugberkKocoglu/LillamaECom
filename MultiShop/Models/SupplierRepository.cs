using Microsoft.EntityFrameworkCore;

namespace MultiShop.Models
{
    public class SupplierRepository
    {
        ShopConnection context= new ShopConnection();
        public async Task<List<Supplier>> SupplierSelect()
        {
            List<Supplier> suppliers = await context.Suppliers.ToListAsync();
            return suppliers;
        }
        public static bool SupplierInsert(Supplier supplier)
        {
            try
            {
                using(ShopConnection context = new ShopConnection())
                {
                    supplier.BrandName.ToUpper();
                    context.Add(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Supplier SupplierDetails(int id)
        {
            Supplier supplier = context.Suppliers.Find(id);
            return supplier;
        }

        public static bool SupplierUpdate(Supplier supplier)
        {
            try
            {
                using (ShopConnection context = new ShopConnection())
                {
                    context.Update(supplier);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }

        }
        public static bool SupplierDelete(int id)
        {
            try
            {
                using (ShopConnection context=new ShopConnection())
                {
                    Supplier supplier=context.Suppliers.FirstOrDefault(s=>s.SupplierID==id);
                    supplier.Active = false;

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }


    }
}
