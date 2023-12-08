using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class User
    {
        public User()
        {
            OrdersQualities = new HashSet<OrdersQuality>();
            RolesRoles = new HashSet<UserRole>();
        }

        public int UserId { get; set; }
        public string Login { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public DateTime Created { get; set; }

        public virtual ICollection<OrdersQuality> OrdersQualities { get; set; }

        public virtual ICollection<UserRole> RolesRoles { get; set; }
    }
}
