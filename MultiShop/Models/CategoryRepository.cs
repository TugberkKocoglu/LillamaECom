using Microsoft.EntityFrameworkCore;

namespace MultiShop.Models
{
	public class CategoryRepository
    {
        ShopConnection context=new ShopConnection();
        public async Task<List<Category>> CategorySelect()
        {
            List<Category> categories = await context.Categories.ToListAsync();
            return categories;
        }

        public List<Category> CategorySelectMain() 
        {
            List<Category> categories = context.Categories.Where(c=>c.ParentID==0).ToList();
            return categories;
        }

        public static bool CategoryInsert(Category category)
        {
            try
            {
                using (ShopConnection context = new ShopConnection())
                {
                    context.Add(category);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Category CategoryDetails(int? id)
        {
            Category? categories = context.Categories.Find(id);
            return categories;
        }

        public static bool CategoryUpdate(Category category)
        {
            try
            {
                using (ShopConnection context = new ShopConnection())
                {
                    context.Update(category);
                    context.SaveChanges();
                    return true;
                }
                    
            }
            catch (Exception)
            {

                return false;
            }
           
        }
        public static bool  CategoryDelete(int id)
        {
            try
            {
                using (ShopConnection context=new ShopConnection())
                {
                    Category? category = context.Categories.FirstOrDefault(c => c.CategoryID == id);
                    category.Active = false;
                    List<Category> categoryList=context.Categories.Where(c=>c.ParentID==id).ToList();
                    foreach (var item in categoryList)
                    {
                        item.Active = false;
                    }
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
