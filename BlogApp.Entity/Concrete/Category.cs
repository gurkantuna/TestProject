using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BlogApp.Core.Entities;
using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Entity.Concrete {
    public class Category : EntityBase, IEntity {

        [Required, StringLength(maximumLength: 20, MinimumLength = 3)]
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}
