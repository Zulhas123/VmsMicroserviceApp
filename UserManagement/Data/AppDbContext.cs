using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data
{
    public class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var dbName = Configuration["UseDatabase"];
            if (dbName != null && dbName == "MySql")
            {
                var conMysql = Configuration.GetConnectionString("DefaultConnectionMySql");
                options.UseMySql(Configuration.GetConnectionString("DefaultConnectionMySql"), ServerVersion.AutoDetect(conMysql));
            }
            else
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));

            }


        }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Submenu> Submenus { get; set; }
        public DbSet<ForgetPassword> ForgetPassword { get; set; }
    }
}
