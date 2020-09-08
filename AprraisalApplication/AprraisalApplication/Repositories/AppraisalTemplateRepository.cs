using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Repositories
{
    public class AppraisalTemplateRepository
    {
        private readonly ApplicationDbContext db;

        public AppraisalTemplateRepository(ApplicationDbContext context)
        {
            db = context;
        }

        internal void SaveNewAppraisalTemplate(AppraisalTemplateParams model)
        {
            AppraisalTemplate newAppraisalTemplate = new AppraisalTemplate(model);
            db.AppraisalTemplates.Add(newAppraisalTemplate);
            db.SaveChanges();
            var appraisalId = newAppraisalTemplate.Id;
            List<DefaultRating> defaultRatings = db.DefaultRatings.ToList();
            foreach (var item in defaultRatings)
            {
                TemplateRating templateRating = new TemplateRating(item, appraisalId);
                db.TemplateRatings.Add(templateRating);
            }

            List<DefaultSummaryRating> defaultSummaries = db.DefaultSummaryRatings.ToList();
            foreach (var item in defaultSummaries)
            {
                TemplateSummaryRating templateSummary = new TemplateSummaryRating(item, appraisalId);
                db.TemplateSummaryRatings.Add(templateSummary);
            }

            // save the section
            foreach (var item in model.AppraisalSectionParams)
            {
                AppraisalTemplateSection templateSection = new AppraisalTemplateSection(item, appraisalId);
                db.AppraisalTemplateSections.Add(templateSection);
                db.SaveChanges();
                if(item.SectionTypeId == 4 || item.SectionTypeId == 3)
                {
                    if(item.QualitativeDetails != null && item.QualitativeDetails.Count() > 0)
                    {
                        foreach (var qualitativeDetail in item.QualitativeDetails)
                        {
                            AppraisalSectionDetail detail = new AppraisalSectionDetail(qualitativeDetail, templateSection.Id);
                            db.AppraisalSectionDetails.Add(detail);
                            db.SaveChanges();
                            if(item.SectionTypeId == 3)
                            {
                                if(qualitativeDetail.ItemBreakDowns != null && qualitativeDetail.ItemBreakDowns.Count() > 0)
                                {
                                    foreach (var itemBreakdown in qualitativeDetail.ItemBreakDowns)
                                    {
                                        SectionDetailBreakdown breakdown = new SectionDetailBreakdown(itemBreakdown, detail.Id);
                                        db.SectionDetailBreakdowns.Add(breakdown);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            db.SaveChanges();
        }

        internal void UpdateUserAppraisal(AppraisalUserParams model)
        {
            foreach (var item in model.Items)
            {
                if (item.TemplateId != null)
                {
                    AppraisalUserTemplate userTemp = db.AppraisalUserTemplates
                                                            .Where(x => x.ApplicationUserId == item.ApplicationUserId).SingleOrDefault();
                    if (userTemp == null)
                    {
                        AppraisalUserTemplate newUserTemp = new AppraisalUserTemplate(item);
                        db.AppraisalUserTemplates.Add(newUserTemp);
                    }
                    else
                    {
                        userTemp.AppraisalTemplateId = (int)item.TemplateId;
                    }
                }
            }
            db.SaveChanges();
        }

        internal void UpdateDepartmentAppraisals(AppraisalDepartmentParams model)
        {
            foreach (var item in model.Items)
            {
                if(item.TemplateId != null)
                {
                    AppraisalDepartmentTemplate deptTemp = db.AppraisalDepartmentTemplates
                                                            .Where(x => x.DepartmentId == item.DepartmentId).SingleOrDefault();
                    if(deptTemp == null)
                    {
                        AppraisalDepartmentTemplate newDeptTemp = new AppraisalDepartmentTemplate(item);
                        db.AppraisalDepartmentTemplates.Add(newDeptTemp);
                    }
                    else
                    {
                        deptTemp.AppraisalTemplateId = (int)item.TemplateId;
                    }
                }
            }
            db.SaveChanges();
        }

        internal void DeleteAppraisalTemplate(int templateId)
        {
            AppraisalTemplate template = db.AppraisalTemplates.Find(templateId);
            if(template != null)
            {
                template.IsDeleted = true;
                db.SaveChanges();
                foreach (var section in template.AppraisalTemplateSections)
                {
                    DeleteAppraisalSection(section.Id);
                }
            }
        }

        internal void DeleteSectionBreakdown(int breakdownId)
        {
            SectionDetailBreakdown breakdown = db.SectionDetailBreakdowns.Find(breakdownId);
            if(breakdown != null)
            {
                breakdown.IsDeleted = true;
                db.SaveChanges();
            }
        }

        internal void DeleteSectionDetail(int detailId)
        {
            AppraisalSectionDetail detail = db.AppraisalSectionDetails.Where(x => x.Id == detailId)
                                                .Include(x => x.SectionDetailBreakdowns)
                                                .SingleOrDefault();
            if(detail != null)
            {
                detail.IsDeleted = true;
                if(detail.SectionDetailBreakdowns != null && detail.SectionDetailBreakdowns.Count() > 0)
                {
                    foreach (var breakdown in detail.SectionDetailBreakdowns)
                    {
                        breakdown.IsDeleted = true;
                    }
                }
                db.SaveChanges();
            }
        }

        internal InitiatedAppraisalTemplate GetInitiatedAppraisalTemplateById(int initiatedAppraisalTemplateId)
        {
            return db.InitiatedAppraisalTemplates.Where(x => x.Id == initiatedAppraisalTemplateId)
                                        .Include(x => x.InitiatedTemplateSections
                                            .Select(d => d.InitiatedSectionDetails
                                            .Select(p => p.InitiatedSectionDetailBreakdowns)))
                                        .SingleOrDefault();
        }

        internal AppraisalTemplate GetAppraisalTemplateById(int appraisalTemplateId)
        {
            return db.AppraisalTemplates.Where(x => x.Id == appraisalTemplateId)
                                        .Include(x => x.AppraisalTemplateSections
                                            .Select(d => d.AppraisalSectionDetails
                                            .Select(p => p.SectionDetailBreakdowns)))
                                        .SingleOrDefault();
        }

        internal List<AppraisalUserTemplate> GetAppraisalUserTemplates()
        {
            return db.AppraisalUserTemplates.Include(x => x.ApplicationUser)
                                            .Include(x => x.AppraisalTemplate)
                                            .ToList();
        }

        internal List<AppraisalDepartmentTemplate> GetAppraisalDepartmentTemplates()
        {
            return db.AppraisalDepartmentTemplates
                        .Include(x => x.Department)
                        .Include(x => x.AppraisalTemplate)
                        .ToList();
        }

        internal void DeleteAppraisalSection(int sectionId)
        {
            AppraisalTemplateSection section = db.AppraisalTemplateSections.Where(x => x.Id == sectionId)
                                                    .Include(x => x.AppraisalSectionDetails
                                                    .Select(b => b.SectionDetailBreakdowns))
                                                    .SingleOrDefault();
            section.IsDeleted = true;
            if(section.AppraisalSectionDetails != null && section.AppraisalSectionDetails.Count()> 0)
            {
                foreach (var detail in section.AppraisalSectionDetails)
                {
                    detail.IsDeleted = true;
                    if(detail.SectionDetailBreakdowns != null && detail.SectionDetailBreakdowns.Count() > 0)
                    {
                        foreach (var item in detail.SectionDetailBreakdowns)
                        {
                            item.IsDeleted = true;
                        }
                    }
                }
            }
            db.SaveChanges();
        }

        internal void EditAppraisalTemplate(AppraisalTemplateParams model)
        {
            AppraisalTemplate template = db.AppraisalTemplates.Find(model.TemplateId);
            if(template != null)
            {
                template.UpdateTemplate(model);

                // update the section
                foreach (var item in model.AppraisalSectionParams)
                {
                    if(item.SectionId == null)
                    {
                        AppraisalTemplateSection templateSection = new AppraisalTemplateSection(item, model.TemplateId);
                        db.AppraisalTemplateSections.Add(templateSection);
                        db.SaveChanges();
                        if (item.SectionTypeId == 4 || item.SectionTypeId == 3)
                        {
                            if (item.QualitativeDetails != null && item.QualitativeDetails.Count() > 0)
                            {
                                foreach (var qualitativeDetail in item.QualitativeDetails)
                                {
                                    AppraisalSectionDetail detail = new AppraisalSectionDetail(qualitativeDetail, templateSection.Id);
                                    db.AppraisalSectionDetails.Add(detail);
                                    db.SaveChanges();
                                    if (item.SectionTypeId == 3)
                                    {
                                        if (qualitativeDetail.ItemBreakDowns != null && qualitativeDetail.ItemBreakDowns.Count() > 0)
                                        {
                                            foreach (var itemBreakdown in qualitativeDetail.ItemBreakDowns)
                                            {
                                                SectionDetailBreakdown breakdown = new SectionDetailBreakdown(itemBreakdown, detail.Id);
                                                db.SectionDetailBreakdowns.Add(breakdown);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        AppraisalTemplateSection section = db.AppraisalTemplateSections.Find(item.SectionId);
                        if(section != null)
                        {
                            section.Update(item);

                            // get all the details
                            if(item.SectionTypeId == 4 || item.SectionTypeId == 3)
                            {
                                if (item.QualitativeDetails != null && item.QualitativeDetails.Count() > 0)
                                {
                                    foreach (var qualitativeDetail in item.QualitativeDetails)
                                    {
                                        if(qualitativeDetail.DetailId == null)
                                        {
                                            AppraisalSectionDetail detail = new AppraisalSectionDetail(qualitativeDetail, section.Id);
                                            db.AppraisalSectionDetails.Add(detail);
                                            db.SaveChanges();
                                            if (item.SectionTypeId == 3)
                                            {
                                                if (qualitativeDetail.ItemBreakDowns != null && qualitativeDetail.ItemBreakDowns.Count() > 0)
                                                {
                                                    foreach (var itemBreakdown in qualitativeDetail.ItemBreakDowns)
                                                    {
                                                        SectionDetailBreakdown breakdown = new SectionDetailBreakdown(itemBreakdown, detail.Id);
                                                        db.SectionDetailBreakdowns.Add(breakdown);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            AppraisalSectionDetail detail = db.AppraisalSectionDetails.Find(qualitativeDetail.DetailId);
                                            detail.Update(qualitativeDetail);

                                            if (item.SectionTypeId == 3)
                                            {
                                                if (qualitativeDetail.ItemBreakDowns != null && qualitativeDetail.ItemBreakDowns.Count() > 0)
                                                {
                                                    foreach (var itemBreakdown in qualitativeDetail.ItemBreakDowns)
                                                    {
                                                        if(itemBreakdown.BreakdownId == null)
                                                        {
                                                            SectionDetailBreakdown breakdown = new SectionDetailBreakdown(itemBreakdown, detail.Id);
                                                            db.SectionDetailBreakdowns.Add(breakdown);
                                                        }
                                                        else
                                                        {
                                                            SectionDetailBreakdown breakdown = db.SectionDetailBreakdowns.Find(itemBreakdown.BreakdownId);
                                                            breakdown.Update(itemBreakdown);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            db.SaveChanges();
        }

        internal AppraisalTemplate GetAppraisalTemplateBySlug(string slug)
        {
            return db.AppraisalTemplates.Where(x => x.Slug == slug)
                                        .Include(x => x.AppraisalTemplateSections
                                            .Select(d => d.AppraisalSectionDetails
                                            .Select(p => p.SectionDetailBreakdowns)))
                                        .SingleOrDefault();
        }

        internal List<AppraisalTemplate> GetAllTemplates()
        {
            return db.AppraisalTemplates.Where(x => x.IsDeleted == false)
                .OrderByDescending(x => x.DateCreated)
                .ToList();
        }
    }
}