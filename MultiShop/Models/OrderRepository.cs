namespace MultiShop.Models
{
    public class OrderRepository
    {
        public int ProductID { get; set; }

        public int Quantity { get; set; }

        public string? MyCart { get; set; }

        public decimal UnitPrice { get; set; }

        public string? ProductName { get; set; }

        public int Kdv { get; set; }

        public string? PhotoPath { get; set; }

        public string? tckimlik_vergi_no { get; set; }

        ShopConnection context=new ShopConnection();
        public bool AddToMyCart(string id)
        {
            bool exists=false;
            if (MyCart=="")
            {
                //Sepete ilk defe ürün eklencek
                MyCart = id + "=1"; //1 adet id li ürün
            }
            else
            {
                string[] MyCartArray = MyCart.Split('&');//ürünleri ayırdım
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    if (MyCartArrayLoop[0]==id)
                    {
                        exists = true; //ürün sepette var 
                    }
                }
                if (exists==false)
                {
                    MyCart = MyCart + "&" + id.ToString() + "=1"; 
                }

            }
            return exists;
        }

        public void DeleteFromMyCart(string id)
        {
            string[] MyCartArray = MyCart.Split('&');
            string NewMyCart = "";
            int count = 1; //birdern fazla ürün için & işareti kullancağımdan lazım

            for (int i = 0; i < MyCartArray.Length; i++)
            {
                string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                string MyCartID = MyCartArrayLoop[0];
                if (MyCartID!=id)
                {
                    //silinmeyecek olanları yeni sepete atıcam
                    if (count==1)
                    {
                        //sepette 1 ürün varsa & işareti olmaz
                        NewMyCart = MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                        count++;
                    }
                    else
                    {
                        //birden fazla ürün var
                        NewMyCart += "&" + MyCartArrayLoop[0] + "=" + MyCartArrayLoop[1];
                    }
                }
            }
            MyCart = NewMyCart; // sepeti de yeni sepet yaptım
        }

        public List<OrderRepository> SelectMyCart()
        {
            List<OrderRepository> list = new List<OrderRepository>();
            string[] MyCartArray = MyCart.Split('&');

            if (MyCartArray[0]!="")
            {
                for (int i = 0; i < MyCartArray.Length; i++)
                {
                    string[] MyCartArrayLoop = MyCartArray[i].Split('=');
                    int MyCartID = Convert.ToInt32(MyCartArrayLoop[0]);

                    Product? product = context.Products.FirstOrDefault(p => p.ProductID == MyCartID);
                    
                    OrderRepository orderRepository = new OrderRepository();
                    orderRepository.ProductID = product.ProductID;
                    orderRepository.Quantity = Convert.ToInt32(MyCartArrayLoop[1]);
                    orderRepository.UnitPrice = product.UnitPrice;
                    orderRepository.ProductName= product.ProductName;
                    orderRepository.PhotoPath  = product.PhotoPath;
                    orderRepository.Kdv=product.Kdv;
                    list.Add(orderRepository);
                }
            }
            return list;
        }

        public void EfaturaCerate()
        {
            //digital planet xml dosyası gelecek
        }

        public string OrderCreate(string Email)
        {
            List<OrderRepository> sipList = SelectMyCart();
            string OrderGroupGUID = DateTime.Now.ToString().Replace(":", "").Replace(" ", "").Replace(".", "");
            DateTime OrderDate=DateTime.Now;

            foreach (var item in sipList)
            {
                Order order=new Order();
                order.OrderDate = OrderDate;
                order.OrderGroupGUID = OrderGroupGUID;
                order.UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
                order.ProductID=item.ProductID;
                order.Quantity = item.Quantity;
                context.Orders.Add(order);
                context.SaveChanges();
            }
            return OrderGroupGUID;
        }
        public List<vw_MyOrders> SelectMyOrders(string Email)
        {
            int UserID = context.Users.FirstOrDefault(u => u.Email == Email).UserID;
            List<vw_MyOrders> myOrders= context.vw_MyOrders.Where(o=>o.UserID == UserID).ToList();
            return myOrders;
        }
    }
}
