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
        public async Task<EmployeeModel> CreateEmployeeAsync(EmployeeModel employee)
        {
            EmployeeModel ret = null;

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

                        if (employeeSaved)
                            AddEmployeeDependents(dbContext, employee.Dependents, newEmployee.ID);
                        var depndentsSaved = (await dbContext.SaveChangesAsync() > 0);

                        if (employeeSaved && (depndentsSaved || employee.Dependents.Count == 0))
                        {
                            employee.Id = newEmployee.ID;
                            employee.CreatedAt = createdAt;
                            employee.CreatedAtText = employee.CreatedAt != null && employee.CreatedAt.HasValue ? employee.CreatedAt.Value.ToString("MM/dd/yyy") : string.Empty;
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

        private void AddEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentModel> dependents, int employeeId)
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

        public async Task<EmployeeModel> GetEmployeeAsync(int employeeId)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeId && x.IsActive == true);
                    if (employee != null)
                    {
                        ret = new EmployeeModel
                        {
                            Id = employee.ID,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            CompanyName = employee.Company.Name,
                            CompanyDisplayName = employee.Company.DisplayName,
                            CompanyId = employee.CompanyId,
                            CreatedAt = employee.CreatedAt,
                            ModifiedAt = employee.ModifiedAt,
                            Dependents = new List<EmployeeDependentModel>()
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

        public async Task<List<EmployeeModel>> GetAllEmployeesForComapnyAsync(int companyId)
        {
            List<EmployeeModel> ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {

                    var employeeUserType = await dbContext.UserTypes.FirstOrDefaultAsync(x => x.Name == AppConstants.UserType.Employee && x.IsActive == true);
                    if (employeeUserType != null)
                    {
                        ret = await (from U in dbContext.Users
                               where U.IsActive == true && U.UserTypeId == employeeUserType.ID && U.CompanyId==companyId
                               select new EmployeeModel
                               {
                                   FirstName = U.FirstName,
                                   LastName = U.LastName,
                                   Email = U.Email,
                                   CompanyId = U.CompanyId,
                                   CreatedAt = U.CreatedAt,
                                   Dependents = U.EmployeeDependents.Select(x => new EmployeeDependentModel
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

        private void GetEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentModel> dependents, int employeeId)
        {
            var dbDependents = dbContext.EmployeeDependents.Where(x => x.EmployeeId == employeeId && x.IsActive);

            foreach(var dbDependent in dbDependents)
            {
                var d = new EmployeeDependentModel
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

        public async Task<EmployeeModel> UpdateEmployeeAsync(EmployeeModel employeeUpdateInfo)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var modifiedAt = DateTime.Now;
                    var e = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeUpdateInfo.Id && x.IsActive == true);
                    if (e != null)
                    {
                        UpdateEmployeeDependents(dbContext, employeeUpdateInfo.Dependents, employeeUpdateInfo.Id);
                        e.FirstName = employeeUpdateInfo.FirstName;
                        e.LastName = employeeUpdateInfo.LastName;
                        e.Email = employeeUpdateInfo.Email;
                        e.ModifiedAt = modifiedAt;
                    }

                    if (dbContext.SaveChanges() > 0)
                    {
                        employeeUpdateInfo.ModifiedAt = modifiedAt;
                        employeeUpdateInfo.ModifiedAtText = modifiedAt.ToString("MM/dd/yyy");
                        ret = employeeUpdateInfo;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Employee Data Layer: UpdateEmployeeAsync Exception Msg", ex.Message);
            }

            return ret;
        }

        private void UpdateEmployeeDependents(ElevateEntities dbContext, List<EmployeeDependentModel> dependents, int employeeId)
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

        public async Task<EmployeeModel> DeleteEmployeeAsync(int employeeId)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = await dbContext.Users.FirstOrDefaultAsync(x => x.ID == employeeId && x.IsActive == true);
                    if (employee != null)
                    {
                        ret = new EmployeeModel { Dependents = new List<EmployeeDependentModel>() };
                        GetEmployeeDependents(dbContext, ret.Dependents, employeeId);

                        employee.IsActive = false;
                        DeleteEmployeeAsyncDependents(dbContext, employeeId);

                        if ((await dbContext.SaveChangesAsync()) > 0)
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

        public async Task<TableModel<EmployeeModel>> GetEmployeesForEBDashboardAsync(EBEmployeeListRequestModel requestModel)
        {
            TableModel<EmployeeModel> ret = new TableModel<EmployeeModel> { Rows = new List<EmployeeModel>() };

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
                        var rows = data.Select(employee => new EmployeeModel
                        {
                            Id = employee.Id,
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            CompanyId = employee.CompanyId,
                            CompanyDisplayName = employee.CompanyDisplayName,
                            NumberOfDependents = employee.NumberOfDependents ?? 0,
                            Dependents = new List<EmployeeDependentModel>(),
                            CreatedAtText = employee.CreatedAt
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

        private void GetEmployeesDependents(ElevateEntities dbContext, List<EmployeeModel> employees)
        {
            var employeeIds = employees.Select(x => x.Id).ToList();

            var dependents = (from ED in dbContext.EmployeeDependents
                              join employeeId in employeeIds on ED.EmployeeId equals employeeId
                              join R in dbContext.Relationships on ED.RelationshipId equals R.ID
                              where ED.IsActive && R.IsActive
                              select new EmployeeDependentModel
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
                if (employee != null)
                {
                    employee.Dependents = group.ToList();
                    foreach(var d in employee.Dependents)
                    {
                        d.CreatedAtText = d.CreatedAt != null && d.CreatedAt.HasValue ? d.CreatedAt.Value.ToString("MM/dd/yyy") : string.Empty;
                    }
                }
            }
        }

        public async Task<EmployeeFormMasterDataModel> GetEmployeeFormMasterDataAsync()
        {
            EmployeeFormMasterDataModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new EmployeeFormMasterDataModel
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

        private  async Task<List<ListItem>> GetRelationshipsForEmployeeFormAsync(ElevateEntities dbContext)
        {
            return await (from R in dbContext.Relationships
                          where R.IsActive
                          select new
                          {
                              Value = R.ID,
                              Text = R.DisplayName
                          }).Select(x => new ListItem
                          {
                              Value = x.Value.ToString(),
                              Text = x.Text
                          }).ToListAsync();
        }
    }
}
