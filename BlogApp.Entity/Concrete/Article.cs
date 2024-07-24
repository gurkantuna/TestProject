using System;
using System.ComponentModel.DataAnnotations;
using BlogApp.Core.Entities;
using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Entity.Concrete {
    public class Article : EntityBase, IEntity {

        [Required, StringLength(maximumLength: 50, MinimumLength = 5)]
        public string Title { get; set; }
        [Required, StringLength(maximumLength: 200, MinimumLength = 10)]
        public string Description { get; set; }
        [StringLength(maximumLength: 4000)]
        public string Body { get; set; }
        public string Image { get; set; }
        public string ImageThumb { get; set; }
        public DateTime Date { get; set; }
        public bool IsApproved { get; set; }

        [Required(ErrorMessage = "The Category field is required.")]
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
