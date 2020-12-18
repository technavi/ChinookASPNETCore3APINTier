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
        public IEnumerable<Invoice> GetAllInvoice()
        {
            var invoices = _invoiceRepository.GetAll();
            foreach (var invoice in invoices)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Invoice-", invoice.InvoiceId), invoice, cacheEntryOptions);
            }
            return invoices;
        }
        
        public Invoice GetInvoiceById(int id)
        {
            var invoiceCached = _cache.Get<Invoice>(string.Concat("Invoice-", id));

            if (invoiceCached != null)
            {
                return invoiceCached;
            }
            else
            {
                var invoice = (_invoiceRepository.GetById(id));
                invoice.Customer = GetCustomerById(invoice.CustomerId);
                invoice.InvoiceLines = (GetInvoiceLineByInvoiceId(invoice.InvoiceId)).ToList();
                invoice.CustomerName =
                    $"{invoice.Customer.LastName}, {invoice.Customer.FirstName}";

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("Invoice-", invoice.InvoiceId), invoice, cacheEntryOptions);

                return invoice;
            }
        }

        public IEnumerable<Invoice> GetInvoiceByCustomerId(int id)
        {
            var invoices = _invoiceRepository.GetByCustomerId(id);
            return invoices;
        }

        public Invoice AddInvoice(Invoice newInvoice)
        {
            var invoice = newInvoice;

            invoice = _invoiceRepository.Add(invoice);
            newInvoice.InvoiceId = invoice.InvoiceId;
            return newInvoice;
        }

        public bool UpdateInvoice(Invoice _invoice)
        {
            var invoice = _invoiceRepository.GetById(_invoice.InvoiceId);

            if (invoice == null) return false;
            invoice.InvoiceId = invoice.InvoiceId;
            invoice.CustomerId = invoice.CustomerId;
            invoice.InvoiceDate = invoice.InvoiceDate;
            invoice.BillingAddress = invoice.BillingAddress;
            invoice.BillingCity = invoice.BillingCity;
            invoice.BillingState = invoice.BillingState;
            invoice.BillingCountry = invoice.BillingCountry;
            invoice.BillingPostalCode = invoice.BillingPostalCode;
            invoice.Total = invoice.Total;

            return _invoiceRepository.Update(invoice);
        }

        public bool DeleteInvoice(int id) 
            => _invoiceRepository.Delete(id);
        
        
        public IEnumerable<Invoice> GetInvoiceByEmployeeId(int id)
        {
            var invoices = _invoiceRepository.GetByEmployeeId(id);
            return invoices;
        }
    }
}