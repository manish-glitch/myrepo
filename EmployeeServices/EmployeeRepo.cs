// <copyright file="EmployeeRepo.cs" company="Enstoa">
// Copyright (c) Enstoa. All rights reserved.
// </copyright>

namespace EmployeeServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using EmployeeServices.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Employee Repo class, inherting interface IEmployeeRepo.
    /// </summary>
    public class EmployeeRepo : IEmployeeRepo
    {
        private readonly NewTestDbContext ctx;
        private readonly ILogger<EmployeeRepo>? logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepo"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        /// <param name="logger">ILogger.</param>
        public EmployeeRepo(NewTestDbContext dbContext, ILogger<EmployeeRepo> logger)
        {
            this.ctx = dbContext;
            this.logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeRepo"/> class.
        /// </summary>
        /// <param name="dbContext">DbContext.</param>
        public EmployeeRepo(NewTestDbContext dbContext)
        {
            this.ctx = dbContext;
        }

        /// <inheritdoc/>
        public async Task<bool> AddEmployee(Employee emp)
        {
            try
            {
                this.ctx.Employees.Add(emp);
                if (await this.ctx.SaveChangesAsync() == 0)
                {
                    throw new Exception("Not able to save the changes!");
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger!.LogError(ex.Message, "Error occurred while Adding employee");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteEmployee(Employee emp)
        {
            try
            {
                this.ctx.Employees.Remove(emp);
                if (await this.ctx.SaveChangesAsync() == 0)
                {
                    throw new Exception("Not able to save the changes!");
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger!.LogError(ex.Message, "Error occurred while Deleting employee");
                return false;
            }
        }

        /// <inheritdoc/>
        public async Task<Employee?> GetEmpById(int id)
        {
            Employee? emp = await this.ctx.Employees.FindAsync(id);
            if (emp != null)
            {
                return emp;
            }
            else
            {
                return null;
            }
        }

        /// <inheritdoc/>
        public async Task<List<Employee>> GetEmployeesInBatches(int index)
        {
            List<Employee> batch = await this.ctx.Employees.Skip(index).Take(20).ToListAsync();
            return batch;
        }

        /// <inheritdoc/>
        public async Task<bool> UpdateEmployee(Employee emp)
        {
            try
            {
                this.ctx.Employees.Update(emp);
                if (await this.ctx.SaveChangesAsync() == 0)
                {
                    throw new Exception("Not able to save the changes!");
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger!.LogError(ex.Message, "Error occurred while updating employee");
                return false;
            }
        }
    }
}
