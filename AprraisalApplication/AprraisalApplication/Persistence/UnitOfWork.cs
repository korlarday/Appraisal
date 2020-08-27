using AprraisalApplication.Models;
using AprraisalApplication.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AprraisalApplication.Persistence
{
    public class UnitOfWork
    {
        private readonly ApplicationDbContext context;
        public AccountRepository Account { get; private set; }
        public ResourcesRepository Resources { get; private set; }
        public AppraisalTemplateRepository AppraisalTemplate { get; private set; }
        public OfficeRepository Office { get; private set; }
        public AppraisalRepository Appraisal { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            this.context = context;
            Account = new AccountRepository(context);
            Resources = new ResourcesRepository(context);
            AppraisalTemplate = new AppraisalTemplateRepository(context);
            Office = new OfficeRepository(context);
            Appraisal = new AppraisalRepository(context);
        }
    }
}