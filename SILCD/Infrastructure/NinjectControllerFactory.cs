using Ninject;
using SILCD.Repository.Abstract;
using SILCD.Repository.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SILCD.Infrastructure {
    public class NinjectControllerFactory : DefaultControllerFactory, IDisposable {
        private IKernel ninjectKernel;
        public NinjectControllerFactory() {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType) {
            return controllerType == null
            ? null
            : (IController)ninjectKernel.Get(controllerType);
        }
        private void AddBindings() {
            ninjectKernel.Bind<IDeputadosRepository>().To<DeputadosRepository>();
        }

        public void Dispose()
        {
            ninjectKernel = null;
        }

        ~NinjectControllerFactory()
        {
            this.Dispose();
        }
    }
}