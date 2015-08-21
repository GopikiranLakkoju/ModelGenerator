using Microsoft.Practices.Unity;
using ModelGeneratorTool.Interfaces;

namespace ModelGeneratorTool.Utilities
{
    public class RegisterInIocContainer
    {
        /// <summary>
        /// Register Dependencies
        /// </summary>
        public static void RegisterDependencies()
        {
            IUnityContainer unitycontainer = new UnityContainer();
            // Register IModelGenerator in UnityContainer
            unitycontainer.RegisterType<IModelGenerator, ModelGenerator>();
            // Register ILogger in UnityContainer
            unitycontainer.RegisterType<ILogger, Logger>();
            unitycontainer.Resolve<MainWindow>().Show();
        }
    }
}