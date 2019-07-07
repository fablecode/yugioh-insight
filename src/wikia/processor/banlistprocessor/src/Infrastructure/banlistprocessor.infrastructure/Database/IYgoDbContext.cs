﻿using banlistprocessor.core.Models.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace banlistprocessor.infrastructure.Database
{
    public interface IYgoDbContext
    {
        DbSet<Category> Category { get; set; }
        DatabaseFacade Database { get; }
        int SaveChanges();
    }
}