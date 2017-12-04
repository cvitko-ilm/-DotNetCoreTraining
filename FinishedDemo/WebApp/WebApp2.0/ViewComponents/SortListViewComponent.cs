using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp2_0.Models;

namespace WebApp2_0.ViewComponents
{
    public class SortListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int maxItems)
        {
            List<UserModel> users = await Task.Run(() => new List<UserModel>() {
                new UserModel(){ Name = "Ted", Phone="12345"},
                new UserModel(){ Name = "Bill", Phone="54321"},
                new UserModel(){ Name = "Station", Phone="67890"},
            });

            return View(users.OrderBy(u => u.Name).Take(maxItems));
        }
    }
}
