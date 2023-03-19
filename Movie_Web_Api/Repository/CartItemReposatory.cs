using Movie_Web_Api.Models;
using Movie_Web_Api.Dto;
using Microsoft.EntityFrameworkCore;

namespace Movie_Web_Api.Repository
{
    public class CartItemReposatory
    {
        public readonly AppDbContext _db;

        public CartItemReposatory(AppDbContext context)
        {
            _db=context;
        }

        public bool CartItemExists(long id)
        {
            return _db.CartItems.Any(e => e.CartItemId == id);
        }
        public List<CartItem> GetShoppingCartItems()
        {
            return CartItems ?? (ShoppingCartItems = _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }

        public async Task<CartItem> GetDbItem(CartItemRequestDTO cartItem)
        {
            return await _db.CartItems
               .Where(e => e.UserId == cartItem.UserId && e.MovieId == cartItem.MovieId)
               .FirstOrDefaultAsync();
        }

        public async Task<CartItem> GetDbItemWithUser(CartItemRequestDTO cartItem)
        {
            return await _db.CartItems
                .Where(e => e.UserId == cartItem.UserId && e.MovieId == cartItem.MovieId)
                .Include(e => e.User)
                .FirstOrDefaultAsync();
        }

        public async Task ValidateRequestBody(CartItemRequestDTO cartItem)
        {
            if (cartItem.Quantity <= 0)
            {
                throw new ArgumentException("Invalid product quantity.");
            }

            Movie product = await _db.Movies.FindAsync(cartItem.MovieId);

            if (product == null)
            {
                throw new ArgumentException("Invalid product.");
            }

            ApplicationUser user = await _db.Users.FindAsync(cartItem.UserId);

            if (user == null)
            {
                throw new ArgumentException("Invalid user.");
            }
        }



    }
}
