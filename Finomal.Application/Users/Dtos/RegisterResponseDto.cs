namespace Finomal.Application.Users.Dtos
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
    }
}
