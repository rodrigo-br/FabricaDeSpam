namespace Domain.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User() : base() {}

        public ICollection<Image>? Images { get; set; }
    }
}
