using Microsoft.AspNetCore.Identity;

namespace SportClub.Model
{
    public class About
    {
        public int id { get; set; }
        public string? phoneNum { get; set; }
        public string? old_pass { get; set; }
        public string? new_pass { get; set; }
    }
}
