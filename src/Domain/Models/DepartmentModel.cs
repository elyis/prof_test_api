using prof_tester_api.src.Domain.Entities.Response;

namespace prof_tester_api.src.Domain.Models
{
    public class DepartmentModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public OrganizationModel Organization { get; set; }
        public Guid OrganizationId { get; set; }

        public List<UserModel> Employees { get; set; } = new();
        public List<TestModel> Tests { get; set; } = new();

        public DepartmentBody ToDepartmentBody()
        {
            return new DepartmentBody
            {
                Id = Id,
                Name = Name
            };
        }

        public DepartmentEmployeesBody ToDepartmentEmployeesBody()
        {
            return new DepartmentEmployeesBody
            {
                Department = ToDepartmentBody(),
                EmployeesFullnames = Employees.Select(e => e.Fullname).ToList()
            };
        }
    }
}