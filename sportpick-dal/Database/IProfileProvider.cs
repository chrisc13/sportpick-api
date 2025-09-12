using System.Threading.Tasks;

namespace sportpick_dal
{
    public interface IProfileProvider
    {
        Task<ProfileEntity?> GetByUsernameAsync(string username);
        Task<ProfileEntity?> GetByUserIdAsync(string userId);
        Task<bool> UpsertProfileAsync(ProfileEntity profile);
        Task EnsureIndexesAsync();
    }
}
