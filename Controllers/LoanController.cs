using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using borrower.Models;

namespace borrower.Controllers
{
    public class LoanController: Controller
    {
        private LoanContext _context;
        public LoanController(LoanContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("lender/{LenderId}")]
        public IActionResult GetLender(int LenderId)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            System.Console.WriteLine(LenderId);
            Lender thisLender = _context.lenders.SingleOrDefault(lender => lender.LenderId == LenderId);

            List<Borrower> Borrowers = _context.borrowers.Include(borrower => borrower.MoneyReceived).ToList();

            List<Loan> myLoans = _context.loans.Where(loan => loan.LenderId == LenderId)
                                                .Include(loan => loan.Lender)
                                                .Include(loan => loan.Borrower)
                                                    .ThenInclude(borrower => borrower.MoneyReceived)
                                                .ToList();
            
            List<Loan> allLoans = _context.loans.Include(loan => loan.Borrower).Include(loan => loan.Lender).ToList();
            int moneyDonated = 0;
            foreach (var loan in myLoans)
            {
                moneyDonated += loan.Amount;
            }
            ViewBag.Borrowers = Borrowers;
            ViewBag.Lender = thisLender;
            ViewBag.Loans = myLoans;
            ViewBag.AllLoans = allLoans;
            ViewBag.MoneyDonated = moneyDonated;
            ViewBag.CurrentBalance = thisLender.Money - moneyDonated;
            return View();

        }
        [HttpPost]
        [Route("lend_money")]
        public IActionResult LendMoney(int BorrowerId, int MoneyLent)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");

            Loan newLoan = new Loan
            {
                Amount = MoneyLent,
                LenderId = (int)HttpContext.Session.GetInt32("UserId"),
                BorrowerId = BorrowerId,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            //detract from the user balance here? should be fine...
            //or calculate it later! get the count of all their donations and just subtract loll ez.
            _context.Add(newLoan);
            _context.SaveChanges();
            return RedirectToAction("GetLender", new {LenderId = (int)HttpContext.Session.GetInt32("UserId")});
        }

        [HttpGet]
        [Route("borrower/{BorrowerId}")]
        public IActionResult GetBorrower(int BorrowerId)
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            Borrower thisBorrower = _context.borrowers.SingleOrDefault(borrower => borrower.BorrowerId == BorrowerId);

            List<Loan> receivedLoans = _context.loans.Where(loan => loan.BorrowerId == BorrowerId)
                                                    .Include(loan => loan.Lender)
                                                    .ToList();
            
            int amountRaised = 0;
            foreach (var loan in receivedLoans)
            {
                amountRaised += loan.Amount;
            }
            ViewBag.Borrower = thisBorrower;
            ViewBag.AmountRaised = amountRaised;
            ViewBag.Loans = receivedLoans;
            return View();
        }

    }
}