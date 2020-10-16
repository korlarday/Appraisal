using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.Constants;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class OfficeRepository
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly ApplicationDbContext db;

        public OfficeRepository(ApplicationDbContext context)
        {
            db = context;
        }
        public OfficeRepository(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        internal Employee GetUserEmployeeDetails(int employeeId)
        {
            return db.Employees.Find(employeeId);
        }

        internal List<Employee> GetAllEmployeesInDepartment(int departmentId)
        {
            return db.Employees.Where(x => x.DepartmentId == departmentId)
                                .Include(x => x.DefaultUserAppraiser)
                                .Include(x => x.State)
                                .Include(x => x.Department)
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

        internal async Task<string> SetEmployeesAppraiser(UserAppraiserParams model)
        {
            foreach (var item in model.Items)
            {
                if(item.AppraiserId != null)
                {
                    DefaultUserAppraiser appraiser = db.DefaultUserAppraisers
                                                        .Where(x => x.AppraiseeId == item.ApplicationUserId)
                                                        .SingleOrDefault();
                    appraiser.AppraiserId = item.AppraiserId;

                    // add supervisor role to the user
                    bool appraiserHasSupervisorRole = await UserManager.IsInRoleAsync(item.AppraiserId, RoleModel.Supervisor);
                    if (!appraiserHasSupervisorRole)
                    {
                        await UserManager.AddToRoleAsync(item.AppraiserId, RoleModel.Supervisor);
                    }
                }
                db.SaveChanges();

            }
            foreach (var user in model.Items)
            {
                //check if the appraisee is not a supervisor
                int subordinatesCount = db.DefaultUserAppraisers
                                            .Where(x => x.AppraiserId == user.ApplicationUserId)
                                            .Count();
                if (subordinatesCount < 1)
                {
                    // check if the user has supervisor role
                    bool appraiseeHasSupervisorRole = await UserManager.IsInRoleAsync(user.ApplicationUserId, RoleModel.Supervisor);
                    if (appraiseeHasSupervisorRole)
                    {
                        // remove the role
                        await UserManager.RemoveFromRoleAsync(user.ApplicationUserId, RoleModel.Supervisor);
                    }
                }
            }
            return "complete";
        }

        internal List<Employee> SearchEmployeesUsingDeptAndState(List<string> departments, List<string> states)
        {
            List<Employee> employees = db.Employees
                                            .Where(x => x.AccountDisabled != true)
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

        internal List<Employee> GetAllHods()
        {
            var hodRole = db.Roles.Where(x => x.Name == PositionsCS.Hod).SingleOrDefault();
            var users = db.Users.Include(x => x.Roles)
                                .Where(x => x.AccountDisabled == false &&
                                x.Roles.Select(r => r.RoleId).Contains(hodRole.Id))
                                .ToList();
            List<Employee> hods = new List<Employee>();
            foreach (var user in users)
            {
                Employee employee = db.Employees.Where(x => x.Id == user.EmployeeId)
                                                .Include(x => x.State)
                                                .Include(x => x.Department)
                                                .Include(x => x.DefaultUserAppraiser)
                                                .SingleOrDefault();
                hods.Add(employee);
            }
            return hods;
        }

        

        internal List<Employee> GetAllHodsAndHigherRanks()
        {
            var hodRole = db.Roles.Where(x => x.Name == PositionsCS.Hod).SingleOrDefault();
            var hrRole = db.Roles.Where(x => x.Name == PositionsCS.Hr).SingleOrDefault();
            var mdRole = db.Roles.Where(x => x.Name == PositionsCS.Md).SingleOrDefault();

            var users = db.Users.Include(x => x.Roles)
                                .Where(x => x.Roles.Select(r => r.RoleId).Contains(hodRole.Id) ||
                                            x.Roles.Select(r => r.RoleId).Contains(hrRole.Id) ||
                                            x.Roles.Select(r => r.RoleId).Contains(mdRole.Id))
                                .ToList();

            List<Employee> hods = new List<Employee>();
            foreach (var user in users)
            {
                Employee employee = db.Employees.Where(x => x.Id == user.EmployeeId)
                                                .SingleOrDefault();
                if (!hods.Contains(employee))
                {
                    hods.Add(employee);
                }
            }
            return hods;
        }

        internal List<Employee> GetDeactivatedEmployees()
        {
            return db.Employees
                    .Where(x => x.AccountDisabled == true)
                    .Include(x => x.Department)
                    .Include(x => x.State)
                    .ToList();
        }

        internal List<Employee> GetAllEmployees()
        {
            return db.Employees
                    .Where(x => x.AccountDisabled != true)
                    .Include(x => x.Department)
                    .Include(x => x.State)
                    .ToList();
        }
        internal List<Employee> GetAllEmployeesAsSupervisors()
        {
            return db.Employees
                    .Where(x => x.AccountDisabled != true)
                    .Include(x => x.DefaultUserAppraiser)
                    .ToList();
        }

        internal List<DepartmentAndParticipants> GetDepartmentAndEmployeesCount()
        {
            List<DepartmentAndParticipants> participants = new List<DepartmentAndParticipants>();
            List<Department> departments = db.Departments.Where(x => x.IsDeleted == false).OrderBy(x => x.Name).ToList();
            foreach (var depart in departments)
            {
                int count = db.Employees.Where(x => x.DepartmentId == depart.Id).Count();
                DepartmentAndParticipants item = new DepartmentAndParticipants
                {
                    Department = depart,
                    NumberOfParticipants = count
                };
                participants.Add(item);
            }
            return participants;
        }

        internal List<LocationAndEmployees> GetLocationAndEmployees()
        {
            List<LocationAndEmployees> results = new List<LocationAndEmployees>();
            List<State> states = db.States.Where(x => x.IsDeleted == false).OrderBy(x => x.Description).ToList();

            foreach (var state in states)
            {
                List<Employee> employees = db.Employees.Where(x => x.StateId == state.Id)
                                                        .ToList();
                LocationAndEmployees location = new LocationAndEmployees
                {
                    State = state,
                    Employees = employees
                };
                results.Add(location);
            }
            return results;
        }

        internal List<Employee> GetAllEmployeesInState(int stateId)
        {
            return db.Employees.Where(x => x.StateId == stateId)
                                .Include(x => x.DefaultUserAppraiser)
                                .Include(x => x.State)
                                .Include(x => x.Department)
                                .ToList();
        }

        internal async Task<List<Employee>> GetAllEmployeesInDepartmentAndHigherRanks(int departmentId)
        {
            List<Employee> employees = new List<Employee>();
            var departmentEmployees = db.Employees.Where(x => x.DepartmentId == departmentId)
                                                    .ToList();
            employees = departmentEmployees;

            var highranks = db.Users.ToList();
            foreach (var user in highranks)
            {
                bool isInRole = await UserManager.IsInRoleAsync(user.Id, "MD");
                if (isInRole)
                {
                    if(!departmentEmployees.Select(x => x.ApplicationUserId).Contains(user.Id))
                    {
                        Employee employee = db.Employees.Where(x => x.Id == user.EmployeeId)
                                                        .SingleOrDefault();
                        employees.Add(employee);
                    }
                }
            }
            return employees;
        }

        internal async Task<List<Employee>> GetAllEmployeesInDepartmentWithoutHod(int departmentId)
        {
            List<Employee> EmployeesWithoutHod = new List<Employee>();
            List<Employee> employees = db.Employees.Where(x => x.DepartmentId == departmentId)
                                                .Include(x => x.DefaultUserAppraiser)
                                                .Include(x => x.State)
                                                .Include(x => x.Department)
                                                .ToList();
            foreach (var employee in employees)
            {
                var hasHodRole = await UserManager.IsInRoleAsync(employee.ApplicationUserId, RoleModel.Hod);
                if (!hasHodRole)
                {
                    EmployeesWithoutHod.Add(employee);
                }
            }
            return EmployeesWithoutHod;
        }
    }
}