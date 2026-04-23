using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using POS.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//dotnet ef migrations add InitialIdentity --project POS.Infrastructure --startup-project POS.Server
//dotnet ef database update --project POS.Infrastructure --startup-project POS.Server

namespace POS.Infrastructure.Data
{
    public class POSDbContext : IdentityDbContext<ApplicationUser>
    {
        public POSDbContext(DbContextOptions<POSDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
