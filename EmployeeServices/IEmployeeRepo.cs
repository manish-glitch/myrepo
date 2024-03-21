// <copyright file="IEmployeeRepo.cs" company="Enstoa">
// Copyright (c) Enstoa. All rights reserved.
// </copyright>

namespace EmployeeServices
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EmployeeServices.Models;

    /// <summary>
    /// Employee Repository Interface.
    /// </summary>
    public interface IEmployeeRepo
    {
        /// <summary>
        /// Get all Employees in Batch of 20 each.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result contains list of 20 employees.</returns>
        Task<List<Employee>> GetEmployeesInBatches(int index);

        /// <summary>
        /// Get Employee By Id.
        /// </summary>
        /// <param name="id">int Id.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result contains the employee object if found, or null if not found.</returns>
        Task<Employee?> GetEmpById(int id);

        /// <summary>
        /// Updates the employee object.
        /// </summary>
        /// <param name="emp">Employee Object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result contains a boolean value indicating whether the employee was successfully updated.</returns>
        Task<bool> UpdateEmployee(Employee emp);

        /// <summary>
        /// Removes provided Employee.
        /// </summary>
        /// <param name="emp">Employee Object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result contains a boolean value indicating whether the employee was successfully deleted.</returns>
        Task<bool> DeleteEmployee(Employee emp);

        /// <summary>
        /// Adds provided Employee Object.
        /// </summary>
        /// <param name="emp">Employee Object.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation. The task result contains a boolean value indicating whether the employee was successfully added.</returns>
        Task<bool> AddEmployee(Employee emp);
    }
}
