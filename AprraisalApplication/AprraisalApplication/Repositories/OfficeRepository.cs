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
    }
}