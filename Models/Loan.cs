using System;
using System.Collections.Generic;

namespace borrower.Models
{
    public class Loan: BaseEntity
    {
        public int LoanId {get;set;}
        public int Amount {get;set;}
        public int LenderId {get;set;}
        public Lender Lender {get;set;}
        public int BorrowerId {get;set;}
        public Borrower Borrower {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}

    }
}