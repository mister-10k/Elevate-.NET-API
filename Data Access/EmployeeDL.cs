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
        public EmployeeModel CreateEmployee(EmployeeModel employee)
        {
            EmployeeModel ret = null;

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
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Email = employee.Email,
                            CompanyId = employee.CompanyId,
                            UserTypeId = employeeUserType.ID,
                            CreatedAt = createdAt,
                            IsActive = true
                        };

                        dbContext.Users.Add(newEmployee);
                        var employeeSaved = dbContext.SaveChanges() > 0;

                        if (employeeSaved)
                            AddEmployeeDependents(dbContext, employee.Dependents, newEmployee.ID);
                        var depndentsSaved = dbContext.SaveChanges() > 0;

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
                Console.WriteLine("Data Layer: CreateEmployee Exception Msg", ex.Message);
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

        public EmployeeModel GetEmployee(int employeeId)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = dbContext.Users.FirstOrDefault(x => x.ID == employeeId);
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
                Console.WriteLine("Data Layer: GetEmployee Exception Msg", ex.Message);
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

        public EmployeeModel UpdateEmployee(EmployeeModel employeeUpdateInfo)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var modifiedAt = DateTime.Now;
                    var e = dbContext.Users.FirstOrDefault(x => x.ID == employeeUpdateInfo.Id);
                    if (e != null)
                    {
                        UpdateEmployeeDependents(dbContext, employeeUpdateInfo.Dependents, employeeUpdateInfo.Id);
                        e.FirstName = employeeUpdateInfo.FirstName;
                        e.LastName = employeeUpdateInfo.LastName;
                        e.CompanyId = employeeUpdateInfo.CompanyId;
                        e.ModifiedAt = modifiedAt;
                    }

                    if (dbContext.SaveChanges() > 0)
                    {
                        e.ModifiedAt = modifiedAt;
                        ret = employeeUpdateInfo;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: UpdateEmployee Exception Msg", ex.Message);
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

        public EmployeeModel DeleteEmployee(int employeeId)
        {
            EmployeeModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    var employee = dbContext.Users.FirstOrDefault(x => x.ID == employeeId);
                    if (employee != null)
                    {
                        ret = new EmployeeModel { Dependents = new List<EmployeeDependentModel>() };
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

        public EBDashbaordStatsModel GetEBDashboardCardsData(int companyId)
        {
            var ret = new EBDashbaordStatsModel();

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret.Employees = new List<EmployeeModel>();
                    var employees = dbContext.Users.Where(x => x.CompanyId == companyId && x.UserType.Name == AppConstants.UserType.Employee);
                    foreach(var employee in employees)
                    {
                        var EmployeeModel = new EmployeeModel
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
                            Dependents = new List<EmployeeDependentModel>()                       
                        };
                        GetEmployeeDependents(dbContext, EmployeeModel.Dependents, employee.ID);
                        ret.Employees.Add(EmployeeModel);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: GetEBDashboardCardsData Exception Msg", ex.Message);
            }

            return ret;
        }

        public TableModel<EmployeeModel> GetEmployeesForEBDashboard(EBEmployeeListRequestModel requestModel)
        {
            TableModel<EmployeeModel> ret = new TableModel<EmployeeModel> { Rows = new List<EmployeeModel>() };

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
                Console.WriteLine("Data Layer: GetEmployeesForEBDashboard Exception Msg", ex.Message);
            }

            return ret;
        }

        private void GetEmployeesDependents(ElevateEntities dbContext, List<EmployeeModel> employees)
        {
            var employeeIds = employees.Select(x => x.Id).ToList();

            var dependents = (from ED in dbContext.EmployeeDependents
                              join employeeId in employeeIds on ED.EmployeeId equals employeeId
                              join R in dbContext.Relationships on ED.RelationshipId equals R.ID
                              where ED.IsActive
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

        public EmployeeFormMasterDataModel GetEmployeeFormMasterData()
        {
            EmployeeFormMasterDataModel ret = null;

            try
            {
                using (ElevateEntities dbContext = new ElevateEntities())
                {
                    ret = new EmployeeFormMasterDataModel
                    {
                        Relationships = GetRelationshipsForEmployeeForm(dbContext)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Data Layer: GetEmployeeFormMasterData Exception Msg", ex.Message);
            }

            return ret;
        }

        private  List<ListItem> GetRelationshipsForEmployeeForm(ElevateEntities dbContext)
        {
            return (from R in dbContext.Relationships
                    where R.IsActive
                    select new
                    {
                        Value = R.ID,
                        Text = R.DisplayName
                    }).Select(x => new ListItem
                    {
                        Value = x.Value.ToString(),
                        Text = x.Text
                    }).ToList();
        }
    }
}
