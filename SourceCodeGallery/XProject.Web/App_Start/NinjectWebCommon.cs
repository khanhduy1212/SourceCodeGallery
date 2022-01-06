using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using CsQuery.Engine.PseudoClassSelectors;
using FluentValidation;
using FluentValidation.Mvc;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using XProject.Web.Validation;
using Ninject;
using Ninject.Web.Common;
using Ninject.Web.Mvc.FluentValidation;
using XProject.Domain;
using XProject.Domain.Abstract;
using XProject.Domain.Concrete;
using XProject.Domain.Entities;
using XProject.Domain.Helpers.Encryption;
using XProject.Web;
using XProject.Web.Infrastructure;

using Portable.Licensing;
using WebActivator;

[assembly: WebActivator.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof(NinjectWebCommon), "Stop")]

namespace XProject.Web
{
    public static class NinjectWebCommon
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        ///     Starts the application
        /// </summary>
        public static void Start()
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        ///     Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        ///     Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = DependencyContainer.Current ?? new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            var ninjectValidatorFactory = new NinjectValidatorFactory(kernel);
            ModelValidatorProviders.Providers.Add(new FluentValidationModelValidatorProvider(ninjectValidatorFactory));
            DataAnnotationsModelValidatorProvider.AddImplicitRequiredAttributeForValueTypes = false;

            RegisterServices(kernel);
            return kernel;
        }


        /// <summary>
        ///     Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            // Database context
            kernel.Bind<DbContext>().To<XProjectDb>().InRequestScope();
            // Logging
            kernel.Bind<IAuditTracker>().To<EFAuditTracker>()
                // initialize DbContext in another block so it will not persist temporary changes in business flow
                  .WithConstructorArgument("db", c => c.Kernel.BeginBlock().Get<DbContext>());

            // Repositories
            kernel.Bind<IAuthenticationService, IMembershipService>().To<InMemLoginTracker>();
            kernel.Bind<IAuthorizationService, IRoleService>().To<EFRoleBasedAuthorizer>();
            kernel.Bind<ILoginTracker>().To<InMemLoginTracker>()
                  .WithConstructorArgument("concurrentMax", Convert.ToInt32(ConfigurationManager.AppSettings["ConcurrentLoginMax"]));
            //_kernel.Bind<ILoginTracker>().To<InMemLoginTracker>();
            kernel.Bind<EFMenuRepository>().ToSelf();
            kernel.Bind<IMenuRepository>().To<MemoryMenuRepository>().InSingletonScope();

            kernel.Bind<IUnitRepository>().To<EFUnitRepository>();
            kernel.Bind<ISettingRepository>().To<EFSettingRepository>();
          


            
            kernel.Bind<IGeneralRepository<XState>>().To<EFGeneralRepository<XState>>();
            kernel.Bind<IGeneralRepository<XStateText>>().To<EFGeneralRepository<XStateText>>();
            kernel.Bind<IGeneralRepository<XDistrict>>().To<EFGeneralRepository<XDistrict>>();
            kernel.Bind<IGeneralRepository<XDistrictText>>().To<EFGeneralRepository<XDistrictText>>();
            kernel.Bind<IGeneralRepository<XLocation>>().To<EFGeneralRepository<XLocation>>();
            kernel.Bind<IGeneralRepository<XLocationText>>().To<EFGeneralRepository<XLocationText>>();
   
            kernel.Bind<IGeneralRepository<Setting>>().To<EFGeneralRepository<Setting>>();

            ///////Web
            


            kernel.Bind<IGeneralRepository<XCountry>>().To<EFGeneralRepository<XCountry>>();
            kernel.Bind<IGeneralRepository<XPicture>>().To<EFGeneralRepository<XPicture>>();
            kernel.Bind<IGeneralRepository<XMenu>>().To<EFGeneralRepository<XMenu>>();
            kernel.Bind<IGeneralRepository<XSlider>>().To<EFGeneralRepository<XSlider>>();
            kernel.Bind<IGeneralRepository<XNew>>().To<EFGeneralRepository<XNew>>();
            kernel.Bind<IGeneralRepository<XContent>>().To<EFGeneralRepository<XContent>>();
            kernel.Bind<IGeneralRepository<XEmail>>().To<EFGeneralRepository<XEmail>>();
            kernel.Bind<IGeneralRepository<XContentDetail>>().To<EFGeneralRepository<XContentDetail>>();
            kernel.Bind<IGeneralRepository<XProduct>>().To<EFGeneralRepository<XProduct>>();
            kernel.Bind<IGeneralRepository<XAttribute>>().To<EFGeneralRepository<XAttribute>>();
            kernel.Bind<IGeneralRepository<XType>>().To<EFGeneralRepository<XType>>();
            kernel.Bind<IGeneralRepository<XPartner>>().To<EFGeneralRepository<XPartner>>();
            kernel.Bind<IGeneralRepository<XSeo>>().To<EFGeneralRepository<XSeo>>();



            kernel.Bind<IStringEncryptor>().To<SimplerAES>();
            // cache configurations.
            kernel.Bind<ICacheStorageLocation>().To<RequestCacheSolution>().Named("InRequest");
            kernel.Bind<ICacheStorageLocation>().To<SessionCacheSolution>().Named("InSession");

       
            //

            DependencyResolver.SetResolver(new NinjectResolver(kernel));
        }
    }
}
