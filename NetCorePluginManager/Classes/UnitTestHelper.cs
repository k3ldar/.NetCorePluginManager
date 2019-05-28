#if DEBUG
using SharedPluginFeatures;

namespace AspNetCore.PluginManager
{
    /// <summary>
    /// This class is only available for Debug builds and is designed to retrieve classes
    /// that are normally only available through DI
    /// </summary>
    public static class UnitTestHelper
    {
        /// <summary>
        /// Returns an instance of IPluginHelperService for unit tests
        /// </summary>
        /// <returns>IPluginHelperService</returns>
        public static IPluginHelperService GetPluginServices()
        {
            return new PluginServices() as IPluginHelperService;
        }

        /// <summary>
        /// Returns an instance of IPluginClassesService for unit tests
        /// </summary>
        /// <returns>IPluginClassesService</returns>
        public static IPluginClassesService GetPluginClassesService()
        {
            return new PluginServices() as IPluginClassesService;
        }

        /// <summary>
        /// Returns an instance of IPluginTypesService for unit tests
        /// </summary>
        /// <returns>IPluginTypesService</returns>
        public static IPluginTypesService GetPluginTypesService()
        {
            return new PluginServices() as IPluginTypesService;
        }

        /// <summary>
        /// Returns an instance of INotificationService for unit tests
        /// </summary>
        /// <returns>INotificationService</returns>
        public static INotificationService GetNotificationService()
        {
            return new NotificationService() as INotificationService;
        }

    }
}

#endif
