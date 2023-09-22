using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace dotnetapiapp.Models;

public class JobApplicationDbContext : DbContext
{

    public JobApplicationDbContext(DbContextOptions<JobApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<Job> Jobs { get; set; }
    public virtual DbSet<Application> Applications { get; set; }
}
