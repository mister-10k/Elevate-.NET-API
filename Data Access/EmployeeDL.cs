using Elevate.Entities;
using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Data
{
    public class EmployeeDL : IEmployeeDL
    {
        public EmployeeDTO CreateEmployee(EmployeeDTO employeeDTO)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var createdAt = DateTime.Now;
                    var employeeUserType = dbContext.UserTypes.FirstOrDefault(x => x.Name == AppConstants.UserType.Employee);
                    if (employeeUserType != null)
                    {
                        var newEmployee = new User
                        {
                            FirstName = employeeDTO.FirstName,
                            LastName = employeeDTO.LastName,
                            Email = employeeDTO.Email,
                            CompanyId = employeeDTO.CompanyId,
                            UserTypeId = employeeUserType.ID,
                            CreatedAt = createdAt,
                            IsActive = true
                        };

                        dbContext.Users.Add(newEmployee);
                        var employeeSaved = dbContext.SaveChanges() > 0;

                        if (employeeSaved)
                            AddEmployeeDependents(dbContext, employeeDTO.Dependents, newEmployee.ID);

                        var dependentsSaved = employeeSaved;

                        if (employeeSaved && (dbContext.SaveChanges() > 0 || employeeDTO.Dependents.Count == 0))
                        {
                            employeeDTO.Id = newEmployee.ID;
                            employeeDTO.CreatedAt = createdAt;
                            ret = employeeDTO;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: CreateEmployee Exception Msg", ex.Message);
            }

            return ret;
        }

        private void AddEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentDTO> dependents, int employeeId)
        {
            foreach (var dependent in dependents)
            {
                var newDependent = new EmployeeDependent
                {
                    EmployeeId = employeeId,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    RelationshipId = dependent.RelationshipId,
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };

                dbContext.EmployeeDependents.Add(newDependent);
            }
        }

        public EmployeeDTO GetEmployee(int employeeId)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = dbContext.Users.FirstOrDefault(x => x.ID == employeeId);
                    if (employee != null)
                    {
                        ret = new EmployeeDTO
                        {
                            Id = employee.ID,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            CompanyName = employee.Company.Name,
                            CompanyDisplayName = employee.Company.DisplayName,
                            CompanyId = employee.CompanyId,
                            CreatedAt = employee.CreatedAt,
                            ModifiedAt = employee.ModifiedAt,
                            Dependents = new List<EmployeeDependentDTO>()
                        };
                        GetEmployeeDependents(dbContext, ret.Dependents, employeeId);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: GetEmployee Exception Msg", ex.Message);
            }

            return ret;
        }

        private void GetEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentDTO> dependents, int employeeId)
        {
            var dbDependents = dbContext.EmployeeDependents.Where(x => x.EmployeeId == employeeId && x.IsActive);

            foreach(var dbDependent in dbDependents)
            {
                var d = new EmployeeDependentDTO
                {
                    Id = dbDependent.ID,
                    EmployeeId = dbDependent.EmployeeId,
                    FirstName = dbDependent.FirstName,
                    LastName = dbDependent.LastName,
                    RelationshipId = dbDependent.RelationshipId,
                    CreatedAt = dbDependent.CreatedAt,
                    ModifiedAt = dbDependent.ModifiedAt
                };
                dependents.Add(d);
            }
        }

        public EmployeeDTO UpdateEmployee(EmployeeDTO employeeDTO)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var modifiedAt = DateTime.Now;
                    var employee = dbContext.Users.FirstOrDefault(x => x.ID == employeeDTO.Id);
                    if (employee != null)
                    {
                        UpdateEmployeeDependents(dbContext, employeeDTO.Dependents, employeeDTO.Id);
                        employee.FirstName = employeeDTO.FirstName;
                        employee.LastName = employeeDTO.LastName;
                        employee.CompanyId = employeeDTO.CompanyId;
                        employee.ModifiedAt = modifiedAt;
                    }

                    if (dbContext.SaveChanges() > 0)
                    {
                        employeeDTO.ModifiedAt = modifiedAt;
                        ret = employeeDTO;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: UpdateEmployee Exception Msg", ex.Message);
            }

            return ret;
        }

        private void UpdateEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentDTO> dependents, int employeeId)
        {
            var dbDependents = dbContext.EmployeeDependents.Where(x => x.EmployeeId == employeeId && x.IsActive);

            // check for deletes
            foreach(var dbDependent in dbDependents)
            {
                if (dependents.FirstOrDefault(x => x.Id == dbDependent.ID) == null)
                {
                    dbDependent.IsActive = false;
                    dbDependent.ModifiedAt = DateTime.Now;
                }
            }

            foreach (var dependent in dependents)
            {
                if (dependent.Id == 0)
                {
                    // add
                    var newDependent = new EmployeeDependent
                    {
                        EmployeeId = employeeId,
                        FirstName = dependent.FirstName,
                        LastName = dependent.LastName,
                        RelationshipId = dependent.RelationshipId,
                        CreatedAt = DateTime.Now,
                        IsActive = true
                    };

                    dbContext.EmployeeDependents.Add(newDependent);
                }
                else
                {
                    // update
                    var dbDependent = dbDependents.FirstOrDefault(x => x.ID == dependent.Id);
                    if (dbDependent != null)
                    {
                        dbDependent.FirstName = dependent.FirstName;
                        dbDependent.LastName = dependent.LastName;
                        dbDependent.RelationshipId = dependent.RelationshipId;
                        dbDependent.ModifiedAt = DateTime.Now;
                    }
                }
            }
        }

        public EmployeeDTO DeleteEmployee(int employeeId)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = dbContext.Users.FirstOrDefault(x => x.ID == employeeId);
                    if (employee != null)
                    {
                        ret = new EmployeeDTO { Dependents = new List<EmployeeDependentDTO>() };
                        GetEmployeeDependents(dbContext, ret.Dependents, employeeId);

                        employee.IsActive = false;
                        DeleteEmployeeDependents(dbContext, employeeId);

                        if (dbContext.SaveChanges() > 0)
                        {
                            ret.FirstName = employee.FirstName;
                            ret.LastName = employee.LastName;
                            ret.CompanyId = employee.CompanyId;
                            ret.CreatedAt = employee.CreatedAt;
                            ret.ModifiedAt = employee.ModifiedAt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: DeleteEmployee Exception Msg", ex.Message);
            }

            return ret;
        }

        private void DeleteEmployeeDependents(ElevateEntities dbContext, int employeeId)
        {
            var dependents = dbContext.EmployeeDependents.Where(x => x.EmployeeId == employeeId);
            foreach(var dependent in dependents)
            {
                dependent.IsActive = false;
            }
        }

        public EBDashbaordStatsDTO GetEBDashboardCardsData(int companyId)
        {
            var ret = new EBDashbaordStatsDTO();

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret.Employees = new List<EmployeeDTO>();
                    var employees = dbContext.Users.Where(x => x.CompanyId == companyId && x.UserType.Name == AppConstants.UserType.Employee);
                    foreach(var employee in employees)
                    {
                        var employeeDTO = new EmployeeDTO
                        {
                            Id = employee.ID,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            CompanyId = employee.CompanyId,
                            CompanyName = employee.Company.Name,
                            CompanyDisplayName = employee.Company.DisplayName,
                            CreatedAt = employee.CreatedAt,
                            ModifiedAt = employee.ModifiedAt,
                            Dependents = new List<EmployeeDependentDTO>()                       
                        };
                        GetEmployeeDependents(dbContext, employeeDTO.Dependents, employee.ID);
                        ret.Employees.Add(employeeDTO);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: GetEBDashboardCardsData Exception Msg", ex.Message);
            }

            return ret;
        }

        public List<EBEmployeeListDTO> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            List<EBEmployeeListDTO> ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var data = dbContext.GetEmployeesForEBDashboard(
                        requestModel.CompanyId,
                        requestModel.SearchText,
                        requestModel.SortBy,
                        requestModel.SortColumn,
                        requestModel.PageSize,
                        requestModel.PageNumber
                    ).ToList();

                    if (data.Count > 0)
                    {
                        ret = data.Select(employee => new EBEmployeeListDTO
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Company = employee.Company,
                            Dependents = employee.Dependents ?? 0,
                            CreatedAt = employee.CreatedAt,
                            TotalCount = employee.TotalCount ?? 0
                        }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: GetEmployeesForEBDashboard Exception Msg", ex.Message);
            }

            return ret;
        }
    }
}
