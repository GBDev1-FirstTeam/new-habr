#nullable disable

namespace NewHabr.Domain.Models;

public class UserLikedUser
{
    // кому поставили лайк
    public Guid Id { get; set; }
    public string UserName { get; set; }

    // кто поставил лайк
    public Guid UserId { get; set; }
}
