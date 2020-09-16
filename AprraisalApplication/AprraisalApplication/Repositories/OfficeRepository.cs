using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class OfficeRepository
    {
        private readonly ApplicationDbContext db;

        public OfficeRepository(ApplicationDbContext context)
        {
            db = context;
        }

        internal Employee GetUserEmployeeDetails(int employeeId)
        {
            return db.Employees.Find(employeeId);
        }

        internal List<Employee> GetAllEmployeesInDepartment(int departmentId)
        {
            return db.Employees.Where(x => x.DepartmentId == departmentId)
                                .Include(x => x.DefaultUserAppraiser)
                                .ToList();
        }

        internal int GetAllCompletedAppraisals()
        {
            return db.NewAppraisals.Where(x => x.IsCompleted == true).Count();
        }

        internal int GetAllOngoingAppraisals()
        {
            return db.NewAppraisals.Where(x => x.IsCompleted == false).Count();
        }

        internal void SetEmployeesAppraiser(UserAppraiserParams model)
        {
            foreach (var item in model.Items)
            {
                if(item.AppraiserId != null)
                {
                    DefaultUserAppraiser appraiser = db.DefaultUserAppraisers
                                                        .Where(x => x.AppraiseeId == item.ApplicationUserId)
                                                        .SingleOrDefault();
                    appraiser.AppraiserId = item.AppraiserId;
                    appraiser.ToTeamLeader = item.ToTeamLeader;
                }
            }
            db.SaveChanges();
        }

        internal List<Employee> SearchEmployeesUsingDeptAndState(List<string> departments, List<string> states)
        {
            List<Employee> employees = db.Employees
                                            .Include(x => x.Department)
                                            .Include(x => x.State)
                                            .ToList();
            List<Employee> empDepartmentFilter = new List<Employee>();
            List<Employee> empStateFilter = new List<Employee>();

            // filter the departments
            if (!departments.Contains("all"))
            {
                foreach (var employee in employees)
                {
                    if (departments.Contains(employee.DepartmentId.ToString()))
                    {
                        empDepartmentFilter.Add(employee);
                    }
                }
            }
            else
            {
                // if selected department contains all
                empDepartmentFilter = employees;
            }

            // filter the locations
            if (!states.Contains("all"))
            {
                foreach (var employee in empDepartmentFilter)
                {
                    if (states.Contains(employee.StateId.ToString()))
                    {
                        empStateFilter.Add(employee);
                    }
                }
            }
            else
            {
                // if selected state contains all
                empStateFilter = empDepartmentFilter;
            }

            return empStateFilter;
        }

        internal List<Employee> GetAllEmployees()
        {
            return db.Employees
                    .Include(x => x.Department)
                    .Include(x => x.State)
                    .ToList();
        }
    }
}