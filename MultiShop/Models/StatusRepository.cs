using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;

namespace MultiShop.Models
{
    public class StatusRepository
    {
        ShopConnection context=new ShopConnection();
        public async Task<List<Status>> StatusSelect()
        {
            List<Status> statuses = await context.Statuses.ToListAsync();
            return statuses;
        }

        public static bool  StatusInsert(Status status)
        {
            try
            {
                using (ShopConnection context=new ShopConnection())
                {
                    status.StatusName.ToUpper();
                    context.Add(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<Status> StatusDetails(int? id)
        {
            Status? status = await context.Statuses.FirstOrDefaultAsync(s => s.StatusID == id);
            return status;
        }

        public static bool StatusUpdate(Status status)
        {
            try
            {
                using (ShopConnection context=new ShopConnection())
                {
                    context.Update(status);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static bool StatusDelete(int id)
        {
            try
            {
                using (ShopConnection context = new ShopConnection())
                {
                    Status status=context.Statuses.FirstOrDefault(s => s.StatusID == id);
                    status.Active = false;

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
