namespace Server.Data.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public UserDto Sender { get; set; }
        public string Content { get; set; }
    }
}
