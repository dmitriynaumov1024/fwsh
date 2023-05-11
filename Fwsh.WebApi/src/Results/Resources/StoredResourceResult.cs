namespace Fwsh.WebApi.Results;

using System;
using Fwsh.Common;

// StoredResourceResult preserves its structure from datamodel-v2
//
public class StoredResourceResult : ResourceResult
{
    public int? SupplierId { get; set; }
    public string ExternalId { get; set; }

    public double InStock { get; set; }
    public double NormalStock { get; set; }

    public int RefillPeriodDays { get; set; }
    public DateTime LastRefilledAt { get; set; }
    public DateTime LastCheckedAt { get; set; }

    public bool IsTimeToRefill { get; set; }
    public bool NeedsRefill { get; set; }

    public SupplierResult Supplier { get; set; }

    public StoredResourceResult () { }

    public StoredResourceResult (Resource res) : base(res)
    { 
        if (res.Stored is StoredResource st) {
            this.SupplierId = st.SupplierId;
            this.ExternalId = st.ExternalId;
            this.InStock = st.InStock;
            this.NormalStock = st.NormalStock;
            this.RefillPeriodDays = st.RefillPeriodDays;
            this.LastRefilledAt = st.LastRefilledAt;
            this.LastCheckedAt = st.LastCheckedAt;
            this.IsTimeToRefill = st.IsTimeToRefill;
            this.NeedsRefill = st.NeedsRefill;

            if (st.Supplier != null) {
                this.Supplier = new SupplierResult(st.Supplier);
            }
        }
    }
}
