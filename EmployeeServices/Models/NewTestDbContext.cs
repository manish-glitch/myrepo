// <copyright file="NewTestDbContext.cs" company="Enstoa">
// Copyright (c) Enstoa. All rights reserved.
// </copyright>

namespace EmployeeServices.Models
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// NewTestDbContext Class, inheriting from DbContext base class.
    /// </summary>
    public partial class NewTestDbContext : DbContext
    {
        private readonly IConfiguration? configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTestDbContext"/> class. Default Constructor.
        /// </summary>
        public NewTestDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTestDbContext"/> class.
        /// </summary>
        /// <param name="options">DbContextOptions.</param>
        public NewTestDbContext(DbContextOptions<NewTestDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewTestDbContext"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration.</param>
        public NewTestDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Gets or Sets the collection of Employees managed by the context.
        /// </summary>
        public virtual DbSet<Employee> Employees { get; set; } = null!;

        /// <inheritdoc/>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string? connectionString = this.configuration!.GetConnectionString("Database");
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(connectionString!);
            }
        }

        /// <inheritdoc/>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId);

                entity.Property(e => e.EmpName).HasMaxLength(30);

                entity.Property(e => e.Salary).HasColumnType("decimal(7, 2)");

                entity.Property(e => e.YearsOfExp).HasColumnType("decimal(3, 1)");
            });

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
