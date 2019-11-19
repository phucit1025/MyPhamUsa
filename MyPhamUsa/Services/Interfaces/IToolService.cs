using System.Threading.Tasks;

namespace MyPhamUsa.Services.Interfaces
{
    public interface IToolService
    {
        void DeleteImages();
        void DeleteCategoryMappings();
        Task CreateAdmin(string username, string password);
    }
}
