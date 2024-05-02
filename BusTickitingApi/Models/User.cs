using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusTickitingApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Roles { get; set; }
    }

    public class Purchase
    {
        public int PurchaseId { get; set; }
        public string PurchaseNo { get; set; }
        public string PassangerName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsPaid { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<PurchaseTickit> PurchaseTickits { get; set; }
    }

    public class PurchaseTickit
    {
        public int PurchaseTickitId { get; set; }
        public int PurchaseId { get; set; }
        public int SeatTypeId { get; set; }
        public virtual SeatType SeatType { get; set; }
    }

    public class SeatType
    {
        public int SeatTypeId { get; set; }
        public string TypeName { get; set; }
        public decimal SeatFear { get; set; }
    }
    public class PurchaseRequest
    {
        public Purchase Purchase { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageFileName { get; set; }
    }

}