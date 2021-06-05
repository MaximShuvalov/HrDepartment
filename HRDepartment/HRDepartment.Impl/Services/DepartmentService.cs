using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;
using HRDepartment.Model.Api;

namespace HRDepartment.Impl.Services
{
    public sealed class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task Create(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            try
            {
                using (_unitOfWork)
                {
                    await _unitOfWork.GetRepositories<Department>().Create(department);
                    _unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<DepartmentDto>> GetAllDepartments()
        {
            List<Department> departments;
            var departmentDtos = new List<DepartmentDto>();
            using (_unitOfWork)
            {
                departments = await _unitOfWork.GetRepositories<Department>().GetAllAsync();
            }

            foreach (var department in departments)
                departmentDtos.Add(_mapper.Map<DepartmentDto>(department));
            return departmentDtos;
        }

        public async Task Delete(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Department>().Delete(department);
                _unitOfWork.Commit();
            }
        }

        public async Task Update(Department department)
        {
            if (department == null) throw new ArgumentException("Department is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<Department>().Update(department);
                _unitOfWork.Commit();
            }
        }

        public async Task<DepartmentDto> Get(long id)
        {
            using (_unitOfWork)
            {
                var department = await _unitOfWork.GetRepositories<Department>().Get(id);
                return _mapper.Map<DepartmentDto>(department);
            }
        }

        public async Task SetBoss(Employee employee, long idDepartment)
        {
            if (employee == null) throw new ArgumentException("Employee is null");
            using (_unitOfWork)
            {
                var department = await _unitOfWork.GetRepositories<Department>().Get(idDepartment);
                department.Boss = employee;
                await Update(department);
            }
        }

        public List<Employee> GetFiredEmployees(long keyDepartment)
        {
            Department department;
            using (_unitOfWork)
            {
                department = _unitOfWork.GetRepositories<Department>().Get(keyDepartment).Result;
            }

            return department.Employees.Where(p => p.Fired == true).ToList();
        }

        public List<Employee> GetEmployees(long keyDepartment)
        {
            Department department;
            using (_unitOfWork)
            {
                department = _unitOfWork.GetRepositories<Department>().Get(keyDepartment).Result;
            }

            return department.Employees.Where(p => p.Fired == false).ToList();
        }
    }
}