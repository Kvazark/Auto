namespace Auto.Website.Models
{
    public class ErrorModelView
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}