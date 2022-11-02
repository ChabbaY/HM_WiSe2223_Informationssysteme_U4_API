using API.DataObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Store {

    /// <summary>
    /// Database context that allows us to specify the tables of a database
    /// </summary>
    public class Context : DbContext {

        public Context(DbContextOptions<Context> options) : base(options) {

        }


        protected override void OnModelCreating(ModelBuilder builder) {
            //additional settings for certain entity options


        }

        /// <summary>
        /// database table Customers
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

    }


}
