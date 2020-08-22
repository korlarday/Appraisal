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