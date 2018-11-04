namespace wcfService1.Database
{
    using System.Data.Entity;

    public partial class NorthwndDBContext : DbContext
    {
        public NorthwndDBContext()
            : base("name=NorthwndDBContext")
        {
        }

        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employees> Employees { get; set; }
        public DbSet<CustOrderHist> CustOrderHist { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>()
                .Property(e => e.CustomerID)
                .IsFixedLength();

            modelBuilder.Entity<Employees>()
                .HasMany(e => e.Employees1)
                .WithOptional(e => e.Employees2)
                .HasForeignKey(e => e.ReportsTo);
        }
    }
}
