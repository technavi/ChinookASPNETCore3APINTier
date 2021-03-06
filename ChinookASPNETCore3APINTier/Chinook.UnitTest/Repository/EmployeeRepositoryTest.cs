﻿using Chinook.DataEFCore.Repositories;
using Chinook.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Chinook.UnitTest.Repository
{
    public class EmployeeRepositoryTest
    {
        private readonly IEmployeeRepository _repo;

        public EmployeeRepositoryTest()
        {
        }

        [Fact]
        public void EmployeeGetAll()
        {
            // Act
            var employees = _repo.GetAll();

            // Assert
            Assert.True(employees.Count > 1, "The number of employees was not greater than 1");
        }
    }
}