// Em Vendas.WPF/Models/Notification.cs
namespace FluxoSistema.Core.Models
{
    public enum NotificationType
    {
        Success,
        Warning,
        Error
    }

    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public NotificationType Type { get; set; }
    }
}