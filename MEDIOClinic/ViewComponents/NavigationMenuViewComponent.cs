using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using CMS.Core;
using CMS.DocumentEngine;
using CMS.DocumentEngine.Routing;

using Kentico.Content.Web.Mvc;

using MEDIOClinic.Models;




namespace CoreTutorial.ViewComponents
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        private readonly IPageRetriever pageRetriever;
        private readonly IPageUrlRetriever pageUrlRetriever;

        public NavigationMenuViewComponent(IPageRetriever pageRetriever, IPageUrlRetriever pageUrlRetriever)
        {
            this.pageRetriever = pageRetriever;
            this.pageUrlRetriever = pageUrlRetriever;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<TreeNode> menuItems = await pageRetriever.RetrieveAsync<TreeNode>(query => query
            .MenuItems()
            .NestingLevel(1)
            .Columns("DocumentName", "NodeID", "NodeSiteID")
            .WithPageUrlPaths()
            .OrderByAscending("NodeOrder"));

            IEnumerable<MenuItemViewModel> model = menuItems.Select(item => new MenuItemViewModel()
            {
                MenuItemText = item.DocumentName,
                MenuItemRelativeUrl = pageUrlRetriever.Retrieve(item).RelativePath
            });

            return View(model);
        }
    }
}
