using Elevate.Entities;
using Elevate.Shared;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elevate.Data
{
    public class EmployeeDL : IEmployeeDL
    {
        public async Task<EmployeeDTO> CreateEmployeeAsync(EmployeeDTO employee)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var createdAt = DateTime.Now;
                    var employeeUserType = await dbContext.UserTypes.FirstOrDefaultAsync(x => x.Name == AppConstants.UserType.Employee && x.IsActive == true);
                    if (employeeUserType != null)
                    {
                        var newEmployee = new User
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            CompanyId = employee.CompanyId,
                            UserTypeId = employeeUserType.ID,
                            CreatedAt = createdAt,
                            IsActive = true
                        };

                        dbContext.Users.Add(newEmployee);
                        var employeeSaved = (await dbContext.SaveChangesAsync()) > 0;
                        var dependentsSaved = false;

                        if (employeeSaved)
                            dependentsSaved = await AddEmployeeDependents(dbContext, employee.Dependents, newEmployee.ID);


                        if (employeeSaved && dependentsSaved)
                        {
                            employee.Id = newEmployee.ID;
                            employee.CreatedAt = createdAt;
                            var company = await dbContext.Companies.FirstOrDefaultAsync(x => x.ID == newEmployee.CompanyId);
                            if (company != null)
                                employee.CompanyDisplayName = company.DisplayName;

                            ret = employee;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: CreateEmployeeAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        private async Task<bool> AddEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentDTO> dependents, int employeeId)
        {
            foreach (var dependent in dependents)
            {
                var createdAt = DateTime.Now;
                var dbDependent = new EmployeeDependent
                {
                    EmployeeId = employeeId,
                    FirstName = dependent.FirstName,
                    LastName = dependent.LastName,
                    RelationshipId = dependent.RelationshipId,
                    CreatedAt = createdAt,
                    IsActive = true
                };

                dbContext.EmployeeDependents.Add(dbDependent);
                await dbContext.SaveChangesAsync();

                dependent.Id = dbDependent.ID;
                dependent.EmployeeId = employeeId;
                var relationship = await dbContext.Relationships.FirstOrDefaultAsync(x => x.ID == dependent.RelationshipId);
                if (relationship != null)
                    dependent.RelationshipDisplayName = relationship.DisplayName;
                dependent.CreatedAt = createdAt;            
            }

            return true;
        }

        public async Task<EmployeeDTO> GetEmployeeAsync(int employeeId)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeId && x.IsActive == true);
                    if (employee != null)
                    {
                        ret = new EmployeeDTO
                        {
                            Id = employee.ID,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
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
                Console.WriteLine("Employee Data Layer: GetEmployeeAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        public async Task<List<EmployeeDTO>> GetAllEmployeesForComapnyAsync(int companyId)
        {
            List<EmployeeDTO> ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {

                    var employeeUserType = await dbContext.UserTypes.FirstOrDefaultAsync(x => x.Name == AppConstants.UserType.Employee && x.IsActive == true);
                    if (employeeUserType != null)
                    {
                        ret = await (from U in dbContext.Users
                               where U.IsActive == true && U.UserTypeId == employeeUserType.ID && U.CompanyId==companyId
                               select new EmployeeDTO
                               {
                                   FirstName = U.FirstName,
                                   LastName = U.LastName,
                                   Email = U.Email,
                                   CompanyId = U.CompanyId,
                                   CreatedAt = U.CreatedAt,
                                   Dependents = U.EmployeeDependents.Select(x => new EmployeeDependentDTO
                                   {
                                       Id = x.ID,
                                       EmployeeId = x.EmployeeId,
                                       FirstName = x.FirstName,
                                       LastName = x.LastName,
                                       RelationshipId = x.RelationshipId,
                                       CreatedAt = x.CreatedAt,
                                       ModifiedAt = x.ModifiedAt
                                   }).ToList()
                               }).ToListAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: GetAllEmployeesForComapnyAsync Exception Msg", ex.Message);
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

        public async Task<EmployeeDTO> UpdateEmployeeAsync(EmployeeDTO employeeDTO)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var modifiedAt = DateTime.Now;
                    var e = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeDTO.Id && x.IsActive == true);
                    if (e != null)
                    {
                        UpdateEmployeeDependents(dbContext, employeeDTO.Dependents, employeeDTO.Id);
                        e.FirstName = employeeDTO.FirstName;
                        e.LastName = employeeDTO.LastName;
                        e.Email = employeeDTO.Email;
                        e.CompanyId = employeeDTO.CompanyId;
                        e.ModifiedAt = modifiedAt;
                    }

                    if ((await dbContext.SaveChangesAsync()) > 0)
                    {
                        employeeDTO.ModifiedAt = modifiedAt;
                        ret = employeeDTO;
                        ret.CreatedAt = e.CreatedAt;
                        ret.CompanyDisplayName = e.Company.DisplayName;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: UpdateEmployeeAsync Exception Msg", ex.Message);
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

        public async Task<EmployeeDTO> DeleteEmployeeAsync(int employeeId)
        {
            EmployeeDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeId && x.IsActive == true);
                    if (employee != null)
                    {
                        ret = new EmployeeDTO { Dependents = new List<EmployeeDependentDTO>() };
                        GetEmployeeDependents(dbContext, ret.Dependents, employeeId);

                        employee.IsActive = false;
                        DeleteEmployeeAsyncDependents(dbContext, employeeId);

                        if ((await dbContext.SaveChangesAsync()) > 0)
                        {
                            ret.Id = employee.ID;
                            ret.Email = employee.Email;
                            ret.FirstName = employee.FirstName;
                            ret.LastName = employee.LastName;
                            ret.CompanyId = employee.CompanyId;
                            ret.CompanyDisplayName = employee.Company.DisplayName;
                            ret.CreatedAt = employee.CreatedAt;
                            ret.ModifiedAt = employee.ModifiedAt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: DeleteEmployeeAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        private void DeleteEmployeeAsyncDependents(ElevateEntities dbContext, int employeeId)
        {
            var dependents = dbContext.EmployeeDependents.Where(x => x.EmployeeId == employeeId && x.IsActive == true);
            foreach(var dependent in dependents)
            {
                dependent.IsActive = false;
            }
        }

        public async Task<TableDTO<EmployeeDTO>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestDTO requestModel)
        {
            TableDTO<EmployeeDTO> ret = new TableDTO<EmployeeDTO> { Rows = new List<EmployeeDTO>() };

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    List<GetEmployeesForEBDashboard_Result> data = new List<GetEmployeesForEBDashboard_Result>();
                    Task<List<GetEmployeesForEBDashboard_Result>> task = new Task<List<GetEmployeesForEBDashboard_Result>>(() => {
                        return dbContext.GetEmployeesForEBDashboard(
                                    requestModel.CompanyId,
                                    requestModel.SearchText,
                                    requestModel.SortBy,
                                    requestModel.SortColumn,
                                    requestModel.PageSize,
                                    requestModel.PageNumber
                               ).ToList();
                    });
                    task.Start();
                    data = await task;

                    if (data.Count > 0)
                    {
                        var rows = data.Select(employee => new EmployeeDTO
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            CompanyId = employee.CompanyId,
                            CompanyDisplayName = employee.CompanyDisplayName,
                            Dependents = new List<EmployeeDependentDTO>(),
                            CreatedAt = employee.CreatedAt
                        }).ToList();

                        GetEmployeesDependents(dbContext, rows);

                        ret.Rows = rows;
                        ret.TotalCount = data[0].TotalCount ?? 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: GetEmployeesForEBDashboardAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        private void GetEmployeesDependents(ElevateEntities dbContext, List<EmployeeDTO> employees)
        {
            var employeeIds = employees.Select(x => x.Id).ToList();

            var dependents = (from ED in dbContext.EmployeeDependents
                              join employeeId in employeeIds on ED.EmployeeId equals employeeId
                              join R in dbContext.Relationships on ED.RelationshipId equals R.ID
                              where ED.IsActive && R.IsActive
                              select new EmployeeDependentDTO
                              {
                                  Id = ED.ID,
                                  EmployeeId = ED.EmployeeId,
                                  FirstName = ED.FirstName,
                                  LastName = ED.LastName,
                                  RelationshipId = ED.RelationshipId,
                                  RelationshipName = R.Name,
                                  RelationshipDisplayName = R.DisplayName,
                                  CreatedAt = ED.CreatedAt
                              }).GroupBy(x =>x.EmployeeId).ToList();

            foreach(var group in dependents)
            {
                var employee = employees.FirstOrDefault(x => x.Id == group.Key);
                employee.Dependents = group.ToList();
            }
        }

        public async Task<EmployeeFormMasterDataDTO> GetEmployeeFormMasterDataAsync()
        {
            EmployeeFormMasterDataDTO ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new EmployeeFormMasterDataDTO
                    {
                        Relationships = await GetRelationshipsForEmployeeFormAsync(dbContext)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: GetEmployeeFormMasterDataAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        private  async Task<List<RelationshipDTO>> GetRelationshipsForEmployeeFormAsync(ElevateEntities dbContext)
        {
            return await (from R in dbContext.Relationships
                          where R.IsActive
                          select new RelationshipDTO
                          {
                              Id = R.ID,
                              Name = R.Name,
                              DisplayName = R.DisplayName,
                              CreatedAt = R.CreatedAt,
                              ModifiedAt = R.ModifiedAt,
                              IsActive = R.IsActive
                          }).ToListAsync();
        }
    }
}
