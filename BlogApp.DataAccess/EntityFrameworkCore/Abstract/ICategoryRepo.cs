using BlogApp.Core.EntityFrameworkCore;
using BlogApp.Entity.Concrete;

namespace BlogApp.DataAccess.EntitFrameworkCore.Abstract {
    public interface ICategoryRepo : IRepoBase<Category> {
    }
}
