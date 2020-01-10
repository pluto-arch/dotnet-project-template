using System;
using System.Collections.Generic;
using System.Linq;
using Pluto.netcoreTemplate.Domain.Events.UserEvents;

namespace Pluto.netcoreTemplate.Domain.Entities.UserAggregate
{
    public class User: BaseEntity<int>
    {
        private  string _userName;
        private  string _tel;
        private int _status;

        public string UserName
        {
            get => _userName;
        }
        public string Tel {
            get => _tel;
        }

        public int Status
        {
            get => _status;
            private set => _status = value;
        }


        public DateTime CreateTime { get; } = DateTime.Now;
        public DateTime EditTime { get; } = DateTime.Now;

        /// <summary>
        /// only for efcore 
        /// </summary>
        protected User()
        {}

        public User(string userName,string tel,int status)
        {
            _userName = userName;
            _tel = tel;
            _status = status;
            _userBookItems = new List<UserBook>();
        }


        private readonly List<UserBook> _userBookItems;
        public IReadOnlyCollection<UserBook> UserBookItems => _userBookItems;

        public void AddUserBook(int bookId,string bookName,decimal bookPrice)
        {
            var book= _userBookItems.Where(o => o.Id == bookId||o.BookName==bookName)
                .SingleOrDefault();
            if (book!=null)
            {
                book.Price = bookPrice;
            }
            else
            {
                var bookitem = new UserBook("哈利波特",3.22M);
                _userBookItems.Add(bookitem);
            }
        }

        public void EnableUser()
        {
            if (Status==(int)EnumUserStatus.Enable)
            {
                throw new System.Exception("已启用");
            }
            Status = (int) EnumUserStatus.Enable;
            AddDomainEvent(new EnableUserEvent("您的账户以启用"));
        }

        public void DisableUser()
        {
            if (Status == (int)EnumUserStatus.Disable)
            {
                throw new System.Exception("已禁用");
            }
            Status = (int)EnumUserStatus.Enable;
            AddDomainEvent(new DisableUserEvent( "您的账户被禁用"));
        }
    }
}