using Microsoft.AspNetCore.Mvc;
using RumahMakanOnline_API.Models;
using RumahMakanOnline_ViewModels;

namespace RumahMakanOnline_API.Controllers
{
        [Route("[controller]")]
        [ApiController]
    public class ApiOrderController : Controller
    {
        private readonly RestaurantKakiTujuhContext db;
        private VMRespons respon = new VMRespons();
        //private int IdCustomer = 1;

        public ApiOrderController(RestaurantKakiTujuhContext db)
        {
            this.db = db;
        }
        [HttpPost("SubmitPayment")]
        public VMRespons SubmitPayment(VMOrderHeader dataheader)
        {
            OrderHeader head = new OrderHeader();
            head.TotalQty = dataheader.TotalQty;
            head.Amount = dataheader.Amount;
            head.CustId = dataheader.CustId;
            head.IsCheckout = dataheader.IsCheckout;
            head.CodeTransaction = GenerateCode();

            head.CreateBy = dataheader.CreateBy;
            head.CreateDate = DateTime.Now;
            head.IsDelete = false;

            try
            {
                db.Add(head);
                db.SaveChanges();

                foreach (VMOrderDetail item in dataheader.ListDetail)
                {
                    OrderDetail detail = new OrderDetail();
                    detail.HeaderId = head.Id;
                    detail.ProductId = item.ProductId;
                    detail.Qty = item.Qty;
                    detail.SubTotal = item.SubTotal;
                    detail.IsDelete = false;
                    detail.CreateBy = dataheader.CreateBy;
                    detail.CreateDate = DateTime.Now;

                    try
                    {
                        db.Add(detail);
                        db.SaveChanges();

                        FoodProduct prod = db.FoodProducts.Where(a => a.Id == item.ProductId).FirstOrDefault();
                        if (prod != null)
                        {
                            prod.Stock = prod.Stock - item.Qty;
                            try
                            {
                                db.Update(prod);
                                db.SaveChanges();

                                respon.Message = "Thanks for order";
                            }
                            catch (Exception e)
                            {
                                respon.Success = false;
                                respon.Message = e.Message;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        respon.Success = false;
                        respon.Message = e.Message;
                    }
                }
            }
            catch (Exception e)
            {
                respon.Success = false;
                respon.Message = e.Message;
            }

            return respon;
        }
        [HttpGet("CodeGenerate")]
        public string GenerateCode()
        {
            string code = $"RM-{DateTime.Now.ToString("ddMMyyyy")}-";
            string digit = "";

            OrderHeader dataLast = db.OrderHeaders.OrderByDescending(a => a.CodeTransaction).FirstOrDefault();
            if (dataLast == null)
            {
                digit = "00001";
            }
            else
            {
                string defaultDigit = "00000";
                string codeLast = dataLast.CodeTransaction;
                string subCode = codeLast.Substring(14);
                int intCode = int.Parse(subCode);
                intCode++;
                int lenCode = intCode.ToString().Length;
                defaultDigit = defaultDigit + intCode.ToString();
                defaultDigit = defaultDigit.Substring(lenCode, 5);

                digit = defaultDigit;
            }
            return code + digit;
        }
        [HttpGet("GetAllOrderHeader/{IdCustomer}")]
        public List<VMOrderHeader> GetAllOrderHeader(int IdCustomer)
        {
            List<VMOrderHeader> dataOrderHeader = new List<VMOrderHeader>();
            //dataVariant = db.TblVariants.Where(a => a.IsDelete == false).ToList();
            dataOrderHeader = (from oh in db.OrderHeaders
                               where oh.IsDelete == false && oh.CustId == IdCustomer
                               select new VMOrderHeader
                               {
                                   Id = oh.Id,
                                   CodeTransaction = oh.CodeTransaction,
                                   CustId = oh.CustId,
                                   Amount = oh.Amount,
                                   TotalQty = oh.TotalQty,
                                   IsCheckout = oh.IsCheckout,
                                   CreateDate = oh.CreateDate,

                                   ListDetail = (from od in db.OrderDetails
                                                 join p in db.FoodProducts
                                                 on od.ProductId equals p.Id
                                                 where od.IsDelete == false && od.HeaderId == oh.Id
                                                 select new VMOrderDetail
                                                 {
                                                     Id = od.Id,
                                                     Qty = od.Qty,
                                                     SubTotal = od.SubTotal,
                                                     HeaderId = od.HeaderId,
                                                     ProductId = p.Id,
                                                     NameProduct = p.NameProduct,
                                                     Price = p.Price
                                                 }).ToList()
                               }).ToList();

            return dataOrderHeader;
        }
        [HttpGet("countTransaction/{idUser}")]
        public int countTransaction(int idUser)
        {
            int count = 0;
            count = db.OrderHeaders.Where(a => a.CustId == idUser).Count();
            //count = db.TblOrderHeaders.Where(a => a.IdCustomer == idUser).Select(a => a.Id).Sum();
            return count;
        }

        [HttpGet("AmounYear/{idUser}")]
        public decimal AmounYear(int idUser)
        {
            decimal count = 0;
            count = db.OrderHeaders.Where(a => a.CustId== idUser && a.CreateDate.Year == DateTime.Now.Year).Select(a => a.Amount).Sum();
            return count;
        }
    }
}
