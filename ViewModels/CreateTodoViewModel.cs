using Flunt.Notifications;
using Flunt.Validations;

namespace MinimalTodo.ViewModels
{
    public class CreateTodoViewModel : Notifiable<Notification>
    {
        public required string Title { get; set; }

        public Todo MapTo()
        {
            AddNotifications(new Contract<Notification>()
                .Requires()
                .IsNotNull(Title, "O titulo é obrigatório")
                .IsGreaterThan(Title, 5, "O titulo deve conter mais de 5 caracteres"));

            return new Todo(Guid.NewGuid(), Title, false);
        }
    }
}
