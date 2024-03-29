﻿namespace Shop.Domain.Domain
{
    public class ShopOrder
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string OrderCode { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpectedLeadTime { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
        public Guid IdPayment { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdOrderAddress { get; set; }

        public virtual Payment PaymentNavigation { get; set; }
        public virtual User UserNavigation { get; set; }
        public virtual OrderAddress OrderAddressNavigation { get; set; }
        public virtual ICollection<OrderItem> OrderItemsNavigation { get; set; }
    }
}