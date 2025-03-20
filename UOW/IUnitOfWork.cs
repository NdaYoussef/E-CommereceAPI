using TestToken.Repositories.Interfaces;

namespace TestToken.UOW
{
    public interface IUnitOfWork : IDisposable
    {
        public IAccountRepository Customers { get; }
        public IBrandRepository Brands { get; }
        public ICartItemRepository CartItems { get; }
        public ICartRepository Carts { get; }

        public ICategoryRepository Categories { get; }

        public IProductRepository Products { get;  }

        public IPaymentRepository Payments { get;  }

        public IWishListItemRepository WishListItems { get;}

        public IWishListRepository WishLists { get;}

        public IOrderItemRepository OrderItems { get; }

        public IOrderRepository Orders { get;}
        public ITokenService TokenService { get; }
        public IReviewRepository Reviews { get; }

        Task<int> SaveCompleted();

    }
}
