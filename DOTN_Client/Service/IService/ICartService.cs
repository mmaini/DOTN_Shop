using DOTN_Client.ViewModels;

namespace DOTN_Client.Service.IService
{
    public interface ICartService
    {
        public event Action OnChange;
        Task DecrementCart(ShoppingCart cart);
        Task IncrementCart(ShoppingCart cart);
    }
}
