namespace Pluto.netcoreTemplate.Domain.Entities.UserAggregate
{
    public class UserBook:BaseEntity<int>
    {
        private string _bookName;
        private decimal _price;



        public string BookName
        {
            get => _bookName;
            set => _bookName = value;
        }

        public decimal Price
        {
            get => _price;
            set => _price = value;
        }

        public User User { get; set; }


        protected UserBook() { }


        public UserBook(string name,decimal price)
        {
            BookName = name;
            Price = price;
        }

    }
}