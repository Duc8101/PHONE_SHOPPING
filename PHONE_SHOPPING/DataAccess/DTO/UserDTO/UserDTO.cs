namespace DataAccess.DTO.UserDTO
{
    public class UserDTO : ProfileDTO
    {
        public Guid UserId { get; set; }
        public string RoleName { get; set; } = null!;
    }
}
