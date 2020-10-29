using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EfficientDesigner_Service.Contexts;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;
using Microsoft.EntityFrameworkCore;

namespace EfficientDesigner_Service.ServiceImplements
{
    public class LayoutService : ILayoutService
    {
        public async Task<Layout[]> GetLayouts()
        {
            using (var context = new LayoutContext())
            {
                return await context.Layouts.OrderByDescending(x => x.CreateTime).ToArrayAsync();
            }
        }

        public Layout UpdateLayout(Layout layout)
        {
            using (var context = new LayoutContext())
            {
                if (context.Layouts.Any(x => x.LayoutId == layout.LayoutId))
                {
                    context.Layouts.Update(layout);
                }
                else
                {
                    context.Layouts.Add(layout);
                }
                context.SaveChanges();
                return layout;
            }
        }

        public void RemoveLayout(Layout layout)
        {
            using (var context = new LayoutContext())
            {
                context.Layouts.Remove(layout);
                context.SaveChanges();
            }
        }
    }
}
