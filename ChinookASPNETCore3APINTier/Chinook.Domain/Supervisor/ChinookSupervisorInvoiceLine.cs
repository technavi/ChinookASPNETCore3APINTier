using System;
using System.Collections.Generic;
using Chinook.Domain.Entities;
using Chinook.Domain.Extensions;

using Microsoft.Extensions.Caching.Memory;

namespace Chinook.Domain.Supervisor
{
    public partial class ChinookSupervisor
    {
        public IEnumerable<InvoiceLine> GetAllInvoiceLine()
        {
            var invoiceLines = _invoiceLineRepository.GetAll();
            foreach (var invoiceLine in invoiceLines)
            {
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("InvoiceLine-", invoiceLine.InvoiceLineId), invoiceLine, cacheEntryOptions);
            }
            return invoiceLines;
        }

        public InvoiceLine GetInvoiceLineById(int id)
        {
            var invoiceLineCached = _cache.Get<InvoiceLine>(string.Concat("InvoiceLine-", id));

            if (invoiceLineCached != null)
            {
                return invoiceLineCached;
            }
            else
            {
                var invoiceLine = (_invoiceLineRepository.GetById(id));
                invoiceLine.Track = GetTrackById(invoiceLine.TrackId);
                invoiceLine.Invoice = GetInvoiceById(invoiceLine.InvoiceId);
                invoiceLine.TrackName = invoiceLine.Track.Name;

                var cacheEntryOptions =
                    new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(604800));
                _cache.Set(string.Concat("InvoiceLine-", invoiceLine.InvoiceLineId), invoiceLine, cacheEntryOptions);

                return invoiceLine;
            }
        }

        public IEnumerable<InvoiceLine> GetInvoiceLineByInvoiceId(int id)
        {
            var invoiceLines = _invoiceLineRepository.GetByInvoiceId(id);
            return invoiceLines;
        }

        public IEnumerable<InvoiceLine> GetInvoiceLineByTrackId(int id)
        {
            var invoiceLines = _invoiceLineRepository.GetByTrackId(id);
            return invoiceLines;
        }

        public InvoiceLine AddInvoiceLine(InvoiceLine newInvoiceLine)
        {
            var invoiceLine = newInvoiceLine;

            invoiceLine = _invoiceLineRepository.Add(invoiceLine);
            newInvoiceLine.InvoiceLineId = invoiceLine.InvoiceLineId;
            return newInvoiceLine;
        }

        public bool UpdateInvoiceLine(InvoiceLine _invoiceLine)
        {
            var invoiceLine = _invoiceLineRepository.GetById(_invoiceLine.InvoiceId);

            if (invoiceLine == null) return false;
            invoiceLine.InvoiceLineId = invoiceLine.InvoiceLineId;
            invoiceLine.InvoiceId = invoiceLine.InvoiceId;
            invoiceLine.TrackId = invoiceLine.TrackId;
            invoiceLine.UnitPrice = invoiceLine.UnitPrice;
            invoiceLine.Quantity = invoiceLine.Quantity;

            return _invoiceLineRepository.Update(invoiceLine);
        }

        public bool DeleteInvoiceLine(int id) 
            => _invoiceLineRepository.Delete(id);
    }
}