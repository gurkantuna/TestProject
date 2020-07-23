using BlogApp.Core.Entities.Abstract;

namespace BlogApp.Core.Entities.Concrete {
    public class UserOperationClaim : EntityBase {

        public int UserId { get; set; }

        public int OperationClaimId { get; set; }
    }
}