using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace AprraisalApplication.Repositories
{
    public class AccountRepository
    {
        private readonly ApplicationDbContext db;

        public AccountRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public ApplicationUser GetUserById(string userId)
        {
            return db.Users.Find(userId);
        }

        internal bool SaveEmployeeDetails(CreateEmployeeProfileVM model)
        {
            var userId = HttpContext.Current.User.Identity.GetUserId();

            var appUser = db.Users.Find(userId);

            // save the employee default appraiser
            DefaultUserAppraiser defaultAppraiser = new DefaultUserAppraiser();
            db.DefaultUserAppraisers.Add(defaultAppraiser);
            db.SaveChanges();

            Employee employee = new Employee(model, defaultAppraiser.Id, appUser);
            db.Employees.Add(employee);
            db.SaveChanges();

            if(model.SelectedQualifications != null && model.SelectedQualifications.Count() > 0)
            {
                foreach (var qualificationId in model.SelectedQualifications)
                {
                    EmployeeQualification employeeQualification = new EmployeeQualification(employee.Id, qualificationId);
                    db.EmployeeQualifications.Add(employeeQualification);
                }
            }

            ApplicationUser user = db.Users.Find(HttpContext.Current.User.Identity.GetUserId());
            user.EmployeeId = employee.Id;
            return db.SaveChanges() != 0;
        }

        internal CareerHistory SaveCareerHistoryHr(CareerHistoryParams model)
        {
            string userId = model.UserId;
            ApplicationUser user = db.Users.Find(userId);
            CareerHistory careerHistory = new CareerHistory(model, (int)user.EmployeeId);
            db.CareerHistories.Add(careerHistory);
            db.SaveChanges();
            return db.CareerHistories.Where(x => x.Id == careerHistory.Id)
                                        .Include(x => x.Department)
                                        .Include(x => x.Grade)
                                        .SingleOrDefault();
        }

        internal CareerHistory UpdateCareerHistory(CareerHistoryParams model)
        {
            CareerHistory careerHistory = db.CareerHistories.Find(model.Id);
            if(careerHistory != null)
            {
                careerHistory.Update(model);
                db.SaveChanges();
            }
            return db.CareerHistories.Where(x => x.Id == model.Id)
                                        .Include(x => x.Department)
                                        .Include(x => x.Grade)
                                        .SingleOrDefault();
        }

        internal ApplicationUser GetEmployeeByUsername(string username)
        {
            return db.Users.Where(x => x.UserName == username).SingleOrDefault();
        }

        internal void DeactivateUserAccount(DeactivateUserParams model)
        {
            ApplicationUser user = db.Users.Find(model.UserId);
            Employee employee = db.Employees.Where(x => x.ApplicationUserId == model.UserId).SingleOrDefault();
            employee.AccountDisabled = true;
            user.AccountDisabled = true;

            db.SaveChanges();
        }
        internal void ActivateUserAccount(DeactivateUserParams model)
        {
            ApplicationUser user = db.Users.Find(model.UserId);
            Employee employee = db.Employees.Where(x => x.ApplicationUserId == model.UserId).SingleOrDefault();
            employee.AccountDisabled = false;
            user.AccountDisabled = false;

            db.SaveChanges();
        }

        internal void DeleteCareerHistory(int id)
        {
            CareerHistory careerHistory = db.CareerHistories.Find(id);
            if(careerHistory != null)
            {
                db.CareerHistories.Remove(careerHistory);
                db.SaveChanges();
            }
        }

        internal CareerHistory SaveCareerHistory(CareerHistoryParams model)
        {
            string userId = HttpContext.Current.User.Identity.GetUserId();
            ApplicationUser user = db.Users.Find(userId);
            CareerHistory careerHistory = new CareerHistory(model, (int)user.EmployeeId);
            db.CareerHistories.Add(careerHistory);
            db.SaveChanges();
            return db.CareerHistories.Where(x => x.Id == careerHistory.Id)
                                        .Include(x => x.Department)
                                        .Include(x => x.Grade)
                                        .SingleOrDefault();
        }

        internal Employee GetEmployeeByUserId(string userId)
        {
            return db.Employees.Where(x => x.ApplicationUserId == userId)
                                .Include(x => x.JobTitle)
                                .Include(x => x.State)
                                .Include(x => x.EmployeeQualifications.Select(q => q.Qualification))
                                .Include(x => x.CareerHistories.Select(q => q.Department))
                                .Include(x => x.CareerHistories.Select(q => q.Grade))
                                .Include(x => x.Branch)
                                .Include(x => x.Title)
                                .Include(x => x.Grade)
                                .Include(x => x.Gender)
                                .Include(x => x.Department)
                                .Include(x => x.ApplicationUser)
                                .SingleOrDefault();
        }

        internal bool UpdateEmployeeDetails(CreateEmployeeProfileVM model)
        {

            Employee employee = db.Employees.Where(x => x.ApplicationUserId == model.ApplicationUserId).SingleOrDefault();
            employee.UpdateEmployee(model);

            if (model.SelectedQualifications != null && model.SelectedQualifications.Count() > 0)
            {
                List<Qualification> qualifications = db.Qualifications.Where(x => x.IsDeleted == false).ToList();
                foreach (var item in qualifications)
                {
                    if(model.SelectedQualifications.Contains(item.Id))
                    {
                        if(!db.EmployeeQualifications.Any(x => x.EmployeeId == employee.Id && x.QualificationId == item.Id))
                        {
                            EmployeeQualification employeeQualification = new EmployeeQualification(employee.Id, item.Id);
                            db.EmployeeQualifications.Add(employeeQualification);
                        }
                    }
                    else
                    {
                        if (db.EmployeeQualifications.Any(x => x.EmployeeId == employee.Id && x.QualificationId == item.Id))
                        {
                            EmployeeQualification empQualification = db.EmployeeQualifications.Where(x => x.EmployeeId == employee.Id && x.QualificationId == item.Id).SingleOrDefault();
                            db.EmployeeQualifications.Remove(empQualification);
                        }
                    }
                }
            }
            else
            {
                List<EmployeeQualification> empQualifications = db.EmployeeQualifications
                                                                    .Where(x => x.EmployeeId == employee.Id)
                                                                    .ToList();
                foreach (var item in empQualifications)
                {
                    db.EmployeeQualifications.Remove(item);
                }
            }

            return db.SaveChanges() != 0;
        }

        internal List<ApplicationUser> GetAllUsers()
        {
            return db.Users.Where(x => x.EmailConfirmed == true && x.EmployeeId != null).ToList();
        }

        internal Employee GetEmployeeById(int employeeId)
        {
            return db.Employees.Find(employeeId);
        }

        internal ApplicationUser GetEmployeeRoles(string employeeUserId)
        {
            return db.Users.Where(x => x.Id == employeeUserId)
                                    .Include(x => x.Roles)
                                    .SingleOrDefault();      
        }
    }
}