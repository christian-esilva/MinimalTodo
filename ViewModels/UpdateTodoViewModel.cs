using Flunt.Notifications;
using Flunt.Validations;

namespace MinimalTodo.ViewModels
{
    public class UpdateTodoViewModel : Notifiable<Notification>
    {
        public required string Title { get; set; }
        public bool Done { get; set; }

        public Todo MapTo(Guid id)
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Title, "O titulo é obrigatório")
                .IsGreaterThan(Title, 5, "O titulo deve conter mais de 5 caracteres"));

            return new Todo(id, Title, Done);
        }
    }
}
