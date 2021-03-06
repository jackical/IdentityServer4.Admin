using System.ComponentModel.DataAnnotations;

namespace IdentityServer4.Admin.Controllers.API.Dtos
{
    /// <summary>
    /// 修改密码 DTO
    /// </summary>
    public class ChangeSelfPasswordInputDto
    {
        /// <summary>
        /// 新密码
        /// </summary>
        [StringLength(24)]
        [MinLength(6)]
        [Required]
        public string NewPassword { get; set; }

        /// <summary>
        /// 旧密码
        /// </summary>
        public string OldPassword { get; set; }
    }
}