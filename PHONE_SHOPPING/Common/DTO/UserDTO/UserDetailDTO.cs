namespace Common.DTO.UserDTO
{
    public class UserDetailDTO : UserUpdateDTO
    {
        public Guid UserId { get; set; }
        public string Username { get; set; } = null!;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public string Token { get; set; } = null!;
    }
}
