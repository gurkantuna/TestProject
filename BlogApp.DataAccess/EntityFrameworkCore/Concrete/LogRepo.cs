using BlogApp.Core.EntityFramework;
using BlogApp.DataAccess.Concrete.EntityFrameworkCore.Context;
using BlogApp.DataAccess.EntitFrameworkCore.Abstract;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.EntitFrameworkCore.Concrete {
    public class LogRepo : RepoBase<Log, BlogDbContext>, ILogRepo {
    }
}
