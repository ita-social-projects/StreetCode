
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class StreetcodeDBContext : DbContext
    {

        public StreetcodeDBContext(DbContextOptions<StreetcodeDBContext> options) : base(options)
        {
        }

        public void All_DBSets() {
            // TODO implement here
        }

        protected void OnModelCreating() {
            // TODO implement here
        }

        internal void SaveChanges()
        {
            throw new NotImplementedException();
        }

        internal Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}