using MediatR;

namespace Demo.Domain.Events.UserEvents
{
    public class DisableUserEvent : INotification
    {

        public string Message { get; set; }


        public DisableUserEvent(string msg)
        {
            Message = msg;
        }
    }
}