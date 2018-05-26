namespace Server.Data.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Nickname { get; set; }
        public string AccountImageUrl { get; set; }
        public string Phone { get; set; }
        public string IPAddress { get; set; }
    }
}
