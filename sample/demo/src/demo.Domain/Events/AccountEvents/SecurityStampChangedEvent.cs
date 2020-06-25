using MediatR;


namespace Demo.Domain.Events.AccountEvents
{
    public class SecurityStampChangedEvent<TKey> : INotification
    {
        public string Message { get; set; }

        public TKey UserId { get; set; }

        public SecurityStampChangedEvent(TKey id, string msg)
        {
            this.UserId = id;
            this.Message = msg;
        }
    }
}