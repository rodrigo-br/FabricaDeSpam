namespace Domain.Entities
{
    using Microsoft.AspNetCore.Identity;

    public class User : IdentityUser
    {
        public User() : base() {}

        ICollection<Image>? Images { get; set; }
    }
}
