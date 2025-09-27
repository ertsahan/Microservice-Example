namespace Order.API.ViewModels
{
    public struct CreateOrderVM
    {
        public Guid BuyerId { get; set; }
        public List<CreateOrderItemVM> OrderItems { get; set; }
    }
}
