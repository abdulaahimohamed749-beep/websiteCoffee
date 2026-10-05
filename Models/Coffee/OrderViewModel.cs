namespace websiteCoffee.Models.Coffee
{
    public class OrderViewModel
    {
        public List<CoffeeItem> AvailableCoffees { get; set; } = new();
        public List<CartItem> SelectedItems { get; set; } = new();
        public decimal TotalAmount => SelectedItems.Sum(item => item.Price * item.Quantity);
    }

    public class CartItem
    {
        public int CoffeeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}