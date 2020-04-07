// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Installer.cs" company="LEGO System A/S">
//   Copyright (c) LEGO System A/S. All rights reserved.
// </copyright>
// <summary>
//   The installer.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TravianManager.Core.Managers;

namespace TravianManager.WindsorInstaller
{
    using System.Data.SqlClient;
    using Castle.MicroKernel.Registration;
    using Castle.MicroKernel.SubSystems.Configuration;
    using Castle.Windsor;
    using Core.Data;
    using Data.Data;
    using TravianManager.BusinessLogic;
    using TravianManager.BusinessLogic.Managers;
    using TravianManager.Core;
    using TravianManager.Core.Context;
    using TravianManager.Core.DataProvider;
    using TravianManager.Data.Sql;
    using TravianManager.Data.Sql.Context;

    /// <summary>
    /// <inheritdoc cref="Installer"/>
    /// </summary>
    public class Installer : IWindsorInstaller
    {
        /// <summary>
        /// The AppSettings.json configuration file, mapped into a POCO for type-safety.
        /// </summary>
        private readonly IAppSettingsPoco _appSettingsPoco;

        /// <summary>
        /// Initializes a new instance of the <see cref="Installer"/> class.
        /// </summary>
        /// <param name="appSettingsPoco">
        /// The app settings easy config POCO.
        /// </param>
        public Installer(IAppSettingsPoco appSettingsPoco)
        {
            _appSettingsPoco = appSettingsPoco;
        }

        /// <summary>
        /// <inheritdoc cref="Installer"/>
        /// </summary>
        /// <param name="container">
        /// the container.
        /// </param>
        /// <param name="store">
        /// The ConfigurationStore.
        /// </param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component.For<IAppSettingsPoco>()
                    .ImplementedBy<AppSettingsPoco>()
                    .DependsOn(_appSettingsPoco)
                    .LifestyleSingleton(),
                Component.For<IEntityFrameworkDbContext>()
                    .ImplementedBy<EntityFrameworkDbContext>()
                    .LifestyleTransient(),
                Component.For<IAccountManager>()
                    .ImplementedBy<AccountManager>()
                    .LifestyleSingleton(),
                Component.For<IUserDataProvider>()
                    .ImplementedBy<UserDataProvider>()
                    .LifestyleSingleton(),
                Component.For<IHelpers>()
                    .ImplementedBy<Helpers>()
                    .LifestyleSingleton(),
                 Component.For<ITemplateManager>()
                    .ImplementedBy<TemplateManager>()
                    .LifestyleSingleton(),
                 Component.For<ICalculator>()
                    .ImplementedBy<Calculator>()
                    .LifestyleSingleton(),
                 Component.For<ITemplateDataProvider>()
                    .ImplementedBy<TemplateDataProvider>()
                    .LifestyleSingleton());
        }
    }
}