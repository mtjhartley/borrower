using System;
using System.Collections.Generic;

namespace borrower.Models
{
    public class Borrower: BaseEntity
    {
        public int BorrowerId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public int Amount {get;set;}
        public string Reason {get;set;}
        public string Description {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Loan> MoneyReceived {get;set;}
        public Borrower()
        {
            MoneyReceived = new List<Loan>();
        }
    }
}