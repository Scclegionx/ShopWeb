namespace ShopWeb.Models.Domain
{
    public class Response
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public string State { get; set; }
    }
}
