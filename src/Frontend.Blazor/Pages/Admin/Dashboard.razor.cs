namespace BlazorShop.Web.Pages.Admin
{
    using Frontend.Blazor.Interop;
    using Microsoft.AspNetCore.Components;
    using System;
    using System.Threading.Tasks;

    public partial class Dashboard : ComponentBase, IAsyncDisposable
    {
        [Inject] private IAppJsInterop JsInterop { get; set; } = default!;

        protected override async Task OnInitializedAsync()
        {

        }

        private async Task ExportCsvAsync()
        {
            //TODO: Implement CSV export functionality
            //var header = "Created,Reference,OrderStatus,PaymentStatus,FulfillmentStatus,Total,Currency,Tracking";
            //var lines = _orders.Select(o => string.Join(',',
            //    o.CreatedOn.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
            //    Escape(o.Reference),
            //    Escape(o.OrderStatus),
            //    Escape(o.PaymentStatus),
            //    Escape(o.FulfillmentStatus),
            //    o.TotalAmount.ToString("F2"),
            //    Escape(o.Currency),
            //    Escape(!string.IsNullOrWhiteSpace(o.TrackingUrl) ? o.TrackingUrl! : o.TrackingNumber ?? "")));
            //var csv = string.Join("\n", new[] { header }.Concat(lines));
            //await JsInterop.DownloadFileAsync($"orders_{DateTime.UtcNow:yyyyMMddHHmm}.csv", csv, "text/csv;charset=utf-8");
        }

        public async ValueTask DisposeAsync()
        {
            if (JsInterop is null)
            {
                return;
            }

            await JsInterop.DisposeChartAsync("salesChart");
            await JsInterop.DisposeChartAsync("trafficChart");
        }
    }
}
