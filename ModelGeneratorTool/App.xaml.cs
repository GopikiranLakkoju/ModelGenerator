using System.Windows;
using ModelGeneratorTool.Utilities;

namespace ModelGeneratorTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            RegisterInIocContainer.RegisterDependencies();            
        }
    }
}
