using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UCPPRAKTIKUM.Models;

namespace UCPPRAKTIKUM.Data
{
    public class UCPPRAKTIKUMContext : DbContext
    {
        public UCPPRAKTIKUMContext (DbContextOptions<UCPPRAKTIKUMContext> options)
            : base(options)
        {
        }

        public DbSet<UCPPRAKTIKUM.Models.BookVieModel> BookVieModel { get; set; }
    }
}
