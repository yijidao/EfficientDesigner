using System.Threading.Tasks;
using EfficientDesigner_Client_Common.Models;

namespace EfficientDesigner_Client_Common.Services
{
    public interface ILayoutService
    {
        Task<LayoutModel[]> GetLayoutList();
    }
}
