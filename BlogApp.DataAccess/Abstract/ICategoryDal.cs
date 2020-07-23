using BlogApp.Core;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.Abstract {
    public interface ICategoryDal : IEntityRepository<Category> {
    }
}
