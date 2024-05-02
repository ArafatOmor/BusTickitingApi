using BusTickitingApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace BusTickitingApi.Controllers
{
    public class PurchaseController : ApiController
    {
        private AppDbContext db = new AppDbContext();
        //[Authorize(Roles = ("User,Admin"))]
        public System.Object GetOrders()
        {
            var result = db.Purchases.ToList();
            return result.OrderBy(o => o.PurchaseId);
        }
        //[Authorize(Roles = ("User,Admin"))]
        public IHttpActionResult GetOrderById(int id)
        {
            var purchase = (from a in db.Purchases
                         where a.PurchaseId == id
                         select new
                         {
                             a.PurchaseId,
                             a.PurchaseNo,
                             a.PassangerName,
                             a.IsPaid,
                             a.ImageUrl
                         }).FirstOrDefault();
            var purchaseItem = (from a in db.PurchaseTickits
                                join b in db.SeatTypes on a.SeatTypeId equals b.SeatTypeId
                                where a.PurchaseId == id
                                select new
                                {
                                    a.PurchaseId,
                                    //a.OrderedItemId,
                                    a.SeatTypeId,
                                    b.TypeName,
                                    b.SeatFear
                                }).ToList();
            return Ok(new { purchase, purchaseItem });

        }
        //[Authorize(Roles = ("Admin"))]
        public IHttpActionResult DeleteOrder(int id)
        {
            var purchase = db.Purchases.Find(id);
            var purchaseItem = db.PurchaseTickits.Where(item => item.PurchaseId == id).ToList();
            foreach (var item in purchaseItem)
            {
                db.PurchaseTickits.Remove(item);
            }
            db.Purchases.Remove(purchase);
            db.SaveChanges();
            return Ok("Purchase and related items have been successfully deleted.");
        }
        //[Authorize(Roles = ("Admin"))]
        public IHttpActionResult PostOrder(PurchaseRequest request)
        {
            if (request.Purchase == null)
            {
                return BadRequest("Purchase data is Missing");
            }
            Purchase obj = request.Purchase;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);

                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            Purchase purchase = new Purchase
            {
                PurchaseNo = obj.PurchaseNo,
                PurchaseDate = obj.PurchaseDate,
                PassangerName = obj.PassangerName,
                IsPaid = obj.IsPaid,
                ImageUrl = obj.ImageUrl,
            };
            db.Purchases.Add(purchase);
            db.SaveChanges();
            var purchaseObj = db.Purchases.FirstOrDefault(x => x.PurchaseNo == obj.PurchaseNo);
            if (purchaseObj != null && obj.PurchaseTickits != null)
            {
                foreach (var item in obj.PurchaseTickits)
                {
                    PurchaseTickit purchaseItem = new PurchaseTickit
                    {
                        PurchaseId = purchaseObj.PurchaseId,
                        SeatTypeId = item.SeatTypeId,
                    };
                    db.PurchaseTickits.Add(purchaseItem);
                }
            }
            db.SaveChanges();
            return Ok("Purchase Saved Successfully");
        }
        //[Authorize(Roles = ("Admin"))]
        public IHttpActionResult PutOrder(int id, PurchaseRequest request)
        {
            Purchase purchase = db.Purchases.FirstOrDefault(x => x.PurchaseId == id);
            if (id != request.Purchase.PurchaseId)
            {
                return BadRequest();
            }
            if (purchase == null)
            {
                return NotFound();
            }
            if (request.Purchase == null)
            {
                return BadRequest("Purchase data is missing.");
            }
            Purchase obj = request.Purchase;
            byte[] imageFile = request.ImageFile;
            if (imageFile != null && imageFile.Length > 0)
            {
                string fileName = Guid.NewGuid().ToString() + ".jpg";
                string filePath = Path.Combine("~/Images/", fileName);
                File.WriteAllBytes(HttpContext.Current.Server.MapPath(filePath), imageFile);
                obj.ImageUrl = filePath;
            }
            purchase.PurchaseNo = obj.PurchaseNo;
            purchase.PurchaseDate = obj.PurchaseDate;
            purchase.PassangerName = obj.PassangerName;
            purchase.IsPaid = obj.IsPaid;
            purchase.ImageUrl = obj.ImageUrl;
            var existingPurchaseItems = db.PurchaseTickits.Where(x => x.PurchaseId == purchase.PurchaseId);
            db.PurchaseTickits.RemoveRange(existingPurchaseItems);
            if (obj.PurchaseTickits != null)
            {
                foreach (var item in obj.PurchaseTickits)
                {
                    PurchaseTickit purchaseItem = new PurchaseTickit
                    {
                        PurchaseId = purchase.PurchaseId,
                        SeatTypeId = item.SeatTypeId,
                    };
                    db.PurchaseTickits.Add(purchaseItem);
                }
            }
            db.SaveChanges();
            return Ok("Purchase and related items have been successfully updated.");
        }
    }
}
