using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using UniCoffeeShop.Models;

namespace UniCoffeeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ProductDBAccessLayer productDb = new ProductDBAccessLayer();
        private readonly SignInManager<ApplicationUser> _signManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            _signManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var products = productDb.GetAllProducts();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<Cart> GetShoppingCart()
        {
            Cart cart = new Cart();
            var user = await _userManager.GetUserAsync(HttpContext.User);
            cart = user.GetShoppingCart();
            await _userManager.UpdateAsync(user);
            return cart;
        }

        [Route("cart/add")]
        public async Task<IActionResult> AddToCart(string productId, uint quantity)
        {
            if (quantity > 0)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                CartItem item = new CartItem
                {
                    Product = productDb.GetProduct(productId),
                    Quantity = quantity
                };

                Cart cart = user.GetShoppingCart();
                cart.AddItemToCart(item);
                user.SetShoppingCart(cart);

                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Index");
        }

        [Route("cart/remove")]
        public async Task<IActionResult> RemoveFromCart(string productId)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            Cart cart = user.GetShoppingCart();
            cart.RemoveItemFromCart(productId);
            user.SetShoppingCart(cart);

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Cart");
        }

        [Route("cart/clear")]
        public async Task<IActionResult> ClearCart()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                user.ShoppingCart = null;
                await _userManager.UpdateAsync(user);
            }

            return RedirectToAction("Cart");
        }

        [Route("cart/adjust")]
        public async Task<IActionResult> AdjustQuantityOfCartItem(string productId, uint quantity)
        {
            if (quantity > 0)
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                Cart cart = user.GetShoppingCart();
                cart.AdjustQuantity(productId, quantity);
                user.SetShoppingCart(cart);

                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Cart");
        }

        [HttpGet]
        [Route("GetShoppingCartCount")]
        public async Task<ActionResult> GetShoppingCartCount()
        {
            Cart cart = await GetShoppingCart();
            return Json(new { cartCount = cart.ShoppingCart.Count });
        }

        public async Task<IActionResult> Cart()
        {
            Cart cart = await GetShoppingCart();
            return View(cart);
        }
    }
}