using Microsoft.EntityFrameworkCore;
using VisitorManagement.Models;

namespace VisitorManagement.Data
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
            var dbName = Configuration["UseDatabase"];
            if(dbName != null && dbName=="MySql")
            {
                var conMysql = Configuration.GetConnectionString("DefaultConnectionMySql");
                options.UseMySql(Configuration.GetConnectionString("DefaultConnectionMySql"), ServerVersion.AutoDetect(conMysql));
            }
            else
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            }
          
            
        }
       
        public DbSet<Visitor> Visitor { get; set; }
        public DbSet<VisitorConfiguration> VisitorConfiguration { get; set; }
        public DbSet<VisitorApplication> VisitorApplication { get; set; }
        public DbSet<VisitorData> VisitorData { get; set; }
        public DbSet<BlackListVisitor>BlackListVisitor { get; set; }
    }
}
