using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EfficientDesigner_Service.Contexts;
using EfficientDesigner_Service.Models;
using EfficientDesigner_Service.Services;

namespace EfficientDesigner_Service.ServiceImplements
{
    public class LayoutService : ILayoutService
    {
        public Layout[] GetLayouts()
        {
            using (var context = new LayoutContext())
            {
                return context.Layouts.OrderByDescending(x => x.CreateTime).ToArray();
            }
        }

        public void UpdateLayout(Layout layout)
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
