namespace BlogApp.WebUI.Models {
    public class RoleEditModel {

        public string RoleId { get; set; }
        public string RoleName { get; set; }//RoleName
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
