using ReadLater.Data;
using ReadLater.Repository;
using ReadLater.Services;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace MVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<ICategoryService, CategoryService>();
            container.RegisterType<IBookmarkService, BookmarkService>();
            container.RegisterType<IDbContext, ReadLaterDataContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}