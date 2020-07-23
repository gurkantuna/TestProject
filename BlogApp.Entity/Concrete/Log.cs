using System;
using BlogApp.Core.Entities;
using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Entity.Concrete {
    public class Log : EntityBase, IEntity {

        public string Detail { get; set; }

        public DateTime? Date { get; set; }

        public string Audit { get; set; }

        public string User { get; set; }

        public string Ip { get; set; }
    }
}
