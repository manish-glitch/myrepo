// <copyright file="Program.cs" company="Enstoa">
// Copyright (c) Enstoa. All rights reserved.
// </copyright>

namespace EmployeeServices
{
    using EmployeeServices.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Program calss.
    /// </summary>
    public class Program
    {
        private static IEmployeeRepo? employeeRepo;

        /// <summary>
        /// Entry point for application.
        /// </summary>
        /// <param name="args">string[] args.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public static async Task Main(string[] args)
        {
            var serviceProvider = ConfigureServices();
            employeeRepo = serviceProvider.GetRequiredService<IEmployeeRepo>();
            bool continuer;
            do
            {
                DisplayMenu();
                string? choice = Console.ReadLine();
                continuer = await ProcessChoice(choice!);
            }
            while (continuer);
        }

        /// <summary>
        /// Support method, takes string input and provides int.
        /// </summary>
        /// <param name="inp">string input from user.</param>
        /// <returns>int or -1 if doesn't meet the criteria.</returns>
        public static int ValidateEmpIdInp(string? inp)
        {
            if (!string.IsNullOrWhiteSpace(inp))
            {
                if (int.TryParse(inp, out int id))
                {
                    return id;
                }
                else
                {
                    Console.WriteLine("Invalid Employee Id Input!");
                    return -1;
                }
            }
            else
            {
                Console.WriteLine("Input is Null");
                return -1;
            }
        }

