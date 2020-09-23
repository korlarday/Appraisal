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

        internal NewAppraisal GetNewAppraisalByTitle(string appraisalTitle)
        {
            return db.NewAppraisals.Where(x => x.AppraisalTitle == appraisalTitle).FirstOrDefault();
        }

        internal void RejectAppraisalToHod(SectionScoresParams model)
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

                        //AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id, "hr");
                        //context.AppraiseeRejections.Add(rejection);

                        string rejectTo = "";
                        // update the appraisee progress
                        var progress = appraisee.AppraiseeProgress;
                        if (model.RejectionType == "hod")
                        {
                            progress.HRReject = true;
                            progress.HODSubmit = false;
                            rejectTo = PositionsCS.Hod;
                        }
                        else if (model.RejectionType == "supervisor")
                        {
                            progress.HODReject = true;
                            progress.HODSubmit = false;
                            progress.SupervisorSubmit = false;
                            rejectTo = PositionsCS.Supervisor;
                        }
                        else
                        {
                            // if rejection is to appraisee
                            progress.SupervisorReject = true;
                            progress.FeedbackFromAppraisee = false;
                            progress.SupervisorSubmit = false;
                            progress.HODSubmit = false;
                            rejectTo = PositionsCS.Appraisee;
                        }

                        AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id, PositionsCS.Hr, rejectTo);
                        context.AppraiseeRejections.Add(rejection);

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

        internal void SaveMdComment(SubmitAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // get the MD userId
                        string userId = HttpContext.Current.User.Identity.GetUserId();
                        int employeeId = context.Employees.Where(x => x.ApplicationUserId == userId).SingleOrDefault().Id;

                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.AppraiseeProgress)
                                                            .Include(x => x.AppraiseeComments)
                                                            .SingleOrDefault();

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.MDAcknowledgement = true;

                        // save the progress
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.MdComment = model.MdComment;
                        comments.MdCommentDate = DateTime.Now;
                        comments.MdEmployeeId = employeeId;

                        
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

        internal BdsPerformanceTracker GetBdsTracker(int? bdsTrackerId)
        {
            return db.BdsPerformanceTrackers.Find(bdsTrackerId);
        }

        internal List<NewAppraisal> GetEmployeeCompletedAppraisals(int employeeId)
        {
            List<AppraisalStaff> appraisalStaffs = db.AppraisalStaffs.Where(x => x.EmployeeId == employeeId
                                                                            && x.IsCompleted == true)
                                                                    .ToList();
            List<NewAppraisal> newAppraisals = new List<NewAppraisal>();
            foreach (var item in appraisalStaffs)
            {
                NewAppraisal newAppraisal = db.NewAppraisals.Find(item.NewAppraisalId);
                newAppraisals.Add(newAppraisal);
            }
            return newAppraisals;
        }

        internal void SaveHrComment(SubmitAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // get the hod userId
                        string userId = HttpContext.Current.User.Identity.GetUserId();
                        int employeeId = context.Employees.Where(x => x.ApplicationUserId == userId).SingleOrDefault().Id;

                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.AppraiseeProgress)
                                                            .Include(x => x.AppraiseeComments)
                                                            .SingleOrDefault();

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.HRSubmit = true;

                        // save the progress
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.HrComment = model.HrComment;
                        comments.HrCommentDate = DateTime.Now;
                        comments.HrEmployeeId = employeeId;

                        // change employee appraisal status to completed
                        // get the newappraisal
                        NewAppraisal appraisal = db.NewAppraisals.Where(x => x.Id == appraisee.NewAppraisalId)
                                                                .Include(x => x.AppraisalStaffs)
                                                                .SingleOrDefault();
                        var appraisalStaffs = appraisal.AppraisalStaffs;
                        var appraisalStaff = appraisalStaffs.Where(x => x.EmployeeId == appraisee.EmployeeId).FirstOrDefault();
                        appraisalStaffs.Where(x => x.Id == appraisalStaff.Id).SingleOrDefault().IsCompleted = true;
                        var AppraisalStaffDb = context.AppraisalStaffs.Find(appraisalStaff.Id);
                        AppraisalStaffDb.IsCompleted = true;
                        context.SaveChanges();

                        // check if it was the last appraisal, if yes change the main appraisal status to completed
                        var isCompleted = true;
                        foreach (var item in appraisalStaffs)
                        {
                            if(item.IsCompleted == false)
                            {
                                isCompleted = false;
                                break;
                            }
                        }
                        if (isCompleted)
                        {
                            NewAppraisal newAppraisal = context.NewAppraisals.Find(appraisal.Id);
                            newAppraisal.IsCompleted = true;
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

        internal void ReScoreAppraisalSections(SectionScoresParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.SectionResults.Select(d => d.SectionDetailResults))
                                                            .Include(x => x.AppraiseeProgress)
                                                            .Include(x => x.AppraiseeComments)
                                                            .Include(x => x.AppraiseeRejections)
                                                            .SingleOrDefault();

                        var sectionResults = appraisee.SectionResults.ToList();
                        foreach (var sectionResult in sectionResults)
                        {
                            var sectionScore = model.SectionScoresResults.Where(x => x.SectionResultId == sectionResult.Id).SingleOrDefault();
                            if (sectionScore != null)
                            {
                                sectionResult.TotalScore = sectionScore.SectionTotalScore;
                                sectionResult.PercentageScore = sectionScore.SectionPercentageScore;
                                foreach (var detail in sectionResult.SectionDetailResults)
                                {
                                    var detailScore = sectionScore.SectionDetailsScore.Where(x => x.SectionResultDetailId == detail.Id).SingleOrDefault();
                                    if (detailScore != null)
                                    {
                                        detail.Score = detailScore.Score;
                                    }
                                }

                            }
                        }

                        // update the appraisee progress
                        var progress = appraisee.AppraiseeProgress;
                        progress.SupervisorReject = false;
                        progress.SupervisorSubmit = true;
                        progress.HODReject = false;

                        // update the appraiser comments
                        var comments = appraisee.AppraiseeComments;
                        comments.AppraiserComment = model.AppraiserComment;
                        comments.AppraiserCommentDate = DateTime.Now;

                        // update the rejection status
                        var rejections = appraisee.AppraiseeRejections.Where(x => x.New == true).ToList();
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

        internal void RejectAppraisalToSupervisor(SectionScoresParams model)
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


                        // update the appraisee progress
                        //check to whom the appraiser was rejected to
                        var rejectTo = "";
                        var progress = appraisee.AppraiseeProgress;
                        if(model.RejectionType == "supervisor")
                        {
                            progress.HODReject = true;
                            progress.HODSubmit = false;
                            progress.SupervisorSubmit = false;
                            rejectTo = PositionsCS.Supervisor;
                        }
                        else
                        {
                            // if rejection is to appraisee
                            progress.SupervisorReject = true;
                            progress.FeedbackFromAppraisee = false;
                            progress.SupervisorSubmit = false;
                            rejectTo = PositionsCS.Appraisee;
                        }

                        AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id, PositionsCS.Hod, rejectTo);
                        context.AppraiseeRejections.Add(rejection);

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

        internal void RejectAppraisalCommentToAppraisee(SectionScoresParams model)
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

                        AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id, PositionsCS.Supervisor, PositionsCS.Appraisee);
                        context.AppraiseeRejections.Add(rejection);

                        // update the appraisee progress

                        var progress = appraisee.AppraiseeProgress;
                        if(model.RejectionType == "comments")
                        {
                            // send the form back to appraisee for comments
                            progress.SupervisorReject = false;
                            progress.SupervisorSubmit = false;
                            progress.SupervisorAskForFeedback = true;
                            progress.HODReject = false;
                        }
                        else
                        {
                            // restart the process
                            progress.SupervisorReject = true;
                            progress.FeedbackFromAppraisee = false;
                            progress.SupervisorSubmit = false;
                            progress.HODReject = false;
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
        internal void SaveHodComment(SubmitAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // get the hod userId
                        string userId = HttpContext.Current.User.Identity.GetUserId();
                        int employeeId = context.Employees.Where(x => x.ApplicationUserId == userId).SingleOrDefault().Id;

                        // first get the appraisee
                        Appraisee appraisee = context.Appraisees.Where(x => x.Id == model.AppraiseeId)
                                                            .Include(x => x.AppraiseeProgress)
                                                            .Include(x => x.AppraiseeComments)
                                                            .Include(x => x.AppraiseeRejections)
                                                            .SingleOrDefault();

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.HODSubmit = true;
                        progress.HRReject = false;

                        // save the progress
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.HodComment = model.HodComment;
                        comments.HodCommentDate = DateTime.Now;
                        comments.HodEmployeeId = employeeId;

                        // change the rejection status
                        List<AppraiseeRejection> rejections = appraisee.AppraiseeRejections.Where(x => x.New == true).ToList();
                        if (rejections != null && rejections.Count() > 0)
                        {
                            foreach (var item in rejections)
                            {
                                item.New = false;
                            }
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


        internal List<DepartmentAndParticipants> GetAppraisalDeptAndParticipants(int newAppraisalId)
        {
            NewAppraisal newAppraisal = db.NewAppraisals.Where(x => x.Id == newAppraisalId)
                                                                .Include(x => x.AppraisalStaffs
                                                                .Select(e => e.Employee.Department))
                                                                .SingleOrDefault();
            List<Department> departments = db.Departments.ToList();
            List<DepartmentAndParticipants> departmentAndParticipants = new List<DepartmentAndParticipants>();
            
            var participants = newAppraisal.AppraisalStaffs.ToList();
            foreach (var department in departments)
            {
                int participantsCount = participants.Where(x => x.Employee.DepartmentId == department.Id).Count();
                DepartmentAndParticipants deptAndParticipants = new DepartmentAndParticipants
                {
                    Department = department,
                    NumberOfParticipants = participantsCount
                };
                departmentAndParticipants.Add(deptAndParticipants);
            }
            return departmentAndParticipants;
        }

        internal List<AppraiseeAndProgress> GetDepartmentAppraisees(Employee employee, NewAppraisal appraisal)
        {
            List<AppraiseeAndProgress> appraiseeAndProgresses = new List<AppraiseeAndProgress>();
            var appraisalStaffs = appraisal.AppraisalStaffs;
            foreach (var staff in appraisalStaffs)
            {
                Employee emp = db.Employees.Where(x => x.Id == staff.EmployeeId)
                                            .Include(x => x.Department)
                                            .Include(x => x.State)
                                            .SingleOrDefault();
                if(emp.DepartmentId == employee.DepartmentId)
                {
                    Appraisee appraisee = db.Appraisees.Where(x => x.NewAppraisalId == appraisal.Id
                                                                && x.EmployeeId == emp.Id)
                                                        .Include(x => x.AppraiseeProgress)
                                                        .SingleOrDefault();
                    AppraiseeAndProgress appraiseeAndProgress = new AppraiseeAndProgress
                    {
                        Employee = emp,
                        AppraiseeProgress = appraisee == null ? null : appraisee.AppraiseeProgress,
                        NewAppraisal = db.NewAppraisals.Find(staff.NewAppraisalId)
                    };
                    appraiseeAndProgresses.Add(appraiseeAndProgress);
                }                         
            }
            return appraiseeAndProgresses;
        }

        internal List<AppraiseeAndProgress> GetAppraiseesAndProgressInDepartment(Department department, NewAppraisal appraisal)
        {
            List<AppraiseeAndProgress> appraiseeAndProgresses = new List<AppraiseeAndProgress>();
            var appraisalStaffs = appraisal.AppraisalStaffs;
            foreach (var staff in appraisalStaffs)
            {
                
                if (staff.Employee.DepartmentId == department.Id)
                {
                    Appraisee appraisee = db.Appraisees.Where(x => x.NewAppraisalId == appraisal.Id
                                                                && x.EmployeeId == staff.EmployeeId)
                                                        .Include(x => x.AppraiseeProgress)
                                                        .SingleOrDefault();
                    AppraiseeAndProgress appraiseeAndProgress = new AppraiseeAndProgress
                    {
                        Employee = staff.Employee,
                        AppraiseeProgress = appraisee == null ? null : appraisee.AppraiseeProgress
                    };
                    appraiseeAndProgresses.Add(appraiseeAndProgress);
                }
            }
            return appraiseeAndProgresses;
        }

        internal List<NewAppriasalAndParticipants> GetDepartmentInitiatedAppraisals(int departmentId)
        {
            List<NewAppraisal> newAppraisals = db.NewAppraisals.Include(x => x.AppraisalStaffs
                                                                            .Select(e => e.Employee))
                                                                .OrderByDescending(x => x.DateInitiated)
                                                                .ToList();
            List<NewAppriasalAndParticipants> newAppriasalAndParticipants = new List<NewAppriasalAndParticipants>();
            foreach (var newAppraisal in newAppraisals)
            {
                int count = 0;
                var participants = newAppraisal.AppraisalStaffs.ToList();
                foreach (var participant in participants)
                {
                    if(participant.Employee.DepartmentId == departmentId)
                    {
                        count += 1;
                    }
                }
                NewAppriasalAndParticipants appAndparts = new NewAppriasalAndParticipants
                {
                    NewAppraisal = newAppraisal,
                    NumberOfParticipants = count
                };
                newAppriasalAndParticipants.Add(appAndparts);
            }
            return newAppriasalAndParticipants;
        }

        internal void SaveAppraiserComment(SubmitAppraisalParams model)
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
                                                            .Include(x => x.AppraiseeComments)
                                                            .SingleOrDefault();

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.SupervisorSubmit = true;

                        // save the progress
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.AppraiserComment = model.AppraiserComment;
                        comments.AppraiserCommentDate = DateTime.Now;


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

        internal void SaveAppraiseeComment(SubmitAppraisalParams model)
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
                                                            .Include(x => x.AppraiseeComments)
                                                            .Include(x => x.AppraiseeRejections)
                                                            .SingleOrDefault();
                        
                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.SupervisorAskForFeedback = false;
                        progress.FeedbackFromAppraisee = true;

                        // save the progress
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.AppraiseeComment = model.AppraiseeComment;
                        comments.AppraiseeCommentDate = DateTime.Now;

                        // change the rejection status
                        List<AppraiseeRejection> rejections = appraisee.AppraiseeRejections.Where(x => x.New == true).ToList();
                        if(rejections != null && rejections.Count() > 0)
                        {
                            foreach (var item in rejections)
                            {
                                item.New = false;
                            }
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
                                                            .Include(x => x.AppraiseeComments)
                                                            .SingleOrDefault();
                        // update the task performed and the duties assigned
                        foreach (var section in appraisee.InitiatedAppraisalTemplate.InitiatedTemplateSections)
                        {
                            if(section.IsDeleted == false)
                            {
                                var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                                
                                // update section result
                                Models.MigrationModels.SectionResult sectionResultDb = context.SectionResults.Where(x => x.AppraiseeId == appraisee.Id
                                                                                                            && x.InitiatedTemplateSectionId == section.Id)
                                                                                                        .SingleOrDefault();
                                sectionResultDb.SectionFilled = sectionResult.OptionSelected;

                                if (section.SectionTypeId == SectionTypeCS.TaskPerfomed)
                                {
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
                                                        if(itemBreakdown != null)
                                                        {
                                                            itemBreakdown.Value = breakdown.BreakdownValue;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                        
                            }
                        }

                        // save bds tracker performance
                        if (appraisee.InitiatedAppraisalTemplate.IncludeBdsTracker)
                        {
                            BdsPerformanceTracker tracker = context.BdsPerformanceTrackers.Find(appraisee.BdsPerformanceTrackerId);
                            if (tracker != null)
                            {
                                tracker.UpdateTrackerAppraisee(model.BdsTracker);
                            }
                        }

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.SupervisorReject = false;

                        // save the comment
                        AppraiseeComments comments = appraisee.AppraiseeComments;
                        comments.AppraiseeComment = model.AppraiseeComment;

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

                        AppraiseeRejection rejection = new AppraiseeRejection(model.AppraiseeId, model.RejectionReason, employee.Id, PositionsCS.Supervisor, PositionsCS.Appraisee);
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
                                                            .Include(x => x.InitiatedAppraisalTemplate)
                                                            .Include(x => x.SectionResults.Select(d => d.SectionDetailResults))
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
                                        if(sectionScore.SectionTypeId == 2)
                                        {
                                            detail.Title2 = detailScore.ResultAchieved;
                                        }
                                        detail.Score = detailScore.Score;
                                        if(detailScore.Breakdowns != null && detailScore.Breakdowns.Count() > 0)
                                        {
                                            foreach (var breakdown in detailScore.Breakdowns)
                                            {
                                                ItemBreakdownResult result = context.ItemBreakdownResults.Find(breakdown.BreakdownId);
                                                if (result != null)
                                                {
                                                    result.Value = breakdown.BreakdownValue;
                                                }
                                            }
                                        }
                                    }
                                }

                            }
                        }

                        // if appraisal has bds tracker
                        if (appraisee.InitiatedAppraisalTemplate.IncludeBdsTracker)
                        {
                            BdsPerformanceTracker tracker = context.BdsPerformanceTrackers.Find(appraisee.BdsPerformanceTrackerId);
                            if(tracker != null)
                            {
                                tracker.UpdateTrackerAppraiser(model.BdsTracker);
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
                            var sectionResult = model.SectionResults.Where(x => x.SectionId == section.Id).SingleOrDefault();
                            
                            // store section result
                            Models.MigrationModels.SectionResult sectionResultDb = new Models.MigrationModels.SectionResult(appraisee.Id, section.Id, section.Optional, sectionResult.OptionSelected);
                            context.SectionResults.Add(sectionResultDb);
                            context.SaveChanges();

                            if (section.SectionTypeId == SectionTypeCS.TaskPerfomed)
                            {
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
                                            if(sectionDetail.Breakdowns != null)
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
                        }

                        // save the progress
                        AppraiseeProgress progress = appraisee.AppraiseeProgress;
                        progress.AppraiseeSubmit = true;

                        // save bds tracker performance
                        if (appraisee.InitiatedAppraisalTemplate.IncludeBdsTracker)
                        {
                            BdsPerformanceTracker tracker = context.BdsPerformanceTrackers.Find(appraisee.BdsPerformanceTrackerId);
                            if(tracker != null)
                            {
                                tracker.UpdateTrackerAppraisee(model.BdsTracker);
                            }
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

                        BdsPerformanceTracker tracker = new BdsPerformanceTracker();
                        if (template.IncludeBdsTracker)
                        {
                            // if bds tracker is included
                            context.BdsPerformanceTrackers.Add(tracker);
                        }

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

                        // check if bds tracker is included 
                        int? trackerId = null;

                        if (template.IncludeBdsTracker)
                        {
                            trackerId = tracker.Id;
                        }

                        Appraisee newAppraisee = new Appraisee(
                                                        newAppraisalId, 
                                                        employeeId, 
                                                        appraiseePD.Id, 
                                                        appraiserPd.Id, 
                                                        template.Id, 
                                                        progress.Id, 
                                                        comments.Id,
                                                        trackerId);
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
                                AppraiseeProgress = appraisee.AppraiseeProgress,
                                NewAppraisal = db.NewAppraisals.Find(appraisee.NewAppraisalId)
                            };
                            progresses.Add(appraiseeAndProgress);
                        }
                    }
                }
            }
            return progresses;
        }

        internal List<AppraiseeAndProgress> GetMyOngoingAppraisalsAndProgress(Employee employee)
        {
            List<AppraiseeAndProgress> progresses = new List<AppraiseeAndProgress>();

            // get my ongoing appraisals; you can identity that by the appraisalStaffs where the iscompleted is false
            List<AppraisalStaff> appraisalStaffs = db.AppraisalStaffs.Where(x => x.EmployeeId == employee.Id
                                                                && x.IsCompleted != true)
                                                                    .ToList();

            if (appraisalStaffs != null && appraisalStaffs.Count() > 0)
            {
                foreach (var staff in appraisalStaffs)
                {
                    Appraisee appraisee = db.Appraisees
                                            .Where(x => x.NewAppraisalId == staff.NewAppraisalId
                                                && x.EmployeeId == staff.EmployeeId)
                                            .Include(x => x.AppraiseeProgress)
                                            .SingleOrDefault();
                    AppraiseeAndProgress appraiseeAndProgress = new AppraiseeAndProgress()
                    {
                        Employee = employee,
                        AppraiseeProgress = appraisee == null ? null : appraisee.AppraiseeProgress,
                        NewAppraisal = db.NewAppraisals.Find(staff.NewAppraisalId)
                    };
                    progresses.Add(appraiseeAndProgress);
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

        internal string UpdateAppraisal(NewAppraisalParams model)
        {
            using (var context = new ApplicationDbContext())
            {
                using (var dbContextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // first check if the new title is unique
                        if (context.NewAppraisals.Where(x => x.Id != model.AppraisalId).Any(x => x.AppraisalTitle == model.AppraisalTitle))
                        {
                            return "title exists";
                        }
                        // get the Appraisal
                        NewAppraisal newAppraisal = context.NewAppraisals.Find(model.AppraisalId);
                        newAppraisal.Update(model);

                        // first if the number of selected staff is equal to the number in db then skip, else, update
                        List<AppraisalStaff> appraisalStaffs = context.AppraisalStaffs
                                                                    .Where(x => x.NewAppraisalId == newAppraisal.Id)
                                                                    .ToList();
                        if (model.SelectedEmployees != null && model.SelectedEmployees.Count() > 0)
                        {
                            foreach (var staffAppraisal in appraisalStaffs)
                            {
                                if (!model.SelectedEmployees.Contains(staffAppraisal.EmployeeId))
                                {
                                    Appraisee appraisee = context.Appraisees
                                                            .Where(x => x.EmployeeId == staffAppraisal.EmployeeId
                                                                    && x.NewAppraisalId == newAppraisal.Id)
                                                            .Include(x => x.SectionResults.Select(d => d.SectionDetailResults))
                                                            .SingleOrDefault();
                                    if (appraisee != null)
                                    {
                                        foreach (var sectionResult in appraisee.SectionResults)
                                        {
                                            foreach (var detailResult in sectionResult.SectionDetailResults)
                                            {
                                                context.SectionDetailResults.Remove(detailResult);
                                                context.SaveChanges();
                                            }
                                            context.SectionResults.Remove(sectionResult);
                                            context.SaveChanges();
                                        }
                                        context.Appraisees.Remove(appraisee);
                                    }
                                    context.AppraisalStaffs.Remove(staffAppraisal);
                                }
                            }

                        }
                        else
                        {
                            foreach (var staffAppraisal in appraisalStaffs)
                            {
                                Appraisee appraisee = context.Appraisees
                                                        .Where(x => x.EmployeeId == staffAppraisal.EmployeeId
                                                                && x.NewAppraisalId == newAppraisal.Id)
                                                        .SingleOrDefault();
                                if (appraisee != null)
                                {
                                    foreach (var sectionResult in appraisee.SectionResults)
                                    {
                                        foreach (var detailResult in sectionResult.SectionDetailResults)
                                        {
                                            context.SectionDetailResults.Remove(detailResult);
                                            context.SaveChanges();
                                        }
                                        context.SectionResults.Remove(sectionResult);
                                        context.SaveChanges();
                                    }
                                    context.Appraisees.Remove(appraisee);
                                }
                                context.AppraisalStaffs.Remove(staffAppraisal);
                            }
                        }
                        if (model.NewSelectedEmployee != null && model.NewSelectedEmployee.Count() > 0)
                        {
                            foreach (var employeeId in model.NewSelectedEmployee)
                            {
                                AppraisalStaff appraisalStaff = new AppraisalStaff(newAppraisal.Id, employeeId);
                                context.AppraisalStaffs.Add(appraisalStaff);
                            }
                        }
                        

                        context.SaveChanges();
                        dbContextTransaction.Commit();
                        return "success";
                    }
                    catch (Exception)
                    {
                        dbContextTransaction.Rollback();
                        return "failed";
                    }
                }
            }

            
        }

        internal AppraisalStaff GetEmployeeOngoingAppraisal(int employeeId, int newAppraisalId)
        {
            return  db.AppraisalStaffs.Where(x => x.EmployeeId == employeeId
                                                    && x.NewAppraisalId == newAppraisalId
                                                    && x.IsCompleted == false)
                                                        .SingleOrDefault();
           
        }
        internal List<AppraisalStaff> GetEmployeeOngoingAppraisals(int employeeId)
        {
            return db.AppraisalStaffs.Where(x => x.EmployeeId == employeeId && x.IsCompleted == false).ToList();
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
                                .Include(x => x.AppraiseeComments)
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
                                    .Include(x => x.AppraisalStaffs.Select(e => e.Employee.Department))
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