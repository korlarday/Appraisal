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
            if (viewModel.DepartmentName == "")
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

    }
}
