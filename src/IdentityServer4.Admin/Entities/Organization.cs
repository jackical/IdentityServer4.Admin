using System;
using System.ComponentModel.DataAnnotations;
using IdentityServer4.Admin.Infrastructure.Entity;

namespace IdentityServer4.Admin.Entities
{
    /// <summary>
    /// 组织
    /// </summary>
    public class Organization : EntityBase<Guid>
    {
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [StringLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// 所属组织标识
        /// </summary>
        public Guid ParentId { get; set; }
    }
}