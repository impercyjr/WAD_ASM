using ASMWAD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ASMWAD.Data.MyDatabaseContext;

namespace ASMWAD.Controllers
{
    public class StoreController : Controller
    {
        private MyDbContext db = new MyDbContext();
        // Tên của shopping cart trong session.
        private const string StoreSessionName = "SHOPPING_CART";
        // Action có tên là AddToCart. Có tham số truyền vào là id sản phẩm và số lượng muốn cho vào giỏ hàng.
        // Thêm một sản phẩm vào cart
        public ActionResult AddToCart(int productId, int quantity)
        {
            // Check product có tồn tại không?
            var existingProduct = db.Products.FirstOrDefault(p => p.Id == productId);
            if (existingProduct == null)
            {
                // trả về 404
                return new HttpNotFoundResult();
            }
            var store = GetStore();
            store.Add(existingProduct, quantity, false);
            SetStore(store);
            return RedirectToAction("ShowStore", "Store");
        }

        // hiển thị danh sách sản phẩm được thêm vào shopping cart.
        public ActionResult ShowStore()
        {
            return View("ShowStore", GetStore());
        }


        public ActionResult UpdateCart(int productId, int quantity)
        {
            var existingProduct = db.Products.FirstOrDefault(p => p.Id == productId);
            if (existingProduct == null)
            {
                // trả về 404
                return new HttpNotFoundResult();
            }
            var store = GetStore();
            store.Update(existingProduct, quantity);
            SetStore(store);
            return View("ShowStore");
        }


        public ActionResult RemoveCartItem(int productId)
        {
            var store = GetStore();
            store.Remove(productId);
            SetStore(store);
            return View("ShowStore");
        }

        public ActionResult RemoveAll()
        {
            ClearStore();
            return RedirectToAction("ShowStore", "Store");
        }

        private Store GetStore()
        {
            Store store = null;
            // Kiểm tra sự tồn tại của sc(shopping cart) trong session.
            if (Session[StoreSessionName] != null)
            {
                // nếu có
                try
                {
                    // ép kiểu đối tượng lấy được về kiểu ShoppingCart.
                    store = Session[StoreSessionName] as Store;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            if (store == null)
            {
                store = new Store();
            }
            return store;
        }

        private void SetStore(Store store)
        {
            Session[StoreSessionName] = store;
        }

        private void ClearStore()
        {
            Session[StoreSessionName] = null;
        }
    }
}
