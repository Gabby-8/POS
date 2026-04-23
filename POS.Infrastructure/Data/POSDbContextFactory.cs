using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Infrastructure.Data
{
    public class POSDbContextFactory : IDesignTimeDbContextFactory<POSDbContext>
    {
        public POSDbContext CreateDbContext(string[] args) 
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../POS.Server");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<POSDbContext>();

            optionsBuilder.UseSqlServer(
                config.GetConnectionString("POS"));

            return new POSDbContext(optionsBuilder.Options);
        }
    }
}
