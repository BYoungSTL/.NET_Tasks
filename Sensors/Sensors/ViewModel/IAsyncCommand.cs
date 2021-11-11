using System.Threading.Tasks;
using System.Windows.Input;

namespace Sensors.ViewModel
{
    public interface IAsyncCommand : ICommand
    {
        public Task ExecuteAsync(object parameter);
    }
}