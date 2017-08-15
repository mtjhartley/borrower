using System;
using System.Collections.Generic;

namespace borrower.Models
{
    public class Lender: BaseEntity
    {
        public int LenderId {get;set;}
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public string Email {get;set;}
        public string Password {get;set;}
        public int Money {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Loan> Loans {get;set;}
        public Lender()
        {
            Loans = new List<Loan>();
        }
    }
}