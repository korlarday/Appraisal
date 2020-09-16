using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AprraisalApplication.Repositories
{
    public class ResourcesRepository
    {
        private readonly ApplicationDbContext db;

        public ResourcesRepository(ApplicationDbContext context)
        {
            db = context;
        }

        public List<Department> GetAllDepartments()
        {
            return db.Departments.Where(x => x.IsDeleted == false).ToList();
        }

        internal Department GetDepartmentByName(string departmentName)
        {
            return db.Departments.Where(x => x.Name == departmentName).FirstOrDefault();
        }

        internal void AddNewDepartment(Department newDept)
        {
            db.Departments.Add(newDept);
            db.SaveChanges();
        }

        internal List<Gender> GetGenders()
        {
            return db.Genders.ToList();
        }

        internal Department GetDepartmentById(int id)
        {
            return db.Departments.Find(id);
        }

        internal List<JobTitle> GetAllJobTitles()
        {
            return db.JobTitles.Where(x => x.IsDeleted == false).ToList();
        }

        internal List<Qualification> GetAllQualifications()
        {
            return db.Qualifications.Where(x => x.IsDeleted == false).ToList();
        }

        internal List<SectionType> GetSectionTypes()
        {
            return db.SectionTypes.ToList();
        }

        internal List<ExpectedValue> GetExpectedValues()
        {
            return db.ExpectedValues.ToList();
        }

        internal List<DefaultRating> GetDefaultRatings()
        {
            return db.DefaultRatings.OrderByDescending(x => x.Score).ToList();
        }

        internal List<Title> GetTitles()
        {
            return db.Titles.Where(x => x.IsDeleted == false).ToList();
        }

        internal bool IsDepartmentNameExists(int id, string departmentName)
        {
            return db.Departments.Where(x => x.Id != id).Any(x => x.Name == departmentName);
        }

        internal List<Branch> GetAllBranches()
        {
            return db.Branches.Where(x => x.IsDeleted == false).Include(x => x.State).ToList();
        }

        internal List<State> GetAllStates()
        {
            return db.States.Where(x => x.IsDeleted == false).ToList();
        }

        internal Department UpdateDepartment(int id, string departmentName)
        {
            Department department = db.Departments.Find(id);
            department.Name = departmentName;
            db.SaveChanges();
            return department;
        }

        internal void DeleteDepartment(int id)
        {
            Department department = db.Departments.Find(id);
            department.IsDeleted = true;
            db.SaveChanges();
        }

        internal Branch GetBranchByName(string branchName)
        {
            return db.Branches.Where(x => x.Description == branchName && x.IsDeleted == false).FirstOrDefault();
        }

        internal void AddNewBranch(Branch newBranch)
        {
            db.Branches.Add(newBranch);
            db.SaveChanges();
        }

        internal State GetStateById(int stateId)
        {
            return db.States.Find(stateId);
        }

        internal object GetBranchById(int id)
        {
            return db.Branches.Find(id);
        }

        internal bool IsBranchNameExists(string branchName, int id)
        {
            return db.Branches.Where(x => x.Id != id).Any(x => x.Description == branchName);
        }

        internal Branch UpdateBranch(BranchParams model)
        {
            Branch branch = db.Branches.Find(model.Id);
            branch.Description = model.BranchName;
            branch.StateId = model.StateId;
            db.SaveChanges();
            return branch;
        }

        internal void DeleteBranch(int id)
        {
            Branch branch = db.Branches.Find(id);
            branch.IsDeleted = true;
            db.SaveChanges();
        }

        internal List<Grade> GetGrades()
        {
            return db.Grades.Where(x => x.IsDeleted == false).ToList();
        }

        internal List<TemplateSummaryRating> GetTemplateSummaryRatings(int appraisalTemplateId)
        {
            return db.TemplateSummaryRatings.Where(x => x.AppraisalTemplateId == appraisalTemplateId)
                                            .OrderBy(x => x.Score).ToList();
        }
    }
}