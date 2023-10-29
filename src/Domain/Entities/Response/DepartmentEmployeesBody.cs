namespace prof_tester_api.src.Domain.Entities.Response
{
    public class DepartmentEmployeesBody
    {
        public DepartmentBody Department { get; set; }
        public List<string> EmployeesFullnames { get; set; }
    }
}