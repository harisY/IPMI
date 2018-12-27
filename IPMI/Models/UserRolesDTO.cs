using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IPMI.Models
{
    public class ExpandedUserDTO
    {
        [Key]
        [Display(Name = "User Name")]
        public string NameIdentifier { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [Display(Name = "Lockout End Date Utc")]
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string PhoneNumber { get; set; }
        public int IdLevel { get; set; }
        public string LevelID { get; set; }
        public string GroupID { get; set; }
        public string LevelName { get; set; }
        public string GroupName { get; set; }
        public string IdUser { get; set; }
        public int IdGroup { get; set; }
        public string PartnerID { get; set; }

        public string IdDept { get; set; }

        //[Required, Microsoft.Web.Mvc.FileExtensions(Extensions = "csv",
        //     ErrorMessage = "Specify a CSV file. (Comma-separated values)")]
        public string Photo { get; set; }
        public IEnumerable<UserRolesDTO> Roles { get; set; }
    }

    public class UserRolesDTO
    {
        [Key]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }
    
    public class UserRoleDTO
    {
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class RoleDTO
    {
        [Key]
        public string Id { get; set; }
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }
    }

    public class UserAndRolesDTO
    {
        [Key]
        [Display(Name = "User Name")]
        public string UserName { get; set; }
        public List<UserRoleDTO> colUserRoleDTO { get; set; }
    }
}