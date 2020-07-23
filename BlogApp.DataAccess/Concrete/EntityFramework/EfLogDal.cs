using BlogApp.Core.EntityFramework;
using BlogApp.DataAccess.Abstract;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.Concrete.EntityFramework {
    public class EfLogDal : EfRepositoryBase<Log, BlogDbContext>, ILogDal {
    }
}
