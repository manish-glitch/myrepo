namespace EmployeeServices.Tests
{
	using EmployeeServices.Models;
	using Microsoft.EntityFrameworkCore;
	public class EmployeeServicesTests
	{

		[Theory]
		[InlineData(0)]
		[InlineData(20)]
		public async Task GetEmployeesInBatches_ReturnCorrectNumberOfEmployees(int index)
		{
			// Arrange
			var _empRepo = GetEmployeeMethodsSqlServerDb();

			// Act
			var batch = await _empRepo.GetEmployeesInBatches(index);

			// Assert
			Assert.NotNull(batch);
		}

		[Fact]
		public async Task GetEmpById_ReturnsEmployee()
		{
			// Arrange
			var empRepo = GetEmployeeMethodsInMemoryDb();

			// Act
			var emp = await empRepo.GetEmpById(1);

			// Assert
			Assert.NotNull(emp);
			Assert.True(emp.EmpName == "test Manish" && emp.EmpId == 1);
		}

		[Fact]
		public async Task GetEmpById_ReturnsNullAndPrintsMessage()
		{
			// Arrange
			var empRepo = GetEmployeeMethodsInMemoryDb();

			// Act
			Employee emp = await empRepo.GetEmpById(500);

			// Assert
			Assert.Null(emp);
		}

		[Theory]
		[InlineData(30, "Manish Testing", 2.2, 6000)]
		public async Task UpdateEmployee_ReturnsTrue(int id, string name, decimal exp, decimal sal)
		{
			// Arrange
			var empRepo = GetEmployeeMethodsSqlServerDb();

			// Act
			var emp = await empRepo.GetEmpById(id);
			emp.EmpName = name;
			emp.YearsOfExp = exp;
			emp.Salary = sal;

			// Assert
			Assert.True(await empRepo.UpdateEmployee(emp));

		}

		[Theory]
		[InlineData("Testing Manish", 5.6, 78945)]
		[InlineData("Testing Manish2", 6.5, 12345)]
		[InlineData("Testing Manish3", 2.2, 75319)]
		public async Task AddEmployee_ReturnsTrue(string name, decimal exp, decimal sal)
		{
			// Arrange
			var empRepo = GetEmployeeMethodsSqlServerDb();

			// Act
			Employee emp = new (name, exp, sal);

			// Assert
			Assert.True(await empRepo.AddEmployee(emp));
		}

		[Theory]
		[InlineData(1)]
		public async Task DeleteEmployee_ReturnsTrue(int id)
		{
			// Arrange
			var empRepo = GetEmployeeMethodsInMemoryDb();

			// Act
			var emp = await empRepo.GetEmpById(id);

			// Assert
			Assert.True(await empRepo.DeleteEmployee(emp));
		}

		[Theory]
		[InlineData("1", 1)]
		[InlineData("2", 2)]
		[InlineData("780", 780)]
		public void ValidateEmpIdInp_ReturnsInt(string inp, int expected)
		{
			// Act
			var actual = Program.ValidateEmpIdInp(inp);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("1a")]
		[InlineData("")]
		[InlineData("as")]
		public void ValidateEmpIdInp_ReturnsNegVal(string inp)
		{
			// Act
			var actual = Program.ValidateEmpIdInp(inp);

			// Assert
			Assert.Equal(-1,  actual);
		}

		[Theory]
		[InlineData("Manish","Manish")]
		[InlineData("Giftson","Giftson")]
		[InlineData("Manish Shingare","Manish Shingare")]
		public void ValidateEmpNameInp_ReturnsNameString(string inp, string expected)
		{
			// Act
			var actual = Program.ValidateEmpNameInp(inp);

			// Assert
			Assert.Equal(expected,actual);
		}

		[Theory]
		[InlineData("Manishdjdiusuhfiuwfhwiufwhfiwfhwifuwhwhjoifwejfewincacajwefwfh")]
		[InlineData("")]
		[InlineData("  ")]
		public void ValidateEmpNameInp_ReturnsExit(string inp)
		{
			// Act
			var actual = Program.ValidateEmpNameInp(inp);

			// Assert
			Assert.Equal("Exit", actual);
		}

		[Theory]
		[InlineData("1.2",1.2)]
		[InlineData("2.9", 2.9)]
		[InlineData("9.9", 9.9)]
		public void ValidateEmpExpInp_ReturnsDecimal(string inp, decimal expected)
		{
			// Act
			var actual = Program.ValidateEmpExpInp(inp);

			// Assert
			Assert.Equal(expected, actual);
		}

		[Theory]
		[InlineData("1.2as")]
		[InlineData("")]
		[InlineData(" ")]
		public void ValidateEmpExpInp_ReturnsNegVal(string inp)
		{
			// Act
			var actual = Program.ValidateEmpExpInp(inp);

			// Assert
			Assert.Equal(-1, actual);
		}

		[Theory]
		[InlineData("7894",7894)]
		[InlineData("56000", 56000)]
		[InlineData("12000", 12000)]
		public void ValidateEmpSalInp_ReturnsDeciaml(string inp, decimal expected)
		{
			// Act
			var actual = Program.ValidateEmpSalInp(inp);

			// Assert
			Assert.Equal(expected,actual);
		}

		[Theory]
		[InlineData("7894mm")]
		[InlineData("masnas")]
		[InlineData("11.200.0")]
		public void ValidateEmpSalInp_ReturnsNegVal(string inp)
		{
			// Act
			var actual = Program.ValidateEmpSalInp(inp);

			// Assert
			Assert.Equal(-1, actual);
		}

		[Theory]
		[InlineData("")]
		[InlineData(" ")]
		public void ValidateEmpSalInp_Returns2000(string inp)
		{
			// Act
			var actual = Program.ValidateEmpSalInp(inp);

			// Assert
			Assert.Equal(2000, actual);
		}

		private static EmployeeRepo GetEmployeeMethodsSqlServerDb()
		{
			var options = new DbContextOptionsBuilder<NewTestDbContext>().UseSqlServer("Data Source=.;Initial Catalog=NewTestDb;Integrated Security=True;").Options;
			NewTestDbContext ctx = new (options);

			// Create instance of EmployeeMethods class by passing ctx
			EmployeeRepo empMethods = new (ctx);

			return empMethods;
		}
		private static EmployeeRepo GetEmployeeMethodsInMemoryDb()
		{
			var options = new DbContextOptionsBuilder<NewTestDbContext>().UseInMemoryDatabase(databaseName: "TestDb").Options;
			NewTestDbContext ctx = new (options);

			// seed test data to testDb
			AddTestData(ctx);

			// create instance of EmployeeMethods class by passing the context
			EmployeeRepo empMethods = new (ctx);

			return empMethods;
		}
		private static void AddTestData(NewTestDbContext context)
		{
			var employees = new List<Employee>()
			{
				new Employee { EmpId = 1, EmpName = "test Manish", YearsOfExp = 5.5m, Salary = 23000 },
				new Employee { EmpId = 2, EmpName = "test Manish2", YearsOfExp = 3.5m, Salary = 95870 },
				new Employee { EmpId = 3, EmpName = "test Manish3", YearsOfExp = 1.5m, Salary = 12050 },
				new Employee { EmpId = 4, EmpName = "test Manish4", YearsOfExp = 4.7m, Salary = 56000 },
				new Employee { EmpId = 5, EmpName = "test Manish5", YearsOfExp = 3.2m, Salary = 47000 },
				new Employee { EmpId = 6, EmpName = "test Manish6", YearsOfExp = 3.3m, Salary = 9800 },
				new Employee { EmpId = 7, EmpName = "test Manish7", YearsOfExp = 7.5m, Salary = 55600 },
			};

			context.Employees.AddRange(employees);
			context.SaveChanges();
		}
	}
}