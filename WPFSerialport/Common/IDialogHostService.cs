using Prism.Services.Dialogs;
using System.Threading.Tasks;

namespace WPFSerialport.Common
{
    public interface IDialogHostService : IDialogService
    {
        Task<IDialogResult> ShowDialog(string name, IDialogParameters parameters, string dialogHostName = "Root");
    }
}
