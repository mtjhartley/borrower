using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using borrower.Models;

namespace borrower.Controllers
{
    public class UserController : Controller
    {
        private LoanContext _context;
        public UserController(LoanContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult RegisterLender()
        {
            ViewBag.Errors = new List<string>();
            return View();
        }

        [HttpPost]
        [Route("register_lender")]
        public IActionResult HandleRegisterLender(LenderRegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<Lender> Hasher = new PasswordHasher<Lender>();
                Lender NewLender = new Lender
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Money = model.Money
                };
                NewLender.Password = Hasher.HashPassword(NewLender, model.Password);

                _context.Add(NewLender);
                _context.SaveChanges();
                Lender justEnteredLender = _context.lenders.SingleOrDefault(lender => lender.Email == model.Email);
                HttpContext.Session.SetString("UserName", justEnteredLender.FirstName);
                HttpContext.Session.SetInt32("UserId", justEnteredLender.LenderId);

                // return RedirectToAction("Success");
                return RedirectToAction("GetLender", "Loan", new {LenderId = justEnteredLender.LenderId});
            }
            System.Console.WriteLine("Not Valid!");
            ViewBag.Errors = new List<string>();
            return View("RegisterLender");
        }
        [HttpGet]
        [Route("borrower")]
        public IActionResult RegisterBorrower()
        {
            ViewBag.Errors = new List<string>();
            return View();
        }

        [HttpPost]
        [Route("register_borrower")]
        public IActionResult HandleRegisterBorrower(BorrowerRegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                PasswordHasher<Borrower> Hasher = new PasswordHasher<Borrower>();
                Borrower NewBorrower = new Borrower
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Amount = model.Amount,
                    Reason = model.Reason,
                    Description = model.Description
                };
                NewBorrower.Password = Hasher.HashPassword(NewBorrower, model.Password);
                _context.Add(NewBorrower);
                _context.SaveChanges();
                Borrower justEnteredBorrower = _context.borrowers.SingleOrDefault(borrower => borrower.Email == model.Email);
                HttpContext.Session.SetString("UserName", justEnteredBorrower.FirstName);
                HttpContext.Session.SetInt32("UserId", justEnteredBorrower.BorrowerId);

                return RedirectToAction("Success");
            }
            System.Console.WriteLine("Not Valid!");
            ViewBag.Errors = new List<string>();
            return View("RegisterBorrower");
        }
        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();

        }
        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.Errors = new List<string>();
            return View();
        }
        [HttpPost]
        [Route("login")]
        public IActionResult HandleLogin(string Email, string PasswordToCheck)
        {
            Lender loggedUser = _context.lenders.SingleOrDefault(lender => lender.Email == Email);
            //if the person trying to log in is a lender
            if (loggedUser != null  && PasswordToCheck != null)
            {
                var Hasher = new PasswordHasher<Lender>();
                if (0 != Hasher.VerifyHashedPassword(loggedUser, loggedUser.Password, PasswordToCheck))
                {
                    HttpContext.Session.SetString("UserName", loggedUser.FirstName);
                    HttpContext.Session.SetInt32("UserId", loggedUser.LenderId);
                    return RedirectToAction("GetLender", "Loan", new {LenderId = loggedUser.LenderId}); //change this line!
                    // return RedirectToAction("Success");
                }
                else {
                    ViewBag.Errors = new List<string>();
                    ViewBag.Errors.Add("Invalid Login Credentials.");
                    return View("Login");
                }
            }
            else if (loggedUser == null)
            {
                Borrower loggedPerson = _context.borrowers.SingleOrDefault(user => user.Email == Email);
                if (loggedPerson != null && PasswordToCheck != null)
                {
                    var PassHasher = new PasswordHasher<Borrower>();
                    if (0 != PassHasher.VerifyHashedPassword(loggedPerson, loggedPerson.Password, PasswordToCheck))
                    {
                        HttpContext.Session.SetString("UserName", loggedPerson.FirstName);
                        HttpContext.Session.SetInt32("UserId", loggedPerson.BorrowerId);
                        return RedirectToAction("GetBorrower", "Loan", new {BorrowerId = loggedPerson.BorrowerId}); //change this line!
                        // return RedirectToAction("Success");
                    }
                    else {
                        ViewBag.Errors = new List<string>();
                        ViewBag.Errors.Add("Invalid Login Credentials.");
                        return View("Login");
                    }

                }

                else {
                    ViewBag.Errors = new List<string>();
                    ViewBag.Errors.Add("Invalid Login Credentials.");
                    return View("Login");
                }
            }
            else {
                    ViewBag.Errors = new List<string>();
                    ViewBag.Errors.Add("Invalid Login Credentials.");
                    return View("Login");
            }
        }
        [HttpGet]
        [Route("/logout")]
        public IActionResult logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}