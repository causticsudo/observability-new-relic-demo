namespace ObservabilityDemo.Api.Contracts.Requests
{
    public class CreateUserRequest
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}