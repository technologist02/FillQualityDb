using System;
using System.Collections.Generic;

namespace Добавление_данных_в_базу_из_Excel.Models
{
    public partial class UserRole
    {
        public UserRole()
        {
            UsersUsers = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string Function { get; set; } = null!;

        public virtual ICollection<User> UsersUsers { get; set; }
    }
}
