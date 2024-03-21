// <copyright file="Employee.cs" company="Enstoa">
// Copyright (c) Enstoa. All rights reserved.
// </copyright>

namespace EmployeeServices.Models
{
    /// <summary>
    /// Employee Class.
    /// </summary>
    public partial class Employee
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class. Default constructor.
        /// </summary>
        public Employee()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Employee"/> class.
        /// </summary>
        /// <param name="name">string name.</param>
        /// <param name="exp">decimal experience.</param>
        /// <param name="sal">decimal salary.</param>
        public Employee(string name, decimal exp, decimal sal)
        {
            this.EmpName = name;
            this.YearsOfExp = exp;
            this.Salary = sal;
        }

        /// <summary>
        /// Gets or Sets Employee Id property.
        /// </summary>
        public int EmpId { get; set; }

        /// <summary>
        /// Gets or Sets Employee Name Property.
        /// </summary>
        public string EmpName { get; set; } = null!;

        /// <summary>
        /// Gets or Sets Employee Experience/ years of experience property.
        /// </summary>
        public decimal YearsOfExp { get; set; }

        /// <summary>
        /// Gets or Sets Employee Salary property.
        /// </summary>
        public decimal Salary { get; set; }
    }
}
