using EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Data
{
    public class DbContextClass : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbContextClass(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        public DbSet<Employee> Employee { get; set; }
        public DbSet<CardApprovalLayer> CardApprovalLayer { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<EmployeeConfig> EmployeeConfig { get; set; }
        public DbSet<EmployeeData> EmployeeData { get; set; }
        public DbSet<EntityType> EntityType { get; set; }
        public DbSet<EntityValue> EntityValue { get; set; }
        public DbSet<VisitorApprovalLayer> VisitorApprovalLayer { get; set; }
    }
}
