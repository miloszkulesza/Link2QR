using System.Threading.Tasks;

namespace Link2QR.Services
{
    public interface ISaveFile
    {
        Task<bool> SaveFile(byte[] bytes, string filename);
    }
}
