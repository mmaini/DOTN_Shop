﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DOTN_DataAccess.Data
{
	public class AppDbContext : DbContext
	{		
		public AppDbContext(DbContextOptions<AppDbContext> options): base(options) { }

		public DbSet<Category> Categories { get; set; }
	}
}
