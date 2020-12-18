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
            public IEnumerable<Customer> GetAllCustomer()
            {
                var customers = _customerRepository.GetAll();
                foreach (var customer in customers)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                    _cache.Set(string.Concat("Customer-", customer.CustomerId), customer, cacheEntryOptions);
                }
                return customers;
            }

            public Customer GetCustomerById(int id)
            {
                var customerCached = _cache.Get<Customer>(string.Concat("Customer-", id));

                if (customerCached != null)
                {
                    return customerCached;
                }
                else
                {
                    var customer = (_customerRepository.GetById(id));
                    customer.Invoices = (GetInvoiceByCustomerId(customer.CustomerId)).ToList();
                    customer.SupportRep =
                        GetEmployeeById(customer.SupportRepId.GetValueOrDefault());
                    customer.SupportRepName =
                        $"{customer.SupportRep.LastName}, {customer.SupportRep.FirstName}";

                    var cacheEntryOptions =
                        new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                    _cache.Set(string.Concat("Customer-", customer.CustomerId), customer, cacheEntryOptions);

                    return customer;
                }
            }

            public IEnumerable<Customer> GetCustomerBySupportRepId(int id)
            {
                var customers = _customerRepository.GetBySupportRepId(id);
                return customers;
            }

            public Customer AddCustomer(Customer newCustomer)
            {
                /*var customer = new Customer
                {
                    FirstName = newCustomer.FirstName,
                    LastName = newCustomer.LastName,
                    Company = newCustomer.Company,
                    Address = newCustomer.Address,
                    City = newCustomer.City,
                    State = newCustomer.State,
                    Country = newCustomer.Country,
                    PostalCode = newCustomer.PostalCode,
                    Phone = newCustomer.Phone,
                    Fax = newCustomer.Fax,
                    Email = newCustomer.Email,
                    SupportRepId = newCustomer.SupportRepId
                };*/

                var customer = newCustomer;

                customer = _customerRepository.Add(customer);
                newCustomer.CustomerId = customer.CustomerId;
                return newCustomer;
            }

            public bool UpdateCustomer(Customer _customer)
            {
                var customer = _customerRepository.GetById(_customer.CustomerId);

                if (customer == null) return false;
                customer.FirstName = customer.FirstName;
                customer.LastName = customer.LastName;
                customer.Company = customer.Company;
                customer.Address = customer.Address;
                customer.City = customer.City;
                customer.State = customer.State;
                customer.Country = customer.Country;
                customer.PostalCode = customer.PostalCode;
                customer.Phone = customer.Phone;
                customer.Fax = customer.Fax;
                customer.Email = customer.Email;
                customer.SupportRepId = customer.SupportRepId;

                return _customerRepository.Update(customer);
            }

            public bool DeleteCustomer(int id) 
                => _customerRepository.Delete(id);
        }
    }