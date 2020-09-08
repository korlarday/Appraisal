using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.Attributes;
using AprraisalApplication.Models.Constants;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class AppraisalRepository
    {
        private readonly ApplicationDbContext db;

        public AppraisalRepository(ApplicationDbContext context)
        {
            db = context;
        }

        internal IEnumerable<AppraisalType> GetAppraisalTypes()
        {
            return db.AppraisalTypes.ToList();
        }

        internal void InitiateNewAppraisal(NewAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        // save the new Appraisal
                        NewAppraisal newAppraisal = new NewAppraisal(model);
                        context.NewAppraisals.Add(newAppraisal);
                        context.SaveChanges();

                        // store the selected locations
                        // if all is selected
                        if (model.States.Contains("all"))
                        {
                            List<int> stateIds = context.States.Select(x => x.Id).ToList();
                            foreach (var stateId in stateIds)
                            {
                                AppraisalLocation appraisalLocation = new AppraisalLocation(newAppraisal.Id, stateId);
                                context.AppraisalLocations.Add(appraisalLocation);
                            }
                        }
                        else
                        {
                            foreach (var stateId in model.States)
                            {
                                AppraisalLocation appraisalLocation = new AppraisalLocation(newAppraisal.Id, Int32.Parse(stateId));
                                context.AppraisalLocations.Add(appraisalLocation);
                            }
                        }

                        // store the selected departments
                        // if all is selected
                        if (model.Departments.Contains("all"))
                        {
                            List<int> departmentIds = context.Departments.Select(x => x.Id).ToList();
                            foreach (var departmentId in departmentIds)
                            {
                                AppraisalDepartment appraisalDept = new AppraisalDepartment(newAppraisal.Id, departmentId);
                                context.AppraisalDepartments.Add(appraisalDept);
                            }
                        }
                        else
                        {
                            foreach (var departmentId in model.Departments)
                            {
                                AppraisalDepartment appraisalDept = new AppraisalDepartment(newAppraisal.Id, Int32.Parse(departmentId));
                                context.AppraisalDepartments.Add(appraisalDept);
                            }
                        }

                        // store the staff for the new appraisal
                        foreach (var employeeId in model.SelectedEmployees)
                        {
                            AppraisalStaff appraisalStaff = new AppraisalStaff(newAppraisal.Id, employeeId);
                            context.AppraisalStaffs.Add(appraisalStaff);
                        }

                        // sync the appraisal templates for the new appraisal exercise
                        List<AppraisalTemplate> appraisalTemplates = context.AppraisalTemplates
                                                                        .Where(x => x.IsDeleted == false)
                                                                        .Include(x => x.AppraisalTemplateSections
                                                                            .Select(d => d.AppraisalSectionDetails
                                                                            .Select(b => b.SectionDetailBreakdowns)))
                                                                        .ToList();
                        foreach (var template in appraisalTemplates)
                        {
                            InitiatedAppraisalTemplate appraisalTemplate = new InitiatedAppraisalTemplate(template, newAppraisal.Id);
                            context.InitiatedAppraisalTemplates.Add(appraisalTemplate);
                            context.SaveChanges();
                            if (template.AppraisalTemplateSections != null && template.AppraisalTemplateSections.Count() > 0)
                            {
                                foreach (var section in template.AppraisalTemplateSections)
                                {
                                    InitiatedTemplateSection initiatedSection = new InitiatedTemplateSection(section, appraisalTemplate.Id);
                                    context.InitiatedTemplateSections.Add(initiatedSection);
                                    context.SaveChanges();

                                    if (section.AppraisalSectionDetails != null && section.AppraisalSectionDetails.Count() > 0)
                                    {
                                        foreach (var detail in section.AppraisalSectionDetails)
                                        {
                                            InitiatedSectionDetail sectionDetail = new InitiatedSectionDetail(detail, initiatedSection.Id);
                                            context.InitiatedSectionDetails.Add(sectionDetail);
                                            context.SaveChanges();

                                            if (detail.SectionDetailBreakdowns != null && detail.SectionDetailBreakdowns.Count() > 0)
                                            {
                                                foreach (var item in detail.SectionDetailBreakdowns)
                                                {
                                                    InitiatedSectionDetailBreakdown breakdown = new InitiatedSectionDetailBreakdown(item, sectionDetail.Id);
                                                    context.InitiatedSectionDetailBreakdowns.Add(breakdown);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }


                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        internal void ResubmitAppraisalToSupervisor(SubmitAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.InitiatedAppraisalTemplate.InitiatedTemplateSections
                                                                            .Select(s => s.InitiatedSectionDetails
                                                                            .Select(b => b.InitiatedSectionDetailBreakdowns)))
                                                            .Include(x => x.AppraiseeProgress)
                                                            .Include(x => x.AppraiseeRejections)
                                                            .SingleOrDefault();
                        // update the task performed and the duties assigned
                        foreach (var section in appraisee.InitiatedAppraisalTemplate.InitiatedTemplateSections)
                        {
                            if(section.IsDeleted == false)
                            {
                                // store section result
                                Models.MigrationModels.SectionResult sectionResultDb = context.SectionResults.Where(x => x.AppraiseeId == appraisee.Id
                                                                                                            && x.InitiatedTemplateSectionId == section.Id)
                                                                                                        .SingleOrDefault();

                                if (section.SectionTypeId == SectionTypeCS.TaskPerfomed)
                                {
                                    var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                
                                    foreach (var item in sectionResult.TaskPerformed)
                                    {
                                        if(item.SectionDetailResultId == null)
                                        {
                                            SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id, section.Id);
                                            context.SectionDetailResults.Add(detail);
                                        }
                                        else
                                        {
                                            SectionDetailResult detail = context.SectionDetailResults.Find(item.SectionDetailResultId);
                                            detail.Update(item);
                                        }
                                    }
                                }
                                else if (section.SectionTypeId == SectionTypeCS.DutiesAssigned)
                                {

                                    var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                
                                    foreach (var item in sectionResult.TaskPerformed)
                                    {
                                        if(item.SectionDetailResultId == null)
                                        {
                                            SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id, section.Id);
                                            context.SectionDetailResults.Add(detail);
                                        }
                                        else
                                        {
                                            SectionDetailResult detail = context.SectionDetailResults.Find(item.SectionDetailResultId);
                                            detail.Update(item);
                                        }
                                    }
                                }
                                else if (section.SectionTypeId == SectionTypeCS.Quantitative)
                                {
                                    var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                    if (sectionResult != null)
                                    {
                                        foreach (var item in section.InitiatedSectionDetails)
                                        {
                                            if (item.IsDeleted == false)
                                            {
                                                var sectionDetail = sectionResult.TaskPerformed.Where(x => x.Number == item.Id).SingleOrDefault();
                                                SectionDetailResult detail = context.SectionDetailResults.Find(sectionDetail.SectionDetailResultId);
                                                if (item.InitiatedSectionDetailBreakdowns != null && item.InitiatedSectionDetailBreakdowns.Count() > 0)
                                                {
                                                    foreach (var breakdown in sectionDetail.Breakdowns)
                                                    {
                                                        ItemBreakdownResult itemBreakdown = context.ItemBreakdownResults.Find(breakdown.BreakdownId);
                                                        itemBreakdown.Value = breakdown.BreakdownValue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        
                            }
                        }

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.SupervisorReject = false;

                        // change the rejection status
                        List<AppraiseeRejection> rejections = appraisee.AppraiseeRejections.Where(x => x.New == true).ToList();
                        foreach (var item in rejections)
                        {
                            item.New = false;
                        }
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new System.ArgumentException(e.Message);
                    }
                }
            }

        }

        internal void DeleteSectionResultDetailItem(SectionResultParams model)
        {
            SectionDetailResult result = db.SectionDetailResults.Find(model.SectionResultDetailId);
            db.SectionDetailResults.Remove(result);
            db.SaveChanges();
        }

        internal void RejectAppraisalToAppraisee(SectionScoresParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.AppraiseeProgress)
                                                            .SingleOrDefault();
                        string userId = HttpContext.Current.User.Identity.GetUserId();
                        Employee employee = context.Employees.Where(x => x.ApplicationUserId == userId).SingleOrDefault();

                        AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id);
                        context.AppraiseeRejections.Add(rejection);

                        // update the appraisee progress
                        var progress = appraisee.AppraiseeProgress;
                        progress.SupervisorReject = true;
                        progress.SupervisorSubmit = false;

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new System.ArgumentException(e.Message);
                    }
                }
            }

        }

        internal void ScoreAppraisalSections(SectionScoresParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.SectionResults)
                                                            .Include(x => x.AppraiseeProgress)
                                                            .SingleOrDefault();

                        var sectionResults = appraisee.SectionResults.ToList();
                        foreach (var sectionResult in sectionResults)
                        {
                            var sectionScore = model.SectionScoresResults.Where(x => x.SectionResultId == sectionResult.Id).SingleOrDefault();
                            if(sectionScore != null)
                            {
                                sectionResult.TotalScore = sectionScore.SectionTotalScore;
                                sectionResult.PercentageScore = sectionScore.SectionPercentageScore;
                                foreach (var detail in sectionResult.SectionDetailResults)
                                {
                                    var detailScore = sectionScore.SectionDetailsScore.Where(x => x.SectionResultDetailId == detail.Id).SingleOrDefault();
                                    if(detailScore != null)
                                    {
                                        detail.Score = detailScore.Score;
                                    }
                                }

                            }
                        }

                        // update the appraisee progress
                        var progress = appraisee.AppraiseeProgress;
                        progress.SupervisorReject = false;
                        progress.SupervisorSubmit = false;
                        progress.SupervisorAskForFeedback = true;

                        
                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new System.ArgumentException(e.Message);
                    }
                }
            }

        }

        internal void SubmitAppraisalToSupervisor(SubmitAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.InitiatedAppraisalTemplate.InitiatedTemplateSections
                                                                            .Select(s => s.InitiatedSectionDetails
                                                                            .Select(b => b.InitiatedSectionDetailBreakdowns)))
                                                            .Include(x => x.AppraiseeProgress)
                                                            .SingleOrDefault();
                        // store the task performed and the duties assigned
                        foreach (var section in appraisee.InitiatedAppraisalTemplate.InitiatedTemplateSections)
                        {
                            // store section result
                            Models.MigrationModels.SectionResult sectionResultDb = new Models.MigrationModels.SectionResult(appraisee.Id, section.Id);
                            context.SectionResults.Add(sectionResultDb);
                            context.SaveChanges();

                            if (section.SectionTypeId == SectionTypeCS.TaskPerfomed)
                            {
                                var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                
                                if (sectionResult != null)
                                {
                                    foreach (var item in sectionResult.TaskPerformed)
                                    {
                                        SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id, section.Id);
                                        context.SectionDetailResults.Add(detail);
                                        context.SaveChanges();
                                    }
                                }
                            }
                            else if (section.SectionTypeId == SectionTypeCS.DutiesAssigned)
                            {

                                var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                if (sectionResult != null)
                                {
                                    foreach (var item in sectionResult.TaskPerformed)
                                    {
                                        SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id, section.Id);
                                        context.SectionDetailResults.Add(detail);
                                    }
                                }
                            }
                            else if (section.SectionTypeId == SectionTypeCS.Qualitative)
                            {
                                foreach (var item in section.InitiatedSectionDetails)
                                {
                                    SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id);
                                    context.SectionDetailResults.Add(detail);
                                    context.SaveChanges();
                                    if (item.InitiatedSectionDetailBreakdowns != null && item.InitiatedSectionDetailBreakdowns.Count() > 0)
                                    {
                                        foreach (var breakdown in item.InitiatedSectionDetailBreakdowns)
                                        {
                                            ItemBreakdownResult itemBreakdown = new ItemBreakdownResult(breakdown, detail.Id);
                                            context.ItemBreakdownResults.Add(itemBreakdown);
                                        }
                                    }
                                }

                            }
                            else if (section.SectionTypeId == SectionTypeCS.Quantitative)
                            {
                                var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                if (sectionResult != null)
                                {
                                    foreach (var item in section.InitiatedSectionDetails)
                                    {
                                        var sectionDetail = sectionResult.TaskPerformed.Where(x => x.Number == item.Id).SingleOrDefault();
                                        SectionDetailResult detail = new SectionDetailResult(item, sectionResultDb.Id);
                                        context.SectionDetailResults.Add(detail);
                                        context.SaveChanges();
                                        if (item.InitiatedSectionDetailBreakdowns != null && item.InitiatedSectionDetailBreakdowns.Count() > 0)
                                        {
                                            foreach (var breakdown in item.InitiatedSectionDetailBreakdowns)
                                            {
                                                var result = sectionDetail.Breakdowns.Where(x => x.BreakdownId == breakdown.Id).SingleOrDefault();
                                                ItemBreakdownResult itemBreakdown = new ItemBreakdownResult(breakdown, result, detail.Id);
                                                context.ItemBreakdownResults.Add(itemBreakdown);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.AppraiseeSubmit = true;

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                    }
                    catch (Exception e)
                    {
                        dbContextTransaction.Rollback();
                        throw new System.ArgumentException(e.Message);
                    }
                }
            }

            
        }

        internal string StartEmployeeAppraisal(int employeeId, int newAppraisalId)
        {
            
            // get the employee
            Employee employee = db.Employees
                                        .Where(x => x.Id == employeeId)
                                        .Include(x => x.EmployeeQualifications)
                                        .Include(x => x.State)
                                        .Include(x => x.Department)
                                        .SingleOrDefault();

            // check if appraisal has been initialized before
            Appraisee appraisee = db.Appraisees.Where(x => x.EmployeeId == employeeId
                                                    && x.NewAppraisalId == newAppraisalId)
                                                .SingleOrDefault();
            if(appraisee != null)
            {
                return "exists";
            }

            // check if employee has a supervisor
            DefaultUserAppraiser userAppraiser = db.DefaultUserAppraisers
                                                    .Where(x => x.AppraiseeId == employee.ApplicationUserId)
                                                    .SingleOrDefault();
            if(userAppraiser.AppraiserId == "")
            {
                return "no appraiser";
            }

            // check if employee has an appraiser template

            // get the user appraisal template
            int templateId = 0;
            AppraisalUserTemplate userTemp = db.AppraisalUserTemplates
                                                    .Where(x => x.ApplicationUserId == employee.ApplicationUserId)
                                                    .SingleOrDefault();
            if (userTemp == null)
            {
                // get the department template
                AppraisalDepartmentTemplate deptTemp = db.AppraisalDepartmentTemplates
                                                            .Where(x => x.DepartmentId == employee.DepartmentId)
                                                            .SingleOrDefault();
                if (deptTemp != null)
                {
                    templateId = deptTemp.AppraisalTemplateId;
                }
            }
            else
            {
                templateId = userTemp.AppraisalTemplateId;
            }
            
            if(templateId == 0)
            {
                return "no template";
            }

            InitiatedAppraisalTemplate template = db.InitiatedAppraisalTemplates
                                                    .Where(x => x.NewAppraiserId == newAppraisalId
                                                            && x.AppraisalTemplateId == templateId)
                                                    .SingleOrDefault();

            // get the user appraiser record
            Employee appraiser = db.Employees
                                    .Where(x => x.ApplicationUserId == userAppraiser.AppraiserId)
                                    .Include(x => x.EmployeeQualifications)
                                    .Include(x => x.State)
                                    .Include(x => x.Department)
                                    .SingleOrDefault();

            // initialize the appraisal (i.e Appraisee model)
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // add the appraisee comments
                        AppraiseeComments comments = new AppraiseeComments();
                        context.AppraiseeComments.Add(comments);

                        // add the appraisee progress
                        AppraiseeProgress progress = new AppraiseeProgress();
                        context.AppraiseeProgresses.Add(progress);

                        // add the appraisee personal data
                        AppraiseePersonalData appraiseePD = new AppraiseePersonalData(employee.Id, employee.GradeId);
                        context.AppraiseePersonalDatas.Add(appraiseePD);

                        // add the appraiser personal data
                        AppraiserPersonalData appraiserPd = new AppraiserPersonalData(appraiser);
                        context.AppraiserPersonalDatas.Add(appraiserPd);
                        context.SaveChanges();

                        Appraisee newAppraisee = new Appraisee(newAppraisalId, employeeId, appraiseePD.Id, appraiserPd.Id, template.Id, progress.Id, comments.Id);
                        context.Appraisees.Add(newAppraisee);
                        context.SaveChanges();

                        // fetch all the career history in user profile
                        List<CareerHistory> careerHistories = context.CareerHistories
                                                                    .Where(x => x.EmployeeId == employeeId)
                                                                    .ToList();
                        if(careerHistories != null)
                        {
                            foreach (var history in careerHistories)
                            {
                                AppraiseeCareerHistoryWithCompany appraiseeCH = new AppraiseeCareerHistoryWithCompany(history, newAppraisee.Id);
                                context.appraiseeCareerHistoryWithCompanies.Add(appraiseeCH);
                            }
                        }

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return "initialized";
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return "failed";
                    }
                }
            }
            
            
        }

        internal List<AppraiseeAndProgress> GetAppraiseesAndProgress(string userId)
        {
            List<AppraiseeAndProgress> progresses = new List<AppraiseeAndProgress>();

            List<DefaultUserAppraiser> userAppraisers = db.DefaultUserAppraisers
                                                            .Where(x => x.AppraiserId == userId)
                                                            .ToList();
            foreach (var item in userAppraisers)
            {
                Employee employee = db.Employees.Where(x => x.ApplicationUserId == item.AppraiseeId)
                                                .Include(x => x.Department)
                                                .Include(x => x.State)
                                                .SingleOrDefault();
                List<AppraisalStaff> appraisalStaffs = db.AppraisalStaffs.Where(x => x.EmployeeId == employee.Id
                                                                && x.IsCompleted == false)
                                                                    .ToList();
                if(appraisalStaffs != null && appraisalStaffs.Count() > 0)
                {
                    foreach (var staff in appraisalStaffs)
                    {
                        Appraisee appraisee = db.Appraisees
                                                .Where(x => x.NewAppraisalId == staff.NewAppraisalId
                                                    && x.EmployeeId == staff.EmployeeId)
                                                .Include(x => x.AppraiseeProgress)
                                                .SingleOrDefault();
                        if(appraisee != null)
                        {
                            AppraiseeAndProgress appraiseeAndProgress = new AppraiseeAndProgress()
                            {
                                Employee = employee,
                                AppraiseeProgress = appraisee.AppraiseeProgress
                            };
                            progresses.Add(appraiseeAndProgress);
                        }
                    }
                }
            }
            return progresses;
        }

        internal List<Employee> GetMyAppraisees(string applicationUserId)
        {
            List<Employee> Appraisees = new List<Employee>();
            List<DefaultUserAppraiser> userAppraisers = db.DefaultUserAppraisers
                                                            .Where(x => x.AppraiserId == applicationUserId)
                                                            .ToList();
            foreach (var item in userAppraisers)
            {
                Employee employee = db.Employees.Where(x => x.ApplicationUserId == item.AppraiseeId)
                                                .Include(x => x.Department)
                                                .Include(x => x.State)
                                                .SingleOrDefault();
                Appraisees.Add(employee);
            }
            return Appraisees;
        }

        internal void UpdateAppraisal(NewAppraisalParams model)
        {
            // get the Appraisal
            NewAppraisal newAppraisal = db.NewAppraisals.Find(model.AppraisalId);
            newAppraisal.Update(model);

            // first if the number of selected staff is equal to the number in db then skip, else, update
            List<AppraisalStaff> appraisalStaffs = db.AppraisalStaffs
                                                        .Where(x => x.NewAppraisalId == newAppraisal.Id)
                                                        .ToList();
            if(model.SelectedEmployees != null && model.SelectedEmployees.Count() > 0)
            {
                if(model.SelectedEmployees.Count() != appraisalStaffs.Count())
                {
                    foreach (var staffAppraisal in appraisalStaffs)
                    {
                        if (!model.SelectedEmployees.Contains(staffAppraisal.EmployeeId))
                        {
                            Appraisee appraisee = db.Appraisees
                                                    .Where(x => x.EmployeeId == staffAppraisal.EmployeeId
                                                            && x.NewAppraisalId == newAppraisal.Id)
                                                    .SingleOrDefault();
                            if(appraisee != null)
                            {
                                db.Appraisees.Remove(appraisee);
                            }
                            db.AppraisalStaffs.Remove(staffAppraisal);
                        }
                    }
                }
            }
            else
            {
                foreach (var staffAppraisal in appraisalStaffs)
                {
                    Appraisee appraisee = db.Appraisees
                                            .Where(x => x.EmployeeId == staffAppraisal.EmployeeId
                                                    && x.NewAppraisalId == newAppraisal.Id)
                                            .SingleOrDefault();
                    if (appraisee != null)
                    {
                        db.Appraisees.Remove(appraisee);
                    }
                    db.AppraisalStaffs.Remove(staffAppraisal);
                }
            }
            if(model.NewSelectedEmployee != null && model.NewSelectedEmployee.Count() > 0)
            {
                foreach (var employeeId in model.NewSelectedEmployee)
                {
                    AppraisalStaff appraisalStaff = new AppraisalStaff(newAppraisal.Id, employeeId);
                    db.AppraisalStaffs.Add(appraisalStaff);
                }
            }
            db.SaveChanges();
        }

        internal AppraisalStaff GetEmployeeOngoingAppraisal(int employeeId)
        {
            return db.AppraisalStaffs.Where(x => x.EmployeeId == employeeId
                                            && x.IsCompleted == false)
                                    .FirstOrDefault();
        }

        internal Appraisee GetAppraisee(int employeeId, int newAppraisalId)
        {
            return db.Appraisees.Where(x => x.EmployeeId == employeeId
                                        && x.NewAppraisalId == newAppraisalId)
                                .Include(x => x.AppraiserPersonalData.Appraiser.Department)
                                .Include(x => x.AppraiserPersonalData.Appraiser.Grade)
                                .Include(x => x.AppraiseeCareerHistoryWithCompanies.Select(d => d.Department))
                                .Include(x => x.AppraiseeProgress)
                                .Include(x => x.AppraiseeRejections)
                                .Include(x => x.SectionResults
                                    .Select(d => d.SectionDetailResults.Select(b => b.ItemBreakdownResults)))
                                .SingleOrDefault();
        }

        internal List<NewAppraisal> GetAllInitiatedAppraisals()
        {
            return db.NewAppraisals
                    .Include(x => x.AppraisalStaffs)
                    .OrderByDescending(x => x.DateInitiated).ToList();
        }

        internal NewAppraisal GetNewAppraisalById(int newAppraisalId)
        {
            return db.NewAppraisals.Where(x => x.Id == newAppraisalId)
                                    .Include(x => x.AppraisalType)
                                    .SingleOrDefault();
        }

        internal NewAppraisal GetAppraisalBySlug(string slug)
        {
            return db.NewAppraisals.Where(x => x.Slug == slug)
                                    .Include(x => x.AppraisalDepartments.Select(d => d.Department))
                                    .Include(x => x.AppraisalLocations.Select(l => l.State))
                                    .Include(x => x.AppraisalType)
                                    .Include(x => x.AppraisalStaffs)
                                    .SingleOrDefault();
        }

        internal List<AppraisalParticipants> GetAppraisalParticipants(string slug)
        {
            List<AppraisalParticipants> appraisalParticipants = new List<AppraisalParticipants>();

            NewAppraisal appraisal = db.NewAppraisals.Where(x => x.Slug == slug).SingleOrDefault();
            List<AppraisalStaff> staffs = db.AppraisalStaffs
                                            .Where(x => x.NewAppraisalId == appraisal.Id)
                                            .ToList();
            foreach (var empAppraisal in staffs)
            {
                Employee employee = db.Employees.Where(x => x.Id == empAppraisal.EmployeeId)
                                                .Include(x => x.Department)
                                                .Include(x => x.State)
                                                .SingleOrDefault();
                // get the user appraisal template
                int templateId = 0;
                AppraisalUserTemplate userTemp = db.AppraisalUserTemplates
                                                        .Where(x => x.ApplicationUserId == employee.ApplicationUserId)
                                                        .SingleOrDefault();
                if(userTemp == null)
                {
                    // get the department template
                    AppraisalDepartmentTemplate deptTemp = db.AppraisalDepartmentTemplates
                                                                .Where(x => x.DepartmentId == employee.DepartmentId)
                                                                .SingleOrDefault();
                    if(deptTemp != null)
                    {
                        templateId = deptTemp.AppraisalTemplateId;
                    }
                }
                else
                {
                    templateId = userTemp.AppraisalTemplateId;
                }

                // initialize the template
                AppraisalTemplate template = new AppraisalTemplate();
                
                // check if a template was found for the employee
                if(templateId != 0)
                {
                    template = db.AppraisalTemplates.Find(templateId);
                }
                else
                {
                    template = null;
                }
                AppraisalParticipants participant = new AppraisalParticipants()
                {
                    Firstname = employee.Firstname,
                    Lastname = employee.Lastname,
                    Department = employee.Department.Name,
                    Location = employee.State.Description,
                    AppraisalTemplate = template
                };
                appraisalParticipants.Add(participant);
            }

            return appraisalParticipants;
        }
    }
}