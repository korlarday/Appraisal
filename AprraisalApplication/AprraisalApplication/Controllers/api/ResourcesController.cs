using AprraisalApplication.Models;
using AprraisalApplication.Models.ApiModels;
using AprraisalApplication.Models.ApiParameters;
using AprraisalApplication.Models.MigrationModels;
using AprraisalApplication.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AprraisalApplication.Controllers.api
{
    [Authorize]
    public class ResourcesController : ApiController
    {
        public readonly ApplicationDbContext db;
        public readonly UnitOfWork _unitOfWork;
        public ResourcesController()
        {
            db = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(db);
        }

        public IHttpActionResult PostAddDepartment([FromBody] NewDepartmentParams model)
        {
            NewDepartmentVM viewModel = new NewDepartmentVM();

            // check if department name is not empty
            if (model.departmentName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if department name is unique
            if ((_unitOfWork.Resources.GetDepartmentByName(model.departmentName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new department
            Department newDept = new Department(model.departmentName);

            _unitOfWork.Resources.AddNewDepartment(newDept);

            // Construct the model
            viewModel.Id = newDept.Id;
            viewModel.DepartmentName = model.departmentName;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetAllDepartments().Count();

            return Ok(viewModel);
        }

        public IHttpActionResult PostEditDepartment([FromBody] NewDepartmentParams model)
        {
            NewDepartmentVM viewModel = new NewDepartmentVM();

            // check if department exist
            if ((_unitOfWork.Resources.GetDepartmentById(model.id)) == null)
            {
                viewModel.Feedback = "notfound";
            }

            // check if departmentname is not empty
            if (model.departmentName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if departmentname is unique
            if (_unitOfWork.Resources.IsDepartmentNameExists(model.id, model.departmentName))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the departent
            Department dept = _unitOfWork.Resources.UpdateDepartment(model.id, model.departmentName);
            
            // Construct the model

            viewModel.Id = dept.Id;
            viewModel.DepartmentName = dept.Name;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }
    
        public IHttpActionResult PostDeleteDepartment([FromBody] NewDepartmentParams model)
        {
            _unitOfWork.Resources.DeleteDepartment(model.id);
            return Ok();
        }
    
        
        public IHttpActionResult PostAddBranch([FromBody] BranchParams model)
        {
            NewBranchVM viewModel = new NewBranchVM();

            // check if branchname or state are not empty
            if (model.BranchName == "" || model.StateId < 1)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if branchname is unique
            if ((_unitOfWork.Resources.GetBranchByName(model.BranchName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new branch
            Branch newBranch = new Branch(model.BranchName, model.StateId);

            _unitOfWork.Resources.AddNewBranch(newBranch);

            //fetch the branch type
            State stateType = _unitOfWork.Resources.GetStateById(model.StateId);

            // Construct the model
            viewModel.Id = newBranch.Id;
            viewModel.BranchName = model.BranchName;
            viewModel.State = stateType.Description;
            viewModel.StateId = stateType.Id;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetAllBranches().Count();

            return Ok(viewModel);
        }
    
        public IHttpActionResult PostEditBranch([FromBody] BranchParams model)
        {
            NewBranchVM viewModel = new NewBranchVM();

            // check if branch exist
            if ((_unitOfWork.Resources.GetBranchById(model.Id)) == null)
            {
                viewModel.Feedback = "notfound";
                return Ok(viewModel);
            }

            // check if branchname or state are not empty
            if (model.BranchName == "" || model.StateId < 1)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if subjectname is unique
            if (_unitOfWork.Resources.IsBranchNameExists(model.BranchName, model.Id))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the subject
            Branch editBranch = _unitOfWork.Resources.UpdateBranch(model);
            // Construct the model

            // get subject type
            State state = _unitOfWork.Resources.GetStateById(model.StateId);

            viewModel.Id = editBranch.Id;
            viewModel.BranchName = editBranch.Description;
            viewModel.State = state.Description;
            viewModel.StateId = state.Id;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }

        public IHttpActionResult PostDeleteBranch([FromBody] NewDepartmentParams model)
        {
            _unitOfWork.Resources.DeleteBranch(model.id);
            return Ok();
        }


        public IHttpActionResult PostAddState([FromBody] StateParams model)
        {
            NewStateVM viewModel = new NewStateVM();

            // check if state name is not empty
            if (model.stateName == "" || model.stateName == null)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if state name is unique
            if ((_unitOfWork.Resources.GetStateByName(model.stateName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new department
            State newState = new State(model.stateName);

            _unitOfWork.Resources.AddNewState(newState);

            // Construct the model
            viewModel.Id = newState.Id;
            viewModel.StateName = model.stateName;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetAllStates().Count();

            return Ok(viewModel);
        }

        public IHttpActionResult PostEditState([FromBody] StateParams model)
        {
            NewStateVM viewModel = new NewStateVM();

            // check if state exist
            if ((_unitOfWork.Resources.GetStateById(model.id)) == null)
            {
                viewModel.Feedback = "notfound";
            }

            // check if statename is not empty
            if (model.stateName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if statename is unique
            if (_unitOfWork.Resources.IsStateNameExists(model.id, model.stateName))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the state
            State state = _unitOfWork.Resources.UpdateState(model.id, model.stateName);

            // Construct the model

            viewModel.Id = state.Id;
            viewModel.StateName = state.Description;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }

        public IHttpActionResult PostDeleteState([FromBody] StateParams model)
        {
            _unitOfWork.Resources.DeleteState(model.id);
            return Ok();
        }


        public IHttpActionResult PostAddJobTitle([FromBody] JobTitleParams model)
        {
            NewJobTitleVM viewModel = new NewJobTitleVM();

            // check if jobtitle name is not empty
            if (model.JobtitleName == "" || model.JobtitleName == null)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if jobtitle name is unique
            if ((_unitOfWork.Resources.GetJobTitleByName(model.JobtitleName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new jobtitle
            JobTitle newJobTitle = new JobTitle(model.JobtitleName);

            _unitOfWork.Resources.AddNewJobTitle(newJobTitle);

            // Construct the model
            viewModel.Id = newJobTitle.Id;
            viewModel.JobtitleName = model.JobtitleName;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetAllJobTitles().Count();

            return Ok(viewModel);
        }
        public IHttpActionResult PostEditJobTitle([FromBody] JobTitleParams model)
        {
            NewJobTitleVM viewModel = new NewJobTitleVM();

            // check if jobtitle exist
            if ((_unitOfWork.Resources.GetJobTitleById(model.Id)) == null)
            {
                viewModel.Feedback = "notfound";
            }

            // check if jobtitle name is not empty
            if (model.JobtitleName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if jobtitle name is unique
            if (_unitOfWork.Resources.IsJobTitleNameExists(model.Id, model.JobtitleName))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the jobtitle
            JobTitle jobtitle = _unitOfWork.Resources.UpdateJobTitle(model.Id, model.JobtitleName);

            // Construct the model
            viewModel.Id = jobtitle.Id;
            viewModel.JobtitleName = jobtitle.Name;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }
        public IHttpActionResult PostDeleteJobTitle([FromBody] StateParams model)
        {
            _unitOfWork.Resources.DeleteJobTitle(model.id);
            return Ok();
        }

        //
        public IHttpActionResult PostAddGrade([FromBody] GradeParams model)
        {
            NewGradeVM viewModel = new NewGradeVM();

            // check if grade name is not empty
            if (model.GradeName == "" || model.GradeName == null)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if grade name is unique
            if ((_unitOfWork.Resources.GetGradeByName(model.GradeName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new grade
            Grade newGrade = new Grade(model.GradeName);

            _unitOfWork.Resources.AddNewGrade(newGrade);

            // Construct the model
            viewModel.Id = newGrade.Id;
            viewModel.GradeName = model.GradeName;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetGrades().Count();

            return Ok(viewModel);
        }
        public IHttpActionResult PostEditGrade([FromBody] GradeParams model)
        {
            NewGradeVM viewModel = new NewGradeVM();

            // check if grade exist
            if ((_unitOfWork.Resources.GetGradeById(model.Id)) == null)
            {
                viewModel.Feedback = "notfound";
            }

            // check if name name is not empty
            if (model.GradeName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if grade name is unique
            if (_unitOfWork.Resources.IsGradeNameExists(model.Id, model.GradeName))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the grade name
            Grade grade = _unitOfWork.Resources.UpdateGrade(model.Id, model.GradeName);

            // Construct the model
            viewModel.Id = grade.Id;
            viewModel.GradeName = grade.Name;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }
        public IHttpActionResult PostDeleteGrade([FromBody] GradeParams model)
        {
            _unitOfWork.Resources.DeleteGrade(model.Id);
            return Ok();
        }


        //
        public IHttpActionResult PostAddQualification([FromBody] QualificationParams model)
        {
            NewQualificationVM viewModel = new NewQualificationVM();

            // check if qualification name is not empty
            if (model.QualificationName == "" || model.QualificationName == null)
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if qualification name is unique
            if ((_unitOfWork.Resources.GetQualificationByName(model.QualificationName)) != null)
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Store the new qualification
            Qualification newQualification = new Qualification(model.QualificationName);

            _unitOfWork.Resources.AddNewQualification(newQualification);

            // Construct the model
            viewModel.Id = newQualification.Id;
            viewModel.QualificationName = model.QualificationName;
            viewModel.Feedback = "success";
            viewModel.Index = _unitOfWork.Resources.GetAllQualifications().Count();

            return Ok(viewModel);
        }
        public IHttpActionResult PostEditQualification([FromBody] QualificationParams model)
        {
            NewQualificationVM viewModel = new NewQualificationVM();

            // check if qualification exist
            if ((_unitOfWork.Resources.GetQualificationById(model.Id)) == null)
            {
                viewModel.Feedback = "notfound";
            }

            // check if qualification name is not empty
            if (model.QualificationName == "")
            {
                viewModel.Feedback = "required";
                return Ok(viewModel);
            }

            // check if qualification name is unique
            if (_unitOfWork.Resources.IsQualificationNameExists(model.Id, model.QualificationName))
            {
                viewModel.Feedback = "taken";
                return Ok(viewModel);
            }

            // Update the qualification name
            Qualification qualification = _unitOfWork.Resources.UpdateQualification(model.Id, model.QualificationName);

            // Construct the model
            viewModel.Id = qualification.Id;
            viewModel.QualificationName = qualification.Name;
            viewModel.Feedback = "success";
            viewModel.Index = 3;

            return Ok(viewModel);
        }
        public IHttpActionResult PostDeleteQualification([FromBody] QualificationParams model)
        {
            _unitOfWork.Resources.DeleteQualification(model.Id);
            return Ok();
        }

    }
}
