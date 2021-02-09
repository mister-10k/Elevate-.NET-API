using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Business
{
    public class EmployeeBL : IEmployeeBL
    {
        private readonly IEmployeeDL employeeDL;
        public EmployeeBL(IEmployeeDL employeeDL)
        {
            this.employeeDL = employeeDL;
        }
        public EmployeeDTO CreateEmployee(EmployeeDTO employeeDTO)
        {
            return employeeDL.CreateEmployee(employeeDTO);
        }
        public EmployeeDTO GetEmployee(int id)
        {
            return employeeDL.GetEmployee(id);
        }

        public EmployeeDTO UpdateEmployee(EmployeeDTO employeeDTO)
        {
            return employeeDL.UpdateEmployee(employeeDTO);
        }

        public EmployeeDTO DeleteEmployee(int id)
        {
            return employeeDL.DeleteEmployee(id);
        }
    }
}
