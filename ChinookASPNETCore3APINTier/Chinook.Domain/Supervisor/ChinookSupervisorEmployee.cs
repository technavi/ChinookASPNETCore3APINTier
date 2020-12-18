using System;
using System.Collections.Generic;
using System.Linq;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;

using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<Employee> GetAllEmployee()
        {
            var employees = _employeeRepository.GetAll();
            foreach (var employee in employees)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Employee-", employee.EmployeeId), employee, cacheEntryOptions);
            }
            return employees;
        }

        public Employee GetEmployeeById(int id)
        {
            var employeeCached = _cache.Get<Employee>(string.Concat("Employee-", id));

            if (employeeCached != null)
            {
                return employeeCached;
            }
            else
            {
                var employee = (_employeeRepository.GetById(id));
                employee.Customers = (GetCustomerBySupportRepId(employee.EmployeeId)).ToList();
                //employee.DirectReports = (GetEmployeeDirectReports(employee.EmployeeId)).ToList();
                // employee.Manager = employee.ReportsTo.HasValue
                //     ? GetEmployeeReportsTo(id)
                //     : null;
                // if (employee.Manager != null)
                //     employee.ReportsToName = employee.ReportsTo.HasValue
                //         ? $"{employee.Manager.LastName}, {employee.Manager.FirstName}"
                //         : string.Empty;

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Employee-", employee.EmployeeId), employee, cacheEntryOptions);

                return employee;
            }
        }

        public Employee GetEmployeeReportsTo(int id)
        {
            var employee = _employeeRepository.GetReportsTo(id);
            return employee;
        }

        public Employee AddEmployee(Employee newEmployee)
        {
            var employee = newEmployee;

            employee = _employeeRepository.Add(employee);
            newEmployee.EmployeeId = employee.EmployeeId;
            return newEmployee;
        }

        public bool UpdateEmployee(Employee _employee)
        {
            var employee = _employeeRepository.GetById(_employee.EmployeeId);

            if (employee == null) return false;
            employee.EmployeeId = employee.EmployeeId;
            employee.LastName = employee.LastName;
            employee.FirstName = employee.FirstName;
            employee.Title = employee.Title;
            employee.ReportsTo = employee.ReportsTo;
            employee.BirthDate = employee.BirthDate;
            employee.HireDate = employee.HireDate;
            employee.Address = employee.Address;
            employee.City = employee.City;
            employee.State = employee.State;
            employee.Country = employee.Country;
            employee.PostalCode = employee.PostalCode;
            employee.Phone = employee.Phone;
            employee.Fax = employee.Fax;
            employee.Email = employee.Email;

            return _employeeRepository.Update(employee);
        }

        public bool DeleteEmployee(int id) 
            => _employeeRepository.Delete(id);

        public IEnumerable<Employee> GetEmployeeDirectReports(int id)
        {
            var employees = _employeeRepository.GetDirectReports(id);
            return employees;
        }

        public IEnumerable<Employee> GetDirectReports(int id)
        {
            var employees = _employeeRepository.GetDirectReports(id);
            return employees;
        }
    }
}