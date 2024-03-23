﻿namespace Shop.Domain.Domain
{
    public class UserRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}