﻿using System;
using System.Collections.Generic;
using System.Text;
using TanoApp.Data.EF.EF;
using TanoApp.Data.Entities;
using TanoApp.Data.IRepositories;

namespace TanoApp.Data.EF.Repositories
{
    public class ColorRepository: EFRepository<Color, int>, IColorRepository
    {
        private AppDbContext _context;
        public ColorRepository(AppDbContext dbContext): base(dbContext)
        {
            _context = dbContext;
        }
    }
}
