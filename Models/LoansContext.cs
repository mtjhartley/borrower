using Microsoft.EntityFrameworkCore;

namespace borrower.Models
{
    public class LoanContext: DbContext
    {
        public LoanContext(DbContextOptions<LoanContext> options) : base(options) { }

        public DbSet<Lender> lenders {get;set;}
        public DbSet<Borrower> borrowers {get;set;}
        public DbSet<Loan> loans {get;set;}
    }
}