using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HRDepartment.Core.Services;
using HRDepartment.Core.UnitOfWork;
using HRDepartment.Model;
using HRDepartment.Model.DataBase;

namespace HRDepartment.Impl.Services
{
    public class EmployeeLogService : IEmployeeLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Create(EmployeeLog employeeLog)
        {
            if (employeeLog == null) throw new ArgumentException("EmployeeLog is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<EmployeeLog>().Create(employeeLog);
                _unitOfWork.Commit();
            }
        }

        public async Task<List<EmployeeLog>> GetAllEmployeeLogs()
        {
            using (_unitOfWork)
            {
                return await _unitOfWork.GetRepositories<EmployeeLog>().GetAllAsync();
            }
        }

        public async Task Delete(EmployeeLog employeeLog)
        {
            if (employeeLog == null) throw new ArgumentException("EmployeeLog is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<EmployeeLog>().Delete(employeeLog);
                _unitOfWork.Commit();
            }
        }

        public async Task Update(EmployeeLog employeeLog)
        {
            if (employeeLog == null) throw new ArgumentException("EmployeeLog is null");
            using (_unitOfWork)
            {
                await _unitOfWork.GetRepositories<EmployeeLog>().Update(employeeLog);
                _unitOfWork.Commit();
            }
        }

        public async Task<EmployeeLog> Get(long id)
        {
            using (_unitOfWork)
            {
                return await _unitOfWork.GetRepositories<EmployeeLog>().Get(id);
            }
        }
    }
}