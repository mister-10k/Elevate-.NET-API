using Elevate.Business;
using Elevate.Data;
using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Unity.Lifetime;

namespace Elevate.IOC
{
    public static class IOCRegistration
    {
        public static void Register(UnityContainer container)
        {
            #region Business Layer
            container.RegisterType<IBenifitsPackageDL, BenifitsPackageDL>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersBL, UsersBL>(new HierarchicalLifetimeManager());
            #endregion

            #region Data Layer
            container.RegisterType<IBenifitsPackageBL, BenifitsPackageBL>(new HierarchicalLifetimeManager());
            container.RegisterType<IUsersDL, UsersDL>(new HierarchicalLifetimeManager());
            #endregion



        }
    }
}