        /// <summary>
        /// Support method, takes string input and provides decimal.
        /// </summary>
        /// <param name="inp">string input from user.</param>
        /// <returns>decimal value or -1 if doesn't meet the criteria.</returns>
        public static decimal ValidateEmpSalInp(string? inp)
        {
            if (decimal.TryParse(inp, out decimal sal))
            {
                return sal;
            }
            else if (string.IsNullOrWhiteSpace(inp))
            {
                return 2000;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Support method, takes string input and provides decimal.
        /// </summary>
        /// <param name="inp">string input from user.</param>
        /// <returns>decimal value or -1 if doesn't meet the criteria.</returns>
        public static decimal ValidateEmpExpInp(string? inp)
        {
            if (decimal.TryParse(inp, out decimal exp))
            {
                return exp;
            }
            else
            {
                Console.WriteLine("invalid input for Exp!");
                return -1;
            }
        }

        /// <summary>
        /// Support method, takes string input and validates and returns string.
        /// </summary>
        /// <param name="inp">string input from user.</param>
        /// <returns>string name or "Exit" if doesn't meet the criteria.</returns>
        public static string ValidateEmpNameInp(string? inp)
        {
            if (!string.IsNullOrWhiteSpace(inp) && inp.Length <= 30)
            {
                return inp;
            }
            else
            {
                Console.WriteLine("Invalid input! Name should not be Null or WhiteSpaces And Should not be greater in length than 30");
                return "Exit";
            }
        }

        private static void DisplayMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine();
            Console.WriteLine("1. Display records of Employees");
            Console.WriteLine("2. Display employee record by ID");
            Console.WriteLine("3. Update a chosen field of a record by Id");
            Console.WriteLine("4. Update whole record.");
            Console.WriteLine("5. Add a new Employee");
            Console.WriteLine("6. Delete a record by Id");
            Console.WriteLine("7. Exit Application");
            Console.WriteLine();
            Console.Write("Enter your choice: ");
        }

        private static async Task<bool> ProcessChoice(string choice)
        {
            Employee? emp = new ();
            bool result;
            switch (choice)
            {
                case "1":
                    await DisplayBatches();
                    break;
                case "2":
                    emp = await GetRecord();
                    if (emp != null)
                    {
                        DisplayFullRecord(emp);
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }

                    break;
                case "3":
                    emp = await GetRecord();
                    if (emp != null)
                    {
                        result = await UpdateChoosenFieldofRecord(emp);
                        if (result)
                        {
                            Console.WriteLine("Employee Updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Unable to update employee!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }

                    break;
                case "4":
                    emp = await GetRecord();
                    if (emp != null)
                    {
                        result = await UpdateWholeRecord(emp!);
                        if (result)
                        {
                            Console.WriteLine("Employee Updated successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Unable to update employee!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Employee not found.");
                    }

                    break;
                case "5":
                    await AddEmployee();
                    break;
                case "6":
                    emp = await GetRecord();
                    if (emp != null)
                    {
                        if (await employeeRepo!.DeleteEmployee(emp))
                        {
                            Console.WriteLine("Employee Deleted Successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Unable to Delete Employee");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Employee Not Found");
                    }

                    break;
                case "7":
                    return false;
                default:
                    Console.WriteLine("Invalid Input");
                    break;
            }

            Console.Write("Press \"y\" to go back to Main Menu.\nPress \"n\" to Exit the application:");
            if (Console.ReadLine() != "y")
            {
                return false;
            }
            else
            {
                Console.Clear();
                return true;
            }
        }

        private static async Task AddEmployee()
        {
            Employee emp = new ();
            Console.Write($"Enter Employee's name:");
            var name = ValidateEmpNameInp(Console.ReadLine());
            Console.Write($"Enter Employee's Experience:");
            var exp = ValidateEmpExpInp(Console.ReadLine());
            Console.Write($"Enter Employee's salary or press Enter: ");
            var sal = ValidateEmpSalInp(Console.ReadLine());

            emp.EmpName = name;
            emp.YearsOfExp = exp;
            emp.Salary = sal;

            var result = await employeeRepo!.AddEmployee(emp);
            if (result)
            {
                Console.WriteLine("Empoyee Added Successfully!");
            }
            else
            {
                Console.WriteLine("Unable to Add Employee!");
            }
        }

        private static async Task<bool> UpdateWholeRecord(Employee emp)
        {
            Console.Write($"Enter new name for {emp.EmpId}:");
            var name = ValidateEmpNameInp(Console.ReadLine());
            Console.Write($"Enter new experience for {emp.EmpId}:");
            var exp = ValidateEmpExpInp(Console.ReadLine());
            Console.Write($"Enter new sal for {emp.EmpId} or press Enter: ");
            var sal = ValidateEmpSalInp(Console.ReadLine());
            if (name != "Exit" && exp != -1 && sal != -1)
            {
                emp.EmpName = name;
                emp.YearsOfExp = exp;
                emp.Salary = sal;
                return await employeeRepo!.UpdateEmployee(emp);
            }
            else
            {
                return false;
            }
        }

        private static async Task<bool> UpdateChoosenFieldofRecord(Employee? emp)
        {
            if (emp != null)
            {
                Console.WriteLine($"Be cautioned you are updating Id:{emp.EmpId} {emp.EmpName}'s record!");
                Console.WriteLine("Fields available to update: \n1. Employee Name\n2. Years of Exp\n3. Salary");
                string inp = string.Empty;
                string? updtChoice = Console.ReadLine();

                switch (updtChoice)
                {
                    case "1":
                        Console.Write($"Enter new name for {emp.EmpId}: ");
                        inp = Console.ReadLine() !;
                        var name = ValidateEmpNameInp(inp);
                        if (name != "Exit")
                        {
                            emp.EmpName = name;
                            var result = await employeeRepo!.UpdateEmployee(emp);
                            if (result)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input! Name should not be Null or WhiteSpaces And Should not be greater in length than 30");
                            return false;
                        }

                        break;
                    case "2":
                        Console.Write($"Enter new Experience for {emp.EmpId}: ");
                        inp = Console.ReadLine() !;
                        var exp = ValidateEmpExpInp(inp);
                        if (exp != -1)
                        {
                            emp.YearsOfExp = exp;
                            var result = await employeeRepo!.UpdateEmployee(emp);
                            if (result)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invlid input for Employee's Experience");
                            return false;
                        }

                        break;
                    case "3":
                        Console.Write($"Enter new Salary for {emp.EmpId} or press enter: ");
                        inp = Console.ReadLine() !;
                        var sal = ValidateEmpSalInp(inp);
                        if (sal != -1)
                        {
                            emp.Salary = sal;
                            var result = await employeeRepo!.UpdateEmployee(emp);
                            if (result)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invlid input for Employee's Salary");
                            return false;
                        }

                        break;
                    default:
                        Console.WriteLine("invalid input!");
                        return false;
                }
            }

            return false;
        }

        private static void DisplayFullRecord(Employee emp)
        {
            Console.WriteLine("{0,-05} | {1,-30} | {2,-04} | {3,-08} |", "EmpId", "EmpName", "Exp", "Sal");
            Console.WriteLine("{0,-05} | {1,-30} | {2,-04} | {3,-08} |", emp.EmpId, emp.EmpName, emp.YearsOfExp, emp.Salary);
            Console.WriteLine();
        }

        private static async Task<Employee?> GetRecord()
        {
            Console.Write("Enter the Employee ID: ");
            string? inp = Console.ReadLine();
            var id = ValidateEmpIdInp(inp);
            var emp = await employeeRepo!.GetEmpById(id);
            if (emp != null)
            {
                return emp;
            }
            else
            {
                return null;
            }
        }

        private static async Task DisplayBatches()
        {
            int i = 0;
            bool loop = true;
            do
            {
                var batch = await employeeRepo!.GetEmployeesInBatches(i);
                Console.WriteLine("Batch : ");
                Console.WriteLine("{0,-05} | {1,-30} | {2,-04} | {3,-08} |", "EmpId", "EmpName", "Exp", "Sal");
                foreach (var emp in batch)
                {
                    Console.WriteLine("{0,-05} | {1,-30} | {2,-04} | {3,-08} |", emp.EmpId, emp.EmpName, emp.YearsOfExp, emp.Salary);
                }

                Console.WriteLine();
                if (batch.Count == 20)
                {
                    Console.Write("Do you want to print next batch? y/n: ");
                    if (Console.ReadLine() == "n")
                    {
                        break;
                    }
                    else
                    {
                        i += 20;
                    }
                }
                else
                {
                    Console.WriteLine("End of Employees!");
                    loop = false;
                }
            }
            while (loop);
        }

        private static ServiceProvider ConfigureServices()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();

            string connectionString = config.GetConnectionString("Database") ?? throw new InvalidOperationException("Database connection string is missing.");

            return new ServiceCollection().AddDbContext<NewTestDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            }).AddLogging(builder =>
            {
                builder.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.None).AddConsole();
            }).AddScoped<IEmployeeRepo, EmployeeRepo>().BuildServiceProvider();
        }
    }
}