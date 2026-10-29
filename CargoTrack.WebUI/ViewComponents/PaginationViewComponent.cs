using CargoTrack.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CargoTrack.WebUI.ViewComponents
{
    public class PaginationViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(PaginationViewModel pagination)
        {
            if (pagination.TotalPages <= 1)
                return Content(string.Empty);

            var start = Math.Max(1, pagination.Page - 2);
            var end = Math.Min(pagination.TotalPages, pagination.Page + 2);

            var model = new PagerViewModel
            {
                TotalCount = pagination.TotalCount,
                PreviousUrl = pagination.Page > 1 ? PageUrl(pagination.Page - 1) : null,
                NextUrl = pagination.Page < pagination.TotalPages ? PageUrl(pagination.Page + 1) : null,
                Pages = Enumerable.Range(start, end - start + 1)
                    .Select(i => new PageLinkViewModel(i, PageUrl(i), i == pagination.Page))
                    .ToList()
            };
            return View(model);
        }

        private string PageUrl(int page)
        {
            var query = Request.Query
                .Where(q => !q.Key.Equals("page", StringComparison.OrdinalIgnoreCase))
                .Select(q => new KeyValuePair<string, string?>(q.Key, q.Value.ToString()))
                .Append(new KeyValuePair<string, string?>("page", page.ToString()));

            return Request.Path + QueryString.Create(query);
        }
    }
}
